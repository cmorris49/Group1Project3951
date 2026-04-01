using Microsoft.AspNetCore.Identity.Data;
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
/// Attempts to read the user identifier from the request header used for API authorization.
/// </summary>
/// <param name="httpContext">The current HTTP context.</param>
/// <param name="userId">When this method returns, contains the user identifier if present and valid.</param>
/// <returns>True when a non-empty user identifier header is present; otherwise, false.</returns>
static bool TryReadUserId(HttpContext httpContext, out string userId)
{
    userId = string.Empty;

    if (!httpContext.Request.Headers.TryGetValue("X-User-Id", out var values))
    {
        return false;
    }

    var headerValue = values.ToString().Trim();
    if (string.IsNullOrWhiteSpace(headerValue))
    {
        return false;
    }

    userId = headerValue;
    return true;
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
/// Authenticates a user with a name and password and returns basic identity data when successful.
/// </summary>
/// <param name="request">The login payload containing user name and password.</param>
/// <returns>An OK response with user identity when authentication succeeds; otherwise unauthorized or bad request.</returns>
app.MapPost("/auth/login", async (LoginRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest("Name and password are required.");
    }

    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string sql = """
        SELECT Id, Name
        FROM AppUser
        WHERE Name = @Name
          AND PasswordHash = SHA2(@Password, 256)
        LIMIT 1
        """;

    await using var cmd = new MySqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@Name", request.Name.Trim());
    cmd.Parameters.AddWithValue("@Password", request.Password);

    await using var reader = await cmd.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new LoginResponse(
        ReadId(reader, "Id"),
        reader.GetString("Name")));
});

/// <summary>
/// Retrieves the current authenticated user using the user identifier supplied in request headers.
/// </summary>
/// <param name="httpContext">The current HTTP context containing request headers.</param>
/// <returns>An OK response with the current user identity when valid; otherwise unauthorized.</returns>
app.MapGet("/auth/me", async (HttpContext httpContext) =>
{
    if (!TryReadUserId(httpContext, out var userId))
    {
        return Results.Unauthorized();
    }

    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string sql = """
        SELECT Id, Name
        FROM AppUser
        WHERE Id = @UserId
        LIMIT 1
        """;

    await using var cmd = new MySqlCommand(sql, connection);
    cmd.Parameters.AddWithValue("@UserId", userId);

    await using var reader = await cmd.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new LoginResponse(
        ReadId(reader, "Id"),
        reader.GetString("Name")));
});

/// <summary>
/// Registers a new user account when the provided user name is not already in use.
/// </summary>
/// <param name="request">The registration payload containing user name and password.</param>
/// <returns>An OK response with the created user identity, or a conflict/bad request response on failure.</returns>
app.MapPost("/auth/register", async (RegisterRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest("Name and password are required.");
    }

    var trimmedName = request.Name.Trim();

    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string existsSql = """
        SELECT COUNT(*)
        FROM AppUser
        WHERE Name = @Name
        """;

    await using (var existsCmd = new MySqlCommand(existsSql, connection))
    {
        existsCmd.Parameters.AddWithValue("@Name", trimmedName);
        var exists = Convert.ToInt32(await existsCmd.ExecuteScalarAsync());
        if (exists > 0)
        {
            return Results.Conflict("User name already exists.");
        }
    }

    var userId = Guid.NewGuid().ToString();

    const string insertSql = """
        INSERT INTO AppUser (Id, Name, PasswordHash)
        VALUES (@Id, @Name, SHA2(@Password, 256))
        """;

    await using (var insertCmd = new MySqlCommand(insertSql, connection))
    {
        insertCmd.Parameters.AddWithValue("@Id", userId);
        insertCmd.Parameters.AddWithValue("@Name", trimmedName);
        insertCmd.Parameters.AddWithValue("@Password", request.Password);
        await insertCmd.ExecuteNonQueryAsync();
    }

    return Results.Ok(new LoginResponse(userId, trimmedName));
});

