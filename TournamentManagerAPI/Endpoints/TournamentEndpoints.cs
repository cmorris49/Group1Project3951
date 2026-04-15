using MySqlConnector;
using TournamentManagerAPI.Contracts;
using TournamentManagerAPI.Infrastructure;

namespace TournamentManagerAPI.Endpoints
{
    /// <summary>
    /// Registers tournament-related API endpoints.
    /// </summary>
    public static class TournamentEndpoints
    {
        /// <summary>
        /// Maps tournament endpoints.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The same endpoint route builder for chaining.</returns>
        public static IEndpointRouteBuilder MapTournamentEndpoints(
            this IEndpointRouteBuilder app,
            string? connectionString)
        {
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
            app.MapGet("/tournaments", async (HttpContext httpContext) =>
            {
                if (!AuthHeaderHelpers.TryReadUserId(httpContext, out var userId))
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
            app.MapPost("/tournaments", async (HttpContext httpContext, TournamentCreateRequest request) =>
            {
                if (!AuthHeaderHelpers.TryReadUserId(httpContext, out var userId))
                {
                    return Results.Unauthorized();
                }

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

                    const string insertAccessSql = """
                        INSERT INTO UserTournamentAccess (UserId, TournamentId)
                        VALUES (@UserId, @TournamentId)
                        """;

                    await using (var cmd = new MySqlCommand(insertAccessSql, connection, (MySqlTransaction)tx))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@TournamentId", tournamentId);
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

            return app;
        }
    }
}