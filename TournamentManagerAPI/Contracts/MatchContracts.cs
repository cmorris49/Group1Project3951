namespace TournamentManagerAPI.Contracts
{
    public record ScheduleMatchRequest(DateTime ScheduledStart);

    public record RecordMatchResultRequest(int ScoreA, int ScoreB);

    public record MatchReadResponse(
        string Id,
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
}