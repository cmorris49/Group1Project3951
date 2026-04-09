using MySqlConnector;
using TournamentManagerAPI.Contracts;
using TournamentManagerAPI.Infrastructure;

namespace TournamentManagerAPI.Endpoints
{
    /// <summary>
    /// Registers authentication-related API endpoints.
    /// </summary>
    public static class AuthEndpoints
    {
        /// <summary>
        /// Maps authentication endpoints.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The same endpoint route builder for chaining.</returns>
        public static IEndpointRouteBuilder MapAuthEndpoints(
            this IEndpointRouteBuilder app,
            string? connectionString)
        {
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
                    DbReaderHelpers.ReadId(reader, "Id"),
                    reader.GetString("Name")));
            });

            /// <summary>
            /// Retrieves the current authenticated user using the user identifier supplied in request headers.
            /// </summary>
            /// <param name="httpContext">The current HTTP context containing request headers.</param>
            /// <returns>An OK response with the current user identity when valid; otherwise unauthorized.</returns>
            app.MapGet("/auth/me", async (HttpContext httpContext) =>
            {
                if (!AuthHeaderHelpers.TryReadUserId(httpContext, out var userId))
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
                    DbReaderHelpers.ReadId(reader, "Id"),
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

            return app;
        }
    }
}