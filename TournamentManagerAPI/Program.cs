using MySqlConnector;

/// <summary>
/// Group 1 Project - TournamentManagerAPI Program
/// Author: Cameron, Jun, Jonathan 
/// Date: March 24, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     ASP.NET Core minimal API info https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis
/// </summary>

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var app = builder.Build();

/// <summary>
/// Reads a required identifier column from a data reader and converts it to string.
/// </summary>
/// <param name="reader">The active data reader.</param>
/// <param name="columnName">The identifier column name.</param>
/// <returns>The identifier as a string.</returns>
static string ReadId(MySqlDataReader reader, string columnName)
{
    return reader.GetValue(reader.GetOrdinal(columnName)).ToString()!;
}

/// <summary>
/// Reads a nullable identifier column from a data reader and converts it to string when present.
/// </summary>
/// <param name="reader">The active data reader.</param>
/// <param name="columnName">The identifier column name.</param>
/// <returns>The identifier as a string, or null when the database value is null.</returns>
static string? ReadNullableId(MySqlDataReader reader, string columnName)
{
    int ordinal = reader.GetOrdinal(columnName);
    return reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal).ToString();
}

/// <summary>
/// Verifies database connectivity by returning the current number of tournaments.
/// </summary>
/// <returns>A success response containing the tournament count if the database is reachable.</returns>
app.MapGet("/test-db", async () =>
{
    await using MySqlConnection connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    await using MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM Tournament", connection);
    object? result = await command.ExecuteScalarAsync();

    return Results.Ok($"Connected. Tournament count = {result}");
});

/// <summary>
/// Retrieves all tournaments ordered by start date.
/// </summary>
/// <returns>A list of tournament records including identifier, name, location, start date, and bracket type.</returns>
app.MapGet("/tournaments", async () =>
{
    List<object> tournaments = new List<object>();

    await using MySqlConnection connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string sql = """
        SELECT Id, Name, Location, StartDate, BracketType
        FROM Tournament
        ORDER BY StartDate
        """;

    await using MySqlCommand command = new MySqlCommand(sql, connection);
    await using MySqlDataReader reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        tournaments.Add(new
        {
            Id = reader.GetValue(reader.GetOrdinal("Id")).ToString(),
            Name = reader.GetString("Name"),
            Location = reader.IsDBNull(reader.GetOrdinal("Location"))
                ? null
                : reader.GetString("Location"),
            StartDate = reader.GetDateTime("StartDate"),
            BracketType = reader.GetString("BracketType")
        });
    }

    return Results.Ok(tournaments);
});