/// <summary>
/// Retrieves all tournaments ordered by start date.
/// </summary>
/// <returns>A list of tournament records including identifier, name, location, start date, and bracket type.</returns>
app.MapGet("/tournaments", async (HttpContext httpContext) =>
{
    if (!TryReadUserId(httpContext, out var userId))
    {
        return Results.Unauthorized();
    }

    List<object> tournaments = new List<object>();

    await using MySqlConnection connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();

    const string sql = """
        SELECT t.Id, t.Name, t.Location, t.StartDate, t.BracketType
        FROM Tournament t
        INNER JOIN UserTournamentAccess uta ON uta.TournamentId = t.Id
        WHERE uta.UserId = @UserId
        ORDER BY t.StartDate
        """;

    await using MySqlCommand command = new MySqlCommand(sql, connection);
    command.Parameters.AddWithValue("@UserId", userId);

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
        // ── 1. Fetch teams ordered by seed ─────────────────────────────────────
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

        // ── 2. Read the tournament's BracketType ────────────────────────────────
        string bracketTypeStr;
        await using (var cmd = new MySqlCommand(
            "SELECT BracketType FROM Tournament WHERE Id = @TournamentId LIMIT 1",
            connection, (MySqlTransaction)tx))
        {
            cmd.Parameters.AddWithValue("@TournamentId", tournamentId.ToString());
            bracketTypeStr = (await cmd.ExecuteScalarAsync())?.ToString() ?? "SingleElimination";
        }

        // ── 3. Upsert the Bracket row (placeholder TotalRounds; updated later) ─
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

        // ── 4. Clear existing matches ───────────────────────────────────────────
        await using (var clearCmd = new MySqlCommand(
            "DELETE FROM Matches WHERE BracketId = @BracketId",
            connection, (MySqlTransaction)tx))
        {
            clearCmd.Parameters.AddWithValue("@BracketId", bracketId);
            await clearCmd.ExecuteNonQueryAsync();
        }

        // ── 5. Dispatch to the correct generation strategy ──────────────────────
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
/// Generates Round Robin matches using the circle/rotation algorithm.
/// Every team plays every other team exactly once.
/// If the team count is odd a null bye-slot is added; matches against the bye are skipped.
/// </summary>
static async Task GenerateRoundRobinMatchesAsync(
    MySqlConnection connection,
    MySqlTransaction tx,
    string bracketId,
    List<string> teamIds)
{
    // Add a bye slot when the count is odd so the rotation stays balanced
    bool hasBye = teamIds.Count % 2 != 0;
    var slots = new List<string?>(teamIds.Select(id => (string?)id));
    if (hasBye) slots.Add(null);

    int n = slots.Count;          // always even
    int totalRounds = n - 1;
    int matchesPerRound = n / 2;

    // Write the final TotalRounds into the Bracket row
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

            // Skip matches where one side is the bye slot
            if (teamA == null || teamB == null)
                continue;

            matchNum++;
            await using var cmd = new MySqlCommand(insertMatchSql, connection, tx);
            cmd.Parameters.AddWithValue("@BracketId", bracketId);
            cmd.Parameters.AddWithValue("@TeamAId", teamA);
            cmd.Parameters.AddWithValue("@TeamBId", teamB);
            cmd.Parameters.AddWithValue("@RoundNumber", round);
            cmd.Parameters.AddWithValue("@MatchNumber", matchNum);
            await cmd.ExecuteNonQueryAsync();
        }

        // Circle rotation: keep slot 0 fixed, shift positions 1..n-1 right by one
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
static async Task GenerateSingleEliminationMatchesAsync(
    MySqlConnection connection,
    MySqlTransaction tx,
    string bracketId,
    List<string> teamIds)
{
    int bracketSize = 1;
    while (bracketSize < teamIds.Count) bracketSize *= 2;

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

    // Fill power-of-two slots with null byes at the end
    var slots = new List<string?>(new string?[bracketSize]);
    for (int i = 0; i < teamIds.Count; i++) slots[i] = teamIds[i];

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

                if (string.Equals(status, "Complete", StringComparison.OrdinalIgnoreCase)) continue;

                string matchId = reader.GetValue(reader.GetOrdinal("Id")).ToString()!;

                if (!string.IsNullOrWhiteSpace(teamA) && string.IsNullOrWhiteSpace(teamB))
                    byeWinners.Add((reader.GetInt32("MatchNumber"), teamA!, true, matchId));
                else if (string.IsNullOrWhiteSpace(teamA) && !string.IsNullOrWhiteSpace(teamB))
                    byeWinners.Add((reader.GetInt32("MatchNumber"), teamB!, false, matchId));
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
/// Total rounds = ceil(log2(n)). Odd team gets a bye.
/// </summary>
static async Task GenerateSwissFirstRoundAsync(
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
    // Odd team: no match inserted — they have a bye for round 1
}

/// <summary>
/// Generates the next Swiss round by pairing teams with equal win counts,
/// avoiding rematches. Teams with a bye in a previous round are treated as having won.
/// </summary>
app.MapPost("/tournaments/{tournamentId:guid}/generate-next-round", async (Guid tournamentId) =>
{
    await using var connection = new MySqlConnection(connectionString);
    await connection.OpenAsync();
    await using var tx = await connection.BeginTransactionAsync();

    try
    {
        // ── Load bracket ────────────────────────────────────────────────────
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

        // ── Find current highest round ──────────────────────────────────────
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

        // ── Check all matches in current round are complete ─────────────────
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

        // ── Load all teams in this tournament ───────────────────────────────
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
                allTeamIds.Add(reader.GetValue(reader.GetOrdinal("Id")).ToString()!);
        }

        // ── Build win counts from all completed matches ─────────────────────
        const string resultsSql = """
            SELECT TeamAId, TeamBId, ScoreA, ScoreB FROM Matches
            WHERE BracketId = @BracketId AND Status = 'Complete'
            """;

        var wins = allTeamIds.ToDictionary(id => id, _ => 0, StringComparer.OrdinalIgnoreCase);

        // Store played pairs as sorted strings "A|B" (smaller ID first) for easy lookup
        var played = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        string PlayedKey(string a, string b) =>
            string.Compare(a, b, StringComparison.OrdinalIgnoreCase) <= 0
                ? $"{a}|{b}" : $"{b}|{a}";

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
                if (tA == null || tB == null) continue;

                int sA = reader.GetInt32("ScoreA");
                int sB = reader.GetInt32("ScoreB");

                played.Add(PlayedKey(tA, tB));

                if (wins.ContainsKey(tA) && wins.ContainsKey(tB))
                {
                    if (sA > sB) wins[tA]++;
                    else if (sB > sA) wins[tB]++;
                }
            }
        }

        // ── Sort teams by wins desc, then by original seed ──────────────────
        var sortedTeams = allTeamIds
            .OrderByDescending(id => wins.TryGetValue(id, out var w) ? w : 0)
            .ToList();

        // ── Greedy pairing: pair first available team with next unpaired ─────
        var paired = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var newMatches = new List<(string A, string B)>();

        for (int i = 0; i < sortedTeams.Count; i++)
        {
            string teamA = sortedTeams[i];
            if (paired.Contains(teamA)) continue;

            bool found = false;
            for (int j = i + 1; j < sortedTeams.Count; j++)
            {
                string teamB = sortedTeams[j];
                if (paired.Contains(teamB)) continue;

                // Avoid rematch
                bool alreadyPlayed = played.Contains(PlayedKey(teamA, teamB));
                if (alreadyPlayed) continue;

                newMatches.Add((teamA, teamB));
                paired.Add(teamA);
                paired.Add(teamB);
                found = true;
                break;
            }

            // If no opponent found (everyone already played), allow a rematch as last resort
            if (!found)
            {
                for (int j = i + 1; j < sortedTeams.Count; j++)
                {
                    string teamB = sortedTeams[j];
                    if (paired.Contains(teamB)) continue;
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

        // ── Insert new round matches ─────────────────────────────────────────
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

            bracketId = ReadId(reader, "BracketId");
            roundNumber = reader.GetInt32("RoundNumber");
            matchNumber = reader.GetInt32("MatchNumber");
            totalRounds = reader.GetInt32("TotalRounds");
            bracketType = reader.GetString("BracketType");
            teamAId = ReadNullableId(reader, "TeamAId");
            teamBId = ReadNullableId(reader, "TeamBId");
        }

        if (string.IsNullOrWhiteSpace(teamAId) || string.IsNullOrWhiteSpace(teamBId))
        {
            await tx.RollbackAsync();
            return Results.BadRequest("Cannot record result until both teams are assigned.");
        }

        bool isRoundRobin = string.Equals(bracketType, "RoundRobin", StringComparison.OrdinalIgnoreCase);

        // Ties are only disallowed in single-elimination (a winner must advance)
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

        // Only advance winner for elimination-style brackets
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

app.Run();

record TournamentCreateRequest(string Name, string? Location, DateTime StartDate, string BracketType);
record TeamCreateRequest(Guid DivisionId, string Name, int Seed, List<PlayerCreateRequest>? Players);
record PlayerCreateRequest(string DisplayName, int Number);
record TeamCreateForTournamentRequest(string Name, int Seed, List<PlayerCreateRequest>? Players);
record TeamReadResponse(string Id, string Name, int Seed, string DivisionId, string DivisionName, List<PlayerReadResponse> Players);
record PlayerReadResponse(string Id, string DisplayName, int Number);
record ScheduleMatchRequest(DateTime ScheduledStart);
record RecordMatchResultRequest(int ScoreA, int ScoreB);
record LoginRequest(string Name, string Password);
record LoginResponse(string Id, string Name);
record RegisterRequest(string Name, string Password);

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