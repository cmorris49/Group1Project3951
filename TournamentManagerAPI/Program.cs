using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var app = builder.Build();

app.MapGet("/test-db", async () =>
{
    await using MySqlConnection connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    await using MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM Tournament", connection);
    object? result = await command.ExecuteScalarAsync();

    return Results.Ok($"Connected. Tournament count = {result}");
});

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
               m.ScoreB
        FROM Matches m
        INNER JOIN Bracket b ON b.Id = m.BracketId
        LEFT JOIN Team ta ON ta.Id = m.TeamAId
        LEFT JOIN Team tb ON tb.Id = m.TeamBId
        WHERE b.TournamentId = @TournamentId
        ORDER BY m.ScheduledStart, m.Id
        """;

    var matches = new List<MatchReadResponse>();

    await using var cmd = new MySqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());

    await using var reader = await cmd.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        var idOrdinal = reader.GetOrdinal("Id");
        var teamAIdOrdinal = reader.GetOrdinal("TeamAId");
        var teamBIdOrdinal = reader.GetOrdinal("TeamBId");
        var teamANameOrdinal = reader.GetOrdinal("TeamAName");
        var teamBNameOrdinal = reader.GetOrdinal("TeamBName");
        var scheduledStartOrdinal = reader.GetOrdinal("ScheduledStart");
        var scoreAOrdinal = reader.GetOrdinal("ScoreA");
        var scoreBOrdinal = reader.GetOrdinal("ScoreB");

        matches.Add(new MatchReadResponse(
            reader.GetValue(idOrdinal).ToString()!,
            reader.IsDBNull(teamAIdOrdinal) ? null : reader.GetValue(teamAIdOrdinal).ToString(),
            reader.IsDBNull(teamANameOrdinal) ? null : reader.GetString("TeamAName"),
            reader.IsDBNull(teamBIdOrdinal) ? null : reader.GetValue(teamBIdOrdinal).ToString(),
            reader.IsDBNull(teamBNameOrdinal) ? null : reader.GetString("TeamBName"),
            reader.IsDBNull(scheduledStartOrdinal) ? null : reader.GetDateTime("ScheduledStart"),
            reader.GetString("Status"),
            reader.IsDBNull(scoreAOrdinal) ? null : reader.GetInt32("ScoreA"),
            reader.IsDBNull(scoreBOrdinal) ? null : reader.GetInt32("ScoreB")));
    }

    return Results.Ok(matches);
});

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
                teamIds.Add(reader.GetValue(reader.GetOrdinal("Id")).ToString()!);
            }
        }

        if (teamIds.Count < 2)
        {
            await tx.RollbackAsync();
            return Results.BadRequest("Need at least 2 teams to generate a bracket.");
        }

        const string upsertBracketSql = """
            INSERT INTO Bracket (Id, TournamentId, BracketType, TotalRounds)
            VALUES (UUID(), @TournamentId, 'SingleElimination', 1)
            ON DUPLICATE KEY UPDATE BracketType = VALUES(BracketType)
            """;

        await using (var cmd = new MySqlCommand(upsertBracketSql, connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
            await cmd.ExecuteNonQueryAsync();
        }

        const string getBracketSql = "SELECT Id FROM Bracket WHERE TournamentId = @TournamentId LIMIT 1";
        string bracketId;
        await using (var cmd = new MySqlCommand(getBracketSql, connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
            bracketId = (await cmd.ExecuteScalarAsync())?.ToString()!;
        }

        const string clearMatchesSql = "DELETE FROM Matches WHERE BracketId = @BracketId";
        await using (var cmd = new MySqlCommand(clearMatchesSql, connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@BracketId", bracketId);
            await cmd.ExecuteNonQueryAsync();
        }

        const string insertMatchSql = """
            INSERT INTO Matches (Id, BracketId, TeamAId, TeamBId, Status)
            VALUES (UUID(), @BracketId, @TeamAId, @TeamBId, 'Unscheduled')
            """;

        for (int i = 0; i < teamIds.Count; i += 2)
        {
            var a = teamIds[i];
            var b = (i + 1 < teamIds.Count) ? teamIds[i + 1] : teamIds[i];

            await using var cmd = new MySqlCommand(insertMatchSql, connection, (MySqlTransaction)tx);
            cmd.Parameters.AddWithValue("@BracketId", bracketId);
            cmd.Parameters.AddWithValue("@TeamAId", a);
            cmd.Parameters.AddWithValue("@TeamBId", b);
            await cmd.ExecuteNonQueryAsync();
        }

        await tx.CommitAsync();
        return Results.Ok("Bracket generated.");
    }
    catch (Exception ex)
    {
        await tx.RollbackAsync();
        return Results.Problem($"Failed to generate bracket: {ex.Message}");
    }
});

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

app.MapPut("/matches/{matchId:guid}/result", async (Guid matchId, RecordMatchResultRequest request) =>
{
    if (request.ScoreA < 0 || request.ScoreB < 0)
    {
        return Results.BadRequest("Scores must be non-negative.");
    }

    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string sql = """
        UPDATE Matches
        SET ScoreA = @ScoreA,
            ScoreB = @ScoreB,
            ScoreRecordedAt = UTC_TIMESTAMP(),
            ScheduledStart = COALESCE(ScheduledStart, UTC_TIMESTAMP()),
            Status = 'Complete'
        WHERE Id = @MatchId
        """;

    await using var cmd = new MySqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@ScoreA", request.ScoreA);
    cmd.Parameters.AddWithValue("@ScoreB", request.ScoreB);
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
record MatchReadResponse(string Id, string? TeamAId, string? TeamAName, string? TeamBId, string? TeamBName, DateTime? ScheduledStart, string Status, int? ScoreA, int? ScoreB);
record ScheduleMatchRequest(DateTime ScheduledStart);
record RecordMatchResultRequest(int ScoreA, int ScoreB);