/// <summary>
/// Creates a new tournament and its default division within a single transaction.
/// </summary>
/// <param name="request">The tournament creation payload containing name, location, start date, and bracket type.</param>
/// <returns>The newly created tournament and default division identifiers, or an error response if creation fails.</returns>
app.MapPost("/tournaments", async (TournamentCreateRequest request) =>
{
    await using MySqlConnection connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    await using var tx = await connection.BeginTransactionAsync();

    try
    {
        var tournamentId = Guid.NewGuid().ToString();
        var divisionId = Guid.NewGuid().ToString();

        const string insertTournamentSql = """
            INSERT INTO Tournament (Id, Name, Location, StartDate, BracketType)
            VALUES (@Id, @Name, @Location, @StartDate, @BracketType)
            """;

        await using (var cmd = new MySqlCommand(insertTournamentSql, connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@Id", tournamentId);
            cmd.Parameters.AddWithValue("@Name", request.Name);
            cmd.Parameters.AddWithValue("@Location",
                string.IsNullOrWhiteSpace(request.Location) ? DBNull.Value : request.Location);
            cmd.Parameters.AddWithValue("@StartDate", request.StartDate);
            cmd.Parameters.AddWithValue("@BracketType", request.BracketType);
            await cmd.ExecuteNonQueryAsync();
        }

        const string insertDivisionSql = """
            INSERT INTO Division (Id, TournamentId, Name)
            VALUES (@Id, @TournamentId, @Name)
            """;

        await using (var cmd = new MySqlCommand(insertDivisionSql, connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@Id", divisionId);
            cmd.Parameters.AddWithValue("@TournamentId", tournamentId);
            cmd.Parameters.AddWithValue("@Name", "Main Division");
            await cmd.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
        return Results.Ok(new { TournamentId = tournamentId, DivisionId = divisionId });
    }
    catch (Exception ex)
    {
        await tx.RollbackAsync();
        return Results.Problem($"Failed to create tournament: {ex.Message}");
    }
});

/// <summary>
/// Creates a team under the tournament's primary division and optionally inserts its players.
/// </summary>
/// <param name="tournamentId">The identifier of the tournament to add the team to.</param>
/// <param name="request">The team creation payload containing team name, seed, and optional players.</param>
/// <returns>A created response with the new team identifier, or an error response if creation fails.</returns>
app.MapPost("/tournaments/{tournamentId:guid}/teams", async (Guid tournamentId, TeamCreateForTournamentRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest("Team name is required.");
    }

    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();
    await using var transaction = await connection.BeginTransactionAsync();

    try
    {
        const string getDivisionSql = """
            SELECT Id
            FROM Division
            WHERE TournamentId = @TournamentId
            ORDER BY Name
            LIMIT 1
            """;

        string? divisionId;
        await using (var divisionCmd = new MySqlCommand(getDivisionSql, connection, (MySqlTransaction)transaction))
        {
            divisionCmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
            divisionId = (await divisionCmd.ExecuteScalarAsync())?.ToString();
        }

        if (string.IsNullOrWhiteSpace(divisionId))
        {
            divisionId = Guid.NewGuid().ToString();

            const string createDivisionSql = """
                INSERT INTO Division (Id, TournamentId, Name)
                VALUES (@Id, @TournamentId, @Name)
                """;

            await using var createDivisionCmd = new MySqlCommand(createDivisionSql, connection, (MySqlTransaction)transaction);
            createDivisionCmd.Parameters.AddWithValue("@Id", divisionId);
            createDivisionCmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
            createDivisionCmd.Parameters.AddWithValue("@Name", "Main Division");
            await createDivisionCmd.ExecuteNonQueryAsync();
        }

        var teamId = Guid.NewGuid().ToString();

        const string insertTeamSql = """
            INSERT INTO Team (Id, DivisionId, Name, Seed)
            VALUES (@Id, @DivisionId, @Name, @Seed)
            """;

        await using (var teamCmd = new MySqlCommand(insertTeamSql, connection, (MySqlTransaction)transaction))
        {
            teamCmd.Parameters.AddWithValue("@Id", teamId);
            teamCmd.Parameters.AddWithValue("@DivisionId", divisionId);
            teamCmd.Parameters.AddWithValue("@Name", request.Name.Trim());
            teamCmd.Parameters.AddWithValue("@Seed", request.Seed);
            await teamCmd.ExecuteNonQueryAsync();
        }

        if (request.Players is not null)
        {
            const string insertPlayerSql = """
                INSERT INTO Player (Id, TeamId, DisplayName, Number)
                VALUES (@Id, @TeamId, @DisplayName, @Number)
                """;

            foreach (var player in request.Players)
            {
                if (string.IsNullOrWhiteSpace(player.DisplayName))
                {
                    continue;
                }

                await using var playerCmd = new MySqlCommand(insertPlayerSql, connection, (MySqlTransaction)transaction);
                playerCmd.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                playerCmd.Parameters.AddWithValue("@TeamId", teamId);
                playerCmd.Parameters.AddWithValue("@DisplayName", player.DisplayName.Trim());
                playerCmd.Parameters.AddWithValue("@Number", player.Number);
                await playerCmd.ExecuteNonQueryAsync();
            }
        }

        await transaction.CommitAsync();
        return Results.Created($"/teams/{teamId}", new { Id = teamId });
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        return Results.Problem($"Failed to create team: {ex.Message}");
    }
});

/// <summary>
/// Retrieves aggregate dashboard counts for teams, players, and matches in a tournament.
/// </summary>
/// <param name="tournamentId">The identifier of the tournament.</param>
/// <returns>Dashboard summary counts for the specified tournament.</returns>
app.MapGet("/tournaments/{tournamentId:guid}/dashboard", async (Guid tournamentId) =>
{
    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string teamCountSql = """
        SELECT COUNT(*)
        FROM Team tm
        INNER JOIN Division d ON d.Id = tm.DivisionId
        WHERE d.TournamentId = @TournamentId
        """;

    const string playerCountSql = """
        SELECT COUNT(*)
        FROM Player p
        INNER JOIN Team tm ON tm.Id = p.TeamId
        INNER JOIN Division d ON d.Id = tm.DivisionId
        WHERE d.TournamentId = @TournamentId
        """;

    const string matchCountSql = """
        SELECT COUNT(*)
        FROM Matches m
        INNER JOIN Bracket b ON b.Id = m.BracketId
        WHERE b.TournamentId = @TournamentId
        """;

    int teams;
    int players;
    int matches;

    await using (var cmd = new MySqlCommand(teamCountSql, connection))
    {
        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
        teams = Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    await using (var cmd = new MySqlCommand(playerCountSql, connection))
    {
        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
        players = Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    await using (var cmd = new MySqlCommand(matchCountSql, connection))
    {
        cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
        matches = Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    return Results.Ok(new { Teams = teams, Players = players, Matches = matches });
});

/// <summary>
/// Retrieves all teams in a tournament, including nested player rosters.
/// </summary>
/// <param name="tournamentId">The identifier of the tournament.</param>
/// <returns>A list of teams with division metadata and player collections.</returns>
app.MapGet("/tournaments/{tournamentId:guid}/teams", async (Guid tournamentId) =>
{
    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string teamsSql = """
        SELECT tm.Id, tm.Name, tm.Seed, d.Id AS DivisionId, d.Name AS DivisionName
        FROM Team tm
        INNER JOIN Division d ON d.Id = tm.DivisionId
        WHERE d.TournamentId = @TournamentId
        ORDER BY d.Name, tm.Seed, tm.Name
        """;

    var teams = new List<TeamReadResponse>();
    var teamMap = new Dictionary<string, TeamReadResponse>(StringComparer.OrdinalIgnoreCase);

    await using (var teamCmd = new MySqlCommand(teamsSql, connection))
    {
        teamCmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
        await using var reader = await teamCmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var team = new TeamReadResponse(
                reader.GetValue(reader.GetOrdinal("Id")).ToString()!,
                reader.GetString("Name"),
                reader.GetInt32("Seed"),
                reader.GetValue(reader.GetOrdinal("DivisionId")).ToString()!,
                reader.GetString("DivisionName"),
                new List<PlayerReadResponse>());

            teams.Add(team);
            teamMap[team.Id] = team;
        }
    }

    const string playersSql = """
        SELECT p.Id, p.TeamId, p.DisplayName, p.Number
        FROM Player p
        INNER JOIN Team tm ON tm.Id = p.TeamId
        INNER JOIN Division d ON d.Id = tm.DivisionId
        WHERE d.TournamentId = @TournamentId
        ORDER BY p.DisplayName
        """;

    await using (var playerCmd = new MySqlCommand(playersSql, connection))
    {
        playerCmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
        await using var reader = await playerCmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var teamId = reader.GetValue(reader.GetOrdinal("TeamId")).ToString()!;
            if (teamMap.TryGetValue(teamId, out var team))
            {
                team.Players.Add(new PlayerReadResponse(
                    reader.GetValue(reader.GetOrdinal("Id")).ToString()!,
                    reader.GetString("DisplayName"),
                    reader.GetInt32("Number")));
            }
        }
    }

    return Results.Ok(teams);
});

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
            ReadId(reader, "Id"),
            ReadNullableId(reader, "TeamAId"),
            reader.IsDBNull(reader.GetOrdinal("TeamAName")) ? null : reader.GetString("TeamAName"),
            ReadNullableId(reader, "TeamBId"),
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
/// Generates a single-elimination bracket for a tournament and initializes all bracket matches.
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
                teamIds.Add(ReadId(reader, "Id"));
            }
        }

        if (teamIds.Count < 2)
        {
            await tx.RollbackAsync();
            return Results.BadRequest("Need at least 2 teams to generate a bracket.");
        }

        int bracketSize = 1;
        while (bracketSize < teamIds.Count)
        {
            bracketSize *= 2;
        }

        int totalRounds = (int)Math.Log2(bracketSize);

        const string upsertBracketSql = """
            INSERT INTO Bracket (Id, TournamentId, BracketType, TotalRounds)
            VALUES (UUID(), @TournamentId, 'SingleElimination', @TotalRounds)
            ON DUPLICATE KEY UPDATE
                BracketType = VALUES(BracketType),
                TotalRounds = VALUES(TotalRounds)
            """;

        await using (var cmd = new MySqlCommand(upsertBracketSql, connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
            cmd.Parameters.AddWithValue("@TotalRounds", totalRounds);
            await cmd.ExecuteNonQueryAsync();
        }

        const string getBracketSql = "SELECT Id FROM Bracket WHERE TournamentId = @TournamentId LIMIT 1";
        string bracketId;
        await using (var cmd = new MySqlCommand(getBracketSql, connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
            bracketId = (await cmd.ExecuteScalarAsync())?.ToString()!;
        }

        await using (var clearCmd = new MySqlCommand("DELETE FROM Matches WHERE BracketId = @BracketId", connection, (MySqlTransaction)tx))
        {
            clearCmd.Parameters.AddWithValue("@BracketId", bracketId);
            await clearCmd.ExecuteNonQueryAsync();
        }

        const string insertMatchSql = """
            INSERT INTO Matches (Id, BracketId, TeamAId, TeamBId, Status, RoundNumber, MatchNumber)
            VALUES (UUID(), @BracketId, @TeamAId, @TeamBId, @Status, @RoundNumber, @MatchNumber)
            """;

        // Fill power-of-two slots with null byes at the end
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

            await using var cmd = new MySqlCommand(insertMatchSql, connection, (MySqlTransaction)tx);
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
                await using var cmd = new MySqlCommand(insertMatchSql, connection, (MySqlTransaction)tx);
                cmd.Parameters.AddWithValue("@BracketId", bracketId);
                cmd.Parameters.AddWithValue("@TeamAId", DBNull.Value);
                cmd.Parameters.AddWithValue("@TeamBId", DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", "Unscheduled");
                cmd.Parameters.AddWithValue("@RoundNumber", round);
                cmd.Parameters.AddWithValue("@MatchNumber", m);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Auto-advance byes
        for (int round = 1; round < totalRounds; round++)
        {
            const string roundSql = """
                SELECT Id, RoundNumber, MatchNumber, TeamAId, TeamBId, Status
                FROM Matches
                WHERE BracketId = @BracketId AND RoundNumber = @RoundNumber
                ORDER BY MatchNumber
                """;

            var byeWinners = new List<(int MatchNumber, string WinnerTeamId, bool winnerIsTeamA, string MatchId)>();

            await using (var readCmd = new MySqlCommand(roundSql, connection, (MySqlTransaction)tx))
            {
                readCmd.Parameters.AddWithValue("@BracketId", bracketId);
                readCmd.Parameters.AddWithValue("@RoundNumber", round);

                await using var reader = await readCmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var teamA = ReadNullableId(reader, "TeamAId");
                    var teamB = ReadNullableId(reader, "TeamBId");
                    var status = reader.GetString("Status");

                    if (string.Equals(status, "Complete", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(teamA) && string.IsNullOrWhiteSpace(teamB))
                    {
                        byeWinners.Add((reader.GetInt32("MatchNumber"), teamA, true, ReadId(reader, "Id")));
                    }
                    else if (string.IsNullOrWhiteSpace(teamA) && !string.IsNullOrWhiteSpace(teamB))
                    {
                        byeWinners.Add((reader.GetInt32("MatchNumber"), teamB, false, ReadId(reader, "Id")));
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

                await using (var completeCmd = new MySqlCommand(completeByeSql, connection, (MySqlTransaction)tx))
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

                await using var nextCmd = new MySqlCommand(nextSql, connection, (MySqlTransaction)tx);
                nextCmd.Parameters.AddWithValue("@WinnerTeamId", bye.WinnerTeamId);
                nextCmd.Parameters.AddWithValue("@BracketId", bracketId);
                nextCmd.Parameters.AddWithValue("@RoundNumber", nextRound);
                nextCmd.Parameters.AddWithValue("@MatchNumber", nextMatch);
                await nextCmd.ExecuteNonQueryAsync();
            }
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

    if (request.ScoreA == request.ScoreB)
    {
        return Results.BadRequest("Single elimination matches cannot end in a tie.");
    }

    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();
    await using var tx = await connection.BeginTransactionAsync();

    try
    {
        const string loadSql = """
            SELECT m.Id, m.BracketId, m.RoundNumber, m.MatchNumber, m.TeamAId, m.TeamBId, b.TotalRounds
            FROM Matches m
            INNER JOIN Bracket b ON b.Id = m.BracketId
            WHERE m.Id = @MatchId
            LIMIT 1
            """;

        string bracketId;
        int roundNumber;
        int matchNumber;
        int totalRounds;
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

            bracketId = ReadId(reader, "BracketId");
            roundNumber = reader.GetInt32("RoundNumber");
            matchNumber = reader.GetInt32("MatchNumber");
            totalRounds = reader.GetInt32("TotalRounds");
            teamAId = ReadNullableId(reader, "TeamAId");
            teamBId = ReadNullableId(reader, "TeamBId");
        }

        if (string.IsNullOrWhiteSpace(teamAId) || string.IsNullOrWhiteSpace(teamBId))
        {
            await tx.RollbackAsync();
            return Results.BadRequest("Cannot record result until both teams are assigned.");
        }

        string winnerTeamId = request.ScoreA > request.ScoreB ? teamAId : teamBId;

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

        if (roundNumber < totalRounds)
        {
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

app.Run();

record TournamentCreateRequest(string Name, string? Location, DateTime StartDate, string BracketType);
record TeamCreateRequest(Guid DivisionId, string Name, int Seed, List<PlayerCreateRequest>? Players);
record PlayerCreateRequest(string DisplayName, int Number);
record TeamCreateForTournamentRequest(string Name, int Seed, List<PlayerCreateRequest>? Players);
record TeamReadResponse(string Id, string Name, int Seed, string DivisionId, string DivisionName, List<PlayerReadResponse> Players);
record PlayerReadResponse(string Id, string DisplayName, int Number);
record ScheduleMatchRequest(DateTime ScheduledStart);
record RecordMatchResultRequest(int ScoreA, int ScoreB);

record MatchReadResponse(string Id,
    string? TeamAId,
    string? TeamAName,
    string? TeamBId,
    string? TeamBName,
    DateTime? ScheduledStart,
    string Status,
    int? ScoreA,
    int? ScoreB,
    int RoundNumber,
    int MatchNumber);