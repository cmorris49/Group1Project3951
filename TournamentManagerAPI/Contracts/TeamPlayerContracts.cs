namespace TournamentManagerAPI.Contracts
{
    public record TeamCreateRequest(Guid DivisionId, string Name, int Seed, List<PlayerCreateRequest>? Players);

    public record PlayerCreateRequest(string DisplayName, int Number);

    public record TeamCreateForTournamentRequest(string Name, int Seed, List<PlayerCreateRequest>? Players);

    public record TeamReadResponse(
        string Id,
        string Name,
        int Seed,
        string DivisionId,
        string DivisionName,
        List<PlayerReadResponse> Players);

    public record PlayerReadResponse(string Id, string DisplayName, int Number);

    public record TeamNameUpdateRequest(string Name);

    public record PlayerUpsertRequest(string DisplayName, int Number);
}