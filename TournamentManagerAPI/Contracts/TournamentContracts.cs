namespace TournamentManagerAPI.Contracts
{
    public record TournamentCreateRequest(string Name, string? Location, DateTime StartDate, string BracketType);
}