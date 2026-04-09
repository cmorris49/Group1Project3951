namespace TournamentManagerAPI.Contracts
{
    public record LoginRequest(string Name, string Password);

    public record LoginResponse(string Id, string Name);

    public record RegisterRequest(string Name, string Password);
}