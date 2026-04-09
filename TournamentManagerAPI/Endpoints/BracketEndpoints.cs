using MySqlConnector;
using TournamentManagerAPI.Infrastructure;

namespace TournamentManagerAPI.Endpoints
{
    /// <summary>
    /// Registers bracket generation and Swiss round progression endpoints.
    /// </summary>
    public static class BracketEndpoints
    {
        /// <summary>
        /// Maps bracket endpoints.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The same endpoint route builder for chaining.</returns>
        public static IEndpointRouteBuilder MapBracketEndpoints(
            this IEndpointRouteBuilder app,
            string? connectionString)
        {
            /// <summary>
            /// Generates a bracket for a tournament and initializes all bracket matches.
            /// </summary>
            /// <param name="tournamentId">The identifier of the tournament.</param>
            /// <returns>A success message when generation completes, or an error response if generation fails.</returns>
            app.MapPost("/tournaments/{tournamentId:guid}/generate-bracket", async (Guid tournamentId) =>
            {
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                await using var tx = await connection.BeginTransactionAsync();

                try
                {
                    const string teamsSql = """
                        SELECT tm.Id, tm.Seed, tm.Name
                        FROM Team tm
                        INNER JOIN Division d ON d.Id = tm.DivisionId
                        WHERE d.TournamentId = @TournamentId
                        ORDER BY CASE WHEN tm.Seed = 0 THEN 2147483647 ELSE tm.Seed END, tm.Name
                        """;

                    var teamIds = new List<string>();

                    await using (var cmd = new MySqlCommand(teamsSql, connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
                        await using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            teamIds.Add(DbReaderHelpers.ReadId(reader, "Id"));
                        }
                    }

                    if (teamIds.Count < 2)
                    {
                        await tx.RollbackAsync();
                        return Results.BadRequest("Need at least 2 teams to generate a bracket.");
                    }

                    string bracketTypeStr;
                    await using (var cmd = new MySqlCommand(
                        "SELECT BracketType FROM Tournament WHERE Id = @TournamentId LIMIT 1",
                        connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
                        bracketTypeStr = (await cmd.ExecuteScalarAsync())?.ToString() ?? "SingleElimination";
                    }

                    const string upsertBracketSql = """
                        INSERT INTO Bracket (Id, TournamentId, BracketType, TotalRounds)
                        VALUES (UUID(), @TournamentId, @BracketType, 0)
                        ON DUPLICATE KEY UPDATE
                            BracketType = VALUES(BracketType),
                            TotalRounds = VALUES(TotalRounds)
                        """;

                    await using (var cmd = new MySqlCommand(upsertBracketSql, connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
                        cmd.Parameters.AddWithValue("@BracketType", bracketTypeStr);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    string bracketId;
                    await using (var cmd = new MySqlCommand(
                        "SELECT Id FROM Bracket WHERE TournamentId = @TournamentId LIMIT 1",
                        connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
                        bracketId = (await cmd.ExecuteScalarAsync())?.ToString()!;
                    }

                    await using (var clearCmd = new MySqlCommand(
                        "DELETE FROM Matches WHERE BracketId = @BracketId",
                        connection, (MySqlTransaction)tx))
                    {
                        clearCmd.Parameters.AddWithValue("@BracketId", bracketId);
                        await clearCmd.ExecuteNonQueryAsync();
                    }

                    if (string.Equals(bracketTypeStr, "RoundRobin", StringComparison.OrdinalIgnoreCase))
                    {
                        await GenerateRoundRobinMatchesAsync(connection, (MySqlTransaction)tx, bracketId, teamIds);
                    }
                    else if (string.Equals(bracketTypeStr, "Swiss", StringComparison.OrdinalIgnoreCase))
                    {
                        await GenerateSwissFirstRoundAsync(connection, (MySqlTransaction)tx, bracketId, teamIds);
                    }
                    else
                    {
                        await GenerateSingleEliminationMatchesAsync(connection, (MySqlTransaction)tx, bracketId, teamIds);
                    }

                    await tx.CommitAsync();
                    return Results.Ok("Bracket generated.");
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    return Results.Problem($"Failed to generate bracket: {ex}");
                }
            });

            /// <summary>
            /// Generates the next Swiss round by pairing teams with equal win counts, avoiding rematches.
            /// </summary>
            app.MapPost("/tournaments/{tournamentId:guid}/generate-next-round", async (Guid tournamentId) =>
            {
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                await using var tx = await connection.BeginTransactionAsync();

                try
                {
                    const string bracketSql = """
                        SELECT Id, TotalRounds, BracketType
                        FROM Bracket
                        WHERE TournamentId = @TournamentId
                        LIMIT 1
                        """;

                    string bracketId;
                    int totalRounds;
                    string bracketType;

                    await using (var cmd = new MySqlCommand(bracketSql, connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
                        await using var reader = await cmd.ExecuteReaderAsync();
                        if (!await reader.ReadAsync())
                        {
                            await tx.RollbackAsync();
                            return Results.NotFound("No bracket found for this tournament.");
                        }

                        bracketId = reader.GetValue(reader.GetOrdinal("Id")).ToString()!;
                        totalRounds = reader.GetInt32("TotalRounds");
                        bracketType = reader.GetString("BracketType");
                    }

                    if (!string.Equals(bracketType, "Swiss", StringComparison.OrdinalIgnoreCase))
                    {
                        await tx.RollbackAsync();
                        return Results.BadRequest("generate-next-round is only for Swiss brackets.");
                    }

                    int currentRound;
                    await using (var cmd = new MySqlCommand(
                        "SELECT COALESCE(MAX(RoundNumber), 0) FROM Matches WHERE BracketId = @BracketId",
                        connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@BracketId", bracketId);
                        currentRound = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }

                    if (currentRound >= totalRounds)
                    {
                        await tx.RollbackAsync();
                        return Results.BadRequest($"Tournament is already at the final round ({totalRounds}).");
                    }

                    int incomplete;
                    await using (var cmd = new MySqlCommand("""
                        SELECT COUNT(*) FROM Matches
                        WHERE BracketId = @BracketId AND RoundNumber = @Round AND Status != 'Complete'
                        """, connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@BracketId", bracketId);
                        cmd.Parameters.AddWithValue("@Round", currentRound);
                        incomplete = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }

                    if (incomplete > 0)
                    {
                        await tx.RollbackAsync();
                        return Results.BadRequest($"Round {currentRound} still has {incomplete} incomplete match(es).");
                    }

                    const string teamsSql = """
                        SELECT tm.Id FROM Team tm
                        INNER JOIN Division d ON d.Id = tm.DivisionId
                        WHERE d.TournamentId = @TournamentId
                        """;

                    var allTeamIds = new List<string>();
                    await using (var cmd = new MySqlCommand(teamsSql, connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
                        await using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            allTeamIds.Add(reader.GetValue(reader.GetOrdinal("Id")).ToString()!);
                        }
                    }

                    const string resultsSql = """
                        SELECT TeamAId, TeamBId, ScoreA, ScoreB FROM Matches
                        WHERE BracketId = @BracketId AND Status = 'Complete'
                        """;

                    var wins = allTeamIds.ToDictionary(id => id, _ => 0, StringComparer.OrdinalIgnoreCase);
                    var played = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    string PlayedKey(string a, string b) =>
                        string.Compare(a, b, StringComparison.OrdinalIgnoreCase) <= 0
                            ? $"{a}|{b}"
                            : $"{b}|{a}";

                    await using (var cmd = new MySqlCommand(resultsSql, connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@BracketId", bracketId);
                        await using var reader = await cmd.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            var tAOrd = reader.GetOrdinal("TeamAId");
                            var tBOrd = reader.GetOrdinal("TeamBId");
                            var tA = reader.IsDBNull(tAOrd) ? null : reader.GetValue(tAOrd).ToString();
                            var tB = reader.IsDBNull(tBOrd) ? null : reader.GetValue(tBOrd).ToString();
                            if (tA == null || tB == null)
                            {
                                continue;
                            }

                            int sA = reader.GetInt32("ScoreA");
                            int sB = reader.GetInt32("ScoreB");

                            played.Add(PlayedKey(tA, tB));

                            if (wins.ContainsKey(tA) && wins.ContainsKey(tB))
                            {
                                if (sA > sB)
                                {
                                    wins[tA]++;
                                }
                                else if (sB > sA)
                                {
                                    wins[tB]++;
                                }
                            }
                        }
                    }

                    var sortedTeams = allTeamIds
                        .OrderByDescending(id => wins.TryGetValue(id, out var w) ? w : 0)
                        .ToList();

                    var paired = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var newMatches = new List<(string A, string B)>();

                    for (int i = 0; i < sortedTeams.Count; i++)
                    {
                        string teamA = sortedTeams[i];
                        if (paired.Contains(teamA))
                        {
                            continue;
                        }

                        bool found = false;
                        for (int j = i + 1; j < sortedTeams.Count; j++)
                        {
                            string teamB = sortedTeams[j];
                            if (paired.Contains(teamB))
                            {
                                continue;
                            }

                            bool alreadyPlayed = played.Contains(PlayedKey(teamA, teamB));
                            if (alreadyPlayed)
                            {
                                continue;
                            }

                            newMatches.Add((teamA, teamB));
                            paired.Add(teamA);
                            paired.Add(teamB);
                            found = true;
                            break;
                        }

                        if (!found)
                        {
                            for (int j = i + 1; j < sortedTeams.Count; j++)
                            {
                                string teamB = sortedTeams[j];
                                if (paired.Contains(teamB))
                                {
                                    continue;
                                }

                                newMatches.Add((teamA, teamB));
                                paired.Add(teamA);
                                paired.Add(teamB);
                                break;
                            }
                        }
                    }

                    if (newMatches.Count == 0)
                    {
                        await tx.RollbackAsync();
                        return Results.BadRequest("No valid pairings could be generated.");
                    }

                    int nextRound = currentRound + 1;
                    const string insertSql = """
                        INSERT INTO Matches (Id, BracketId, TeamAId, TeamBId, Status, RoundNumber, MatchNumber)
                        VALUES (UUID(), @BracketId, @TeamAId, @TeamBId, 'Unscheduled', @RoundNumber, @MatchNumber)
                        """;

                    for (int i = 0; i < newMatches.Count; i++)
                    {
                        await using var cmd = new MySqlCommand(insertSql, connection, (MySqlTransaction)tx);
                        cmd.Parameters.AddWithValue("@BracketId", bracketId);
                        cmd.Parameters.AddWithValue("@TeamAId", newMatches[i].A);
                        cmd.Parameters.AddWithValue("@TeamBId", newMatches[i].B);
                        cmd.Parameters.AddWithValue("@RoundNumber", nextRound);
                        cmd.Parameters.AddWithValue("@MatchNumber", i + 1);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    await tx.CommitAsync();
                    return Results.Ok($"Round {nextRound} generated with {newMatches.Count} match(es).");
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    return Results.Problem($"Failed to generate next round: {ex.Message}");
                }
            });

            return app;
        }

        /// <summary>
        /// Generates Round Robin matches using the circle/rotation algorithm.
        /// </summary>
        private static async Task GenerateRoundRobinMatchesAsync(
            MySqlConnection connection,
            MySqlTransaction tx,
            string bracketId,
            List<string> teamIds)
        {
            bool hasBye = teamIds.Count % 2 != 0;
            var slots = new List<string?>(teamIds.Select(id => (string?)id));
            if (hasBye)
            {
                slots.Add(null);
            }

            int n = slots.Count;
            int totalRounds = n - 1;
            int matchesPerRound = n / 2;

            await using (var cmd = new MySqlCommand(
                "UPDATE Bracket SET TotalRounds = @TotalRounds WHERE Id = @BracketId",
                connection, tx))
            {
                cmd.Parameters.AddWithValue("@TotalRounds", totalRounds);
                cmd.Parameters.AddWithValue("@BracketId", bracketId);
                await cmd.ExecuteNonQueryAsync();
            }

            const string insertMatchSql = """
                INSERT INTO Matches (Id, BracketId, TeamAId, TeamBId, Status, RoundNumber, MatchNumber)
                VALUES (UUID(), @BracketId, @TeamAId, @TeamBId, 'Unscheduled', @RoundNumber, @MatchNumber)
                """;

            var rotation = new List<string?>(slots);

            for (int round = 1; round <= totalRounds; round++)
            {
                int matchNum = 0;
                for (int m = 0; m < matchesPerRound; m++)
                {
                    string? teamA = rotation[m];
                    string? teamB = rotation[n - 1 - m];

                    if (teamA == null || teamB == null)
                    {
                        continue;
                    }

                    matchNum++;
                    await using var cmd = new MySqlCommand(insertMatchSql, connection, tx);
                    cmd.Parameters.AddWithValue("@BracketId", bracketId);
                    cmd.Parameters.AddWithValue("@TeamAId", teamA);
                    cmd.Parameters.AddWithValue("@TeamBId", teamB);
                    cmd.Parameters.AddWithValue("@RoundNumber", round);
                    cmd.Parameters.AddWithValue("@MatchNumber", matchNum);
                    await cmd.ExecuteNonQueryAsync();
                }

                var last = rotation[n - 1];
                for (int i = n - 1; i > 1; i--)
                {
                    rotation[i] = rotation[i - 1];
                }

                rotation[1] = last;
            }
        }

        /// <summary>
        /// Generates a standard single-elimination bracket with automatic bye advancement.
        /// </summary>
        private static async Task GenerateSingleEliminationMatchesAsync(
            MySqlConnection connection,
            MySqlTransaction tx,
            string bracketId,
            List<string> teamIds)
        {
            int bracketSize = 1;
            while (bracketSize < teamIds.Count)
            {
                bracketSize *= 2;
            }

            int totalRounds = (int)Math.Log2(bracketSize);

            await using (var cmd = new MySqlCommand(
                "UPDATE Bracket SET TotalRounds = @TotalRounds WHERE Id = @BracketId",
                connection, tx))
            {
                cmd.Parameters.AddWithValue("@TotalRounds", totalRounds);
                cmd.Parameters.AddWithValue("@BracketId", bracketId);
                await cmd.ExecuteNonQueryAsync();
            }

            const string insertMatchSql = """
                INSERT INTO Matches (Id, BracketId, TeamAId, TeamBId, Status, RoundNumber, MatchNumber)
                VALUES (UUID(), @BracketId, @TeamAId, @TeamBId, @Status, @RoundNumber, @MatchNumber)
                """;

            var slots = new List<string?>(new string?[bracketSize]);
            for (int i = 0; i < teamIds.Count; i++)
            {
                slots[i] = teamIds[i];
            }

            int firstRoundMatches = bracketSize / 2;
            for (int m = 0; m < firstRoundMatches; m++)
            {
                string? teamAId = slots[m * 2];
                string? teamBId = slots[(m * 2) + 1];

                await using var cmd = new MySqlCommand(insertMatchSql, connection, tx);
                cmd.Parameters.AddWithValue("@BracketId", bracketId);
                cmd.Parameters.AddWithValue("@TeamAId", (object?)teamAId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TeamBId", (object?)teamBId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", "Unscheduled");
                cmd.Parameters.AddWithValue("@RoundNumber", 1);
                cmd.Parameters.AddWithValue("@MatchNumber", m + 1);
                await cmd.ExecuteNonQueryAsync();
            }

            for (int round = 2; round <= totalRounds; round++)
            {
                int matchesInRound = bracketSize / (int)Math.Pow(2, round);
                for (int m = 1; m <= matchesInRound; m++)
                {
                    await using var cmd = new MySqlCommand(insertMatchSql, connection, tx);
                    cmd.Parameters.AddWithValue("@BracketId", bracketId);
                    cmd.Parameters.AddWithValue("@TeamAId", DBNull.Value);
                    cmd.Parameters.AddWithValue("@TeamBId", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", "Unscheduled");
                    cmd.Parameters.AddWithValue("@RoundNumber", round);
                    cmd.Parameters.AddWithValue("@MatchNumber", m);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            for (int round = 1; round < totalRounds; round++)
            {
                const string roundSql = """
                    SELECT Id, RoundNumber, MatchNumber, TeamAId, TeamBId, Status
                    FROM Matches
                    WHERE BracketId = @BracketId AND RoundNumber = @RoundNumber
                    ORDER BY MatchNumber
                    """;

                var byeWinners = new List<(int MatchNumber, string WinnerTeamId, bool winnerIsTeamA, string MatchId)>();

                await using (var readCmd = new MySqlCommand(roundSql, connection, tx))
                {
                    readCmd.Parameters.AddWithValue("@BracketId", bracketId);
                    readCmd.Parameters.AddWithValue("@RoundNumber", round);

                    await using var reader = await readCmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var teamAOrd = reader.GetOrdinal("TeamAId");
                        var teamBOrd = reader.GetOrdinal("TeamBId");
                        var teamA = reader.IsDBNull(teamAOrd) ? null : reader.GetValue(teamAOrd).ToString();
                        var teamB = reader.IsDBNull(teamBOrd) ? null : reader.GetValue(teamBOrd).ToString();
                        var status = reader.GetString("Status");

                        if (string.Equals(status, "Complete", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        string matchId = reader.GetValue(reader.GetOrdinal("Id")).ToString()!;

                        if (!string.IsNullOrWhiteSpace(teamA) && string.IsNullOrWhiteSpace(teamB))
                        {
                            byeWinners.Add((reader.GetInt32("MatchNumber"), teamA!, true, matchId));
                        }
                        else if (string.IsNullOrWhiteSpace(teamA) && !string.IsNullOrWhiteSpace(teamB))
                        {
                            byeWinners.Add((reader.GetInt32("MatchNumber"), teamB!, false, matchId));
                        }
                    }
                }

                foreach (var bye in byeWinners)
                {
                    const string completeByeSql = """
                        UPDATE Matches
                        SET ScoreA = @ScoreA,
                            ScoreB = @ScoreB,
                            ScoreRecordedAt = UTC_TIMESTAMP(),
                            ScheduledStart = COALESCE(ScheduledStart, UTC_TIMESTAMP()),
                            Status = 'Complete'
                        WHERE Id = @MatchId
                        """;

                    await using (var completeCmd = new MySqlCommand(completeByeSql, connection, tx))
                    {
                        completeCmd.Parameters.AddWithValue("@ScoreA", bye.winnerIsTeamA ? 1 : 0);
                        completeCmd.Parameters.AddWithValue("@ScoreB", bye.winnerIsTeamA ? 0 : 1);
                        completeCmd.Parameters.AddWithValue("@MatchId", bye.MatchId);
                        await completeCmd.ExecuteNonQueryAsync();
                    }

                    int nextRound = round + 1;
                    int nextMatch = (bye.MatchNumber + 1) / 2;
                    bool toTeamA = (bye.MatchNumber % 2) == 1;

                    string nextSql = toTeamA
                        ? "UPDATE Matches SET TeamAId = @WinnerTeamId WHERE BracketId = @BracketId AND RoundNumber = @RoundNumber AND MatchNumber = @MatchNumber"
                        : "UPDATE Matches SET TeamBId = @WinnerTeamId WHERE BracketId = @BracketId AND RoundNumber = @RoundNumber AND MatchNumber = @MatchNumber";

                    await using var nextCmd = new MySqlCommand(nextSql, connection, tx);
                    nextCmd.Parameters.AddWithValue("@WinnerTeamId", bye.WinnerTeamId);
                    nextCmd.Parameters.AddWithValue("@BracketId", bracketId);
                    nextCmd.Parameters.AddWithValue("@RoundNumber", nextRound);
                    nextCmd.Parameters.AddWithValue("@MatchNumber", nextMatch);
                    await nextCmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Generates Round 1 of a Swiss bracket using seed order pairing (1v2, 3v4, ...).
        /// </summary>
        private static async Task GenerateSwissFirstRoundAsync(
            MySqlConnection connection,
            MySqlTransaction tx,
            string bracketId,
            List<string> teamIds)
        {
            int n = teamIds.Count;
            int totalRounds = (int)Math.Ceiling(Math.Log2(n));

            await using (var cmd = new MySqlCommand(
                "UPDATE Bracket SET TotalRounds = @TotalRounds WHERE Id = @BracketId",
                connection, tx))
            {
                cmd.Parameters.AddWithValue("@TotalRounds", totalRounds);
                cmd.Parameters.AddWithValue("@BracketId", bracketId);
                await cmd.ExecuteNonQueryAsync();
            }

            const string insertSql = """
                INSERT INTO Matches (Id, BracketId, TeamAId, TeamBId, Status, RoundNumber, MatchNumber)
                VALUES (UUID(), @BracketId, @TeamAId, @TeamBId, 'Unscheduled', 1, @MatchNumber)
                """;

            int matchNum = 0;
            for (int i = 0; i < n - 1; i += 2)
            {
                matchNum++;
                await using var cmd = new MySqlCommand(insertSql, connection, tx);
                cmd.Parameters.AddWithValue("@BracketId", bracketId);
                cmd.Parameters.AddWithValue("@TeamAId", teamIds[i]);
                cmd.Parameters.AddWithValue("@TeamBId", teamIds[i + 1]);
                cmd.Parameters.AddWithValue("@MatchNumber", matchNum);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}