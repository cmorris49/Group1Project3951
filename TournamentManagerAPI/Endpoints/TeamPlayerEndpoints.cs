using MySqlConnector;
using TournamentManagerAPI.Contracts;

namespace TournamentManagerAPI.Endpoints
{
    /// <summary>
    /// Registers team and player management API endpoints.
    /// </summary>
    public static class TeamPlayerEndpoints
    {
        /// <summary>
        /// Maps team and player endpoints.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The same endpoint route builder for chaining.</returns>
        public static IEndpointRouteBuilder MapTeamPlayerEndpoints(
            this IEndpointRouteBuilder app,
            string? connectionString)
        {
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

            app.MapPut("/teams/{teamId:guid}", async (Guid teamId, TeamNameUpdateRequest request) =>
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Results.BadRequest("Team name is required.");
                }

                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                const string sql = "UPDATE Team SET Name = @Name WHERE Id = @TeamId";
                await using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@Name", request.Name.Trim());
                cmd.Parameters.AddWithValue("@TeamId", teamId.ToString());

                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 0 ? Results.NotFound("Team not found.") : Results.Ok();
            });

            app.MapDelete("/teams/{teamId:guid}", async (Guid teamId) =>
            {
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                const string sql = "DELETE FROM Team WHERE Id = @TeamId";
                await using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@TeamId", teamId.ToString());

                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 0 ? Results.NotFound("Team not found.") : Results.Ok();
            });

            app.MapPost("/teams/{teamId:guid}/players", async (Guid teamId, PlayerUpsertRequest request) =>
            {
                if (string.IsNullOrWhiteSpace(request.DisplayName))
                {
                    return Results.BadRequest("Player name is required.");
                }

                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                const string existsSql = "SELECT COUNT(*) FROM Team WHERE Id = @TeamId";
                await using (var existsCmd = new MySqlCommand(existsSql, connection))
                {
                    existsCmd.Parameters.AddWithValue("@TeamId", teamId.ToString());
                    var exists = Convert.ToInt32(await existsCmd.ExecuteScalarAsync());
                    if (exists == 0)
                    {
                        return Results.NotFound("Team not found.");
                    }
                }

                const string insertSql = """
                    INSERT INTO Player (Id, TeamId, DisplayName, Number)
                    VALUES (UUID(), @TeamId, @DisplayName, @Number)
                    """;

                await using var cmd = new MySqlCommand(insertSql, connection);
                cmd.Parameters.AddWithValue("@TeamId", teamId.ToString());
                cmd.Parameters.AddWithValue("@DisplayName", request.DisplayName.Trim());
                cmd.Parameters.AddWithValue("@Number", request.Number);
                await cmd.ExecuteNonQueryAsync();

                return Results.Ok();
            });

            app.MapPut("/players/{playerId:guid}", async (Guid playerId, PlayerUpsertRequest request) =>
            {
                if (string.IsNullOrWhiteSpace(request.DisplayName))
                {
                    return Results.BadRequest("Player name is required.");
                }

                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                const string sql = """
                    UPDATE Player
                    SET DisplayName = @DisplayName, Number = @Number
                    WHERE Id = @PlayerId
                    """;

                await using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@DisplayName", request.DisplayName.Trim());
                cmd.Parameters.AddWithValue("@Number", request.Number);
                cmd.Parameters.AddWithValue("@PlayerId", playerId.ToString());

                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 0 ? Results.NotFound("Player not found.") : Results.Ok();
            });

            app.MapDelete("/players/{playerId:guid}", async (Guid playerId) =>
            {
                await using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                const string sql = "DELETE FROM Player WHERE Id = @PlayerId";
                await using var cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@PlayerId", playerId.ToString());

                var affected = await cmd.ExecuteNonQueryAsync();
                return affected == 0 ? Results.NotFound("Player not found.") : Results.Ok();
            });

            return app;
        }
    }
}