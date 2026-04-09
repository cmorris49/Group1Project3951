using TournamentManagerAPI.Endpoints;

/// <summary>
/// Group 1 Project - TournamentManagerAPI Program
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     ASP.NET Core minimal API info https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis
/// </summary>

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing connection string: DefaultConnection.");

var app = builder.Build();

app.MapAuthEndpoints(connectionString);
app.MapTournamentEndpoints(connectionString);
app.MapBracketEndpoints(connectionString);
app.MapMatchEndpoints(connectionString);
app.MapTeamPlayerEndpoints(connectionString);

app.Run();