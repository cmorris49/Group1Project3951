using MySqlConnector;
using TournamentManagerAPI.Contracts;
using TournamentManagerAPI.Infrastructure;

namespace TournamentManagerAPI.Endpoints
{
    /// <summary>
    /// Registers match-related API endpoints.
    /// </summary>
    public static class MatchEndpoints
    {
        /// <summary>
        /// Maps match endpoints.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The same endpoint route builder for chaining.</returns>
        public static IEndpointRouteBuilder MapMatchEndpoints(
            this IEndpointRouteBuilder app,
            string? connectionString)
        {
            /// <summary>
            /// Retrieves all matches for a tournament ordered by bracket round and match number.
            /// </summary>
            /// <param name="tournamentId">The identifier of the tournament.</param>
            /// <returns>A list of matches with team names, schedule data, status, scores, and bracket position.</returns>
            app.MapGet("/tournaments/{tournamentId:guid}/matches", async (Guid tournamentId) =>
            {
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                const string sql = """
                    SELECT m.Id,
                           m.TeamAId,
                           ta.Name AS TeamAName,
                           m.TeamBId,
                           tb.Name AS TeamBName,
                           m.ScheduledStart,
                           m.Status,
                           m.ScoreA,
                           m.ScoreB,
                           m.RoundNumber,
                           m.MatchNumber
                    FROM Matches m
                    INNER JOIN Bracket b ON b.Id = m.BracketId
                    LEFT JOIN Team ta ON ta.Id = m.TeamAId
                    LEFT JOIN Team tb ON tb.Id = m.TeamBId
                    WHERE b.TournamentId = @TournamentId
                    ORDER BY m.RoundNumber, m.MatchNumber
                    """;

                var matches = new List<MatchReadResponse>();

                await using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    matches.Add(new MatchReadResponse(
                        DbReaderHelpers.ReadId(reader, "Id"),
                        DbReaderHelpers.ReadNullableId(reader, "TeamAId"),
                        reader.IsDBNull(reader.GetOrdinal("TeamAName")) ? null : reader.GetString("TeamAName"),
                        DbReaderHelpers.ReadNullableId(reader, "TeamBId"),
                        reader.IsDBNull(reader.GetOrdinal("TeamBName")) ? null : reader.GetString("TeamBName"),
                        reader.IsDBNull(reader.GetOrdinal("ScheduledStart")) ? null : reader.GetDateTime("ScheduledStart"),
                        reader.GetString("Status"),
                        reader.IsDBNull(reader.GetOrdinal("ScoreA")) ? null : reader.GetInt32("ScoreA"),
                        reader.IsDBNull(reader.GetOrdinal("ScoreB")) ? null : reader.GetInt32("ScoreB"),
                        reader.GetInt32("RoundNumber"),
                        reader.GetInt32("MatchNumber")));
                }

                return Results.Ok(matches);
            });

