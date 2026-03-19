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

    const string sql = """
        INSERT INTO Tournament (Id, Name, Location, StartDate, BracketType)
        VALUES (UUID(), @Name, @Location, @StartDate, @BracketType)
        """;

    await using MySqlCommand command = new MySqlCommand(sql, connection);
    command.Parameters.AddWithValue("@Name", request.Name);
    command.Parameters.AddWithValue("@Location",
        string.IsNullOrWhiteSpace(request.Location) ? DBNull.Value : request.Location);
    command.Parameters.AddWithValue("@StartDate", request.StartDate);
    command.Parameters.AddWithValue("@BracketType", request.BracketType);

    await command.ExecuteNonQueryAsync();

    return Results.Ok("Tournament added successfully.");
});

app.Run();

record TournamentCreateRequest(string Name, string? Location, DateTime StartDate, string BracketType);