            /// <summary>
            /// Records the final result of a match and advances the winning team to the next bracket match when applicable.
            /// </summary>
            /// <param name="matchId">The identifier of the match to update.</param>
            /// <param name="request">The result payload containing Team A and Team B scores.</param>
            /// <returns>A success response when result recording completes, or an error response if validation or persistence fails.</returns>
            app.MapPut("/matches/{matchId:guid}/result", async (Guid matchId, RecordMatchResultRequest request) =>
            {
                if (request.ScoreA < 0 || request.ScoreB < 0)
                {
                    return Results.BadRequest("Scores must be non-negative.");
                }

                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                await using var tx = await connection.BeginTransactionAsync();

                try
                {
                    const string loadSql = """
                        SELECT m.Id, m.BracketId, m.RoundNumber, m.MatchNumber, m.TeamAId, m.TeamBId,
                               b.TotalRounds, b.BracketType
                        FROM Matches m
                        INNER JOIN Bracket b ON b.Id = m.BracketId
                        WHERE m.Id = @MatchId
                        LIMIT 1
                        """;

                    string bracketId;
                    int roundNumber;
                    int matchNumber;
                    int totalRounds;
                    string bracketType;
                    string? teamAId;
                    string? teamBId;

                    await using (var loadCmd = new MySqlCommand(loadSql, connection, (MySqlTransaction)tx))
                    {
                        loadCmd.Parameters.AddWithValue("@MatchId", matchId.ToString());
                        await using var reader = await loadCmd.ExecuteReaderAsync();

                        if (!await reader.ReadAsync())
                        {
                            await tx.RollbackAsync();
                            return Results.NotFound("Match not found.");
                        }

                        bracketId = DbReaderHelpers.ReadId(reader, "BracketId");
                        roundNumber = reader.GetInt32("RoundNumber");
                        matchNumber = reader.GetInt32("MatchNumber");
                        totalRounds = reader.GetInt32("TotalRounds");
                        bracketType = reader.GetString("BracketType");
                        teamAId = DbReaderHelpers.ReadNullableId(reader, "TeamAId");
                        teamBId = DbReaderHelpers.ReadNullableId(reader, "TeamBId");
                    }

                    if (string.IsNullOrWhiteSpace(teamAId) || string.IsNullOrWhiteSpace(teamBId))
                    {
                        await tx.RollbackAsync();
                        return Results.BadRequest("Cannot record result until both teams are assigned.");
                    }

                    bool isRoundRobin = string.Equals(bracketType, "RoundRobin", StringComparison.OrdinalIgnoreCase);

                    if (!isRoundRobin && request.ScoreA == request.ScoreB)
                    {
                        await tx.RollbackAsync();
                        return Results.BadRequest("Single elimination matches cannot end in a tie.");
                    }

                    const string completeSql = """
                        UPDATE Matches
                        SET ScoreA = @ScoreA,
                            ScoreB = @ScoreB,
                            ScoreRecordedAt = UTC_TIMESTAMP(),
                            ScheduledStart = COALESCE(ScheduledStart, UTC_TIMESTAMP()),
                            Status = 'Complete'
                        WHERE Id = @MatchId
                        """;

                    await using (var completeCmd = new MySqlCommand(completeSql, connection, (MySqlTransaction)tx))
                    {
                        completeCmd.Parameters.AddWithValue("@ScoreA", request.ScoreA);
                        completeCmd.Parameters.AddWithValue("@ScoreB", request.ScoreB);
                        completeCmd.Parameters.AddWithValue("@MatchId", matchId.ToString());
                        await completeCmd.ExecuteNonQueryAsync();
                    }

                    if (!isRoundRobin && roundNumber < totalRounds)
                    {
                        string winnerTeamId = request.ScoreA > request.ScoreB ? teamAId : teamBId;

                        int nextRound = roundNumber + 1;
                        int nextMatchNumber = (matchNumber + 1) / 2;
                        bool goesToTeamA = (matchNumber % 2) == 1;

                        string nextSql = goesToTeamA
                            ? "UPDATE Matches SET TeamAId = @WinnerTeamId WHERE BracketId = @BracketId AND RoundNumber = @RoundNumber AND MatchNumber = @MatchNumber"
                            : "UPDATE Matches SET TeamBId = @WinnerTeamId WHERE BracketId = @BracketId AND RoundNumber = @RoundNumber AND MatchNumber = @MatchNumber";

                        await using var nextCmd = new MySqlCommand(nextSql, connection, (MySqlTransaction)tx);
                        nextCmd.Parameters.AddWithValue("@WinnerTeamId", winnerTeamId);
                        nextCmd.Parameters.AddWithValue("@BracketId", bracketId);
                        nextCmd.Parameters.AddWithValue("@RoundNumber", nextRound);
                        nextCmd.Parameters.AddWithValue("@MatchNumber", nextMatchNumber);
                        await nextCmd.ExecuteNonQueryAsync();
                    }

                    await tx.CommitAsync();
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    return Results.Problem($"Failed to record result: {ex.Message}");
                }
            });

            /// <summary>
            /// Schedules or reschedules a match start date and time unless the match is already complete.
            /// </summary>
            /// <param name="matchId">The identifier of the match to schedule.</param>
            /// <param name="request">The scheduling payload containing the target start date and time.</param>
            /// <returns>A success response when scheduling is applied, or a not found response when the match does not exist.</returns>
            app.MapPut("/matches/{matchId:guid}/schedule", async (Guid matchId, ScheduleMatchRequest request) =>
            {
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                const string sql = """
                    UPDATE Matches
                    SET ScheduledStart = @ScheduledStart,
                        Status = CASE WHEN Status = 'Complete' THEN Status ELSE 'Scheduled' END
                    WHERE Id = @MatchId
                    """;

                await using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ScheduledStart", request.ScheduledStart);
                cmd.Parameters.AddWithValue("@MatchId", matchId.ToString());

                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 0 ? Results.NotFound("Match not found.") : Results.Ok();
            });

            return app;
        }
    }
}