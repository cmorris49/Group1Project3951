using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Group 1 Project - ApiClient Class
/// Author: Cameron, Jun, Jonathan 
/// Date: March 24, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     HttpClient info https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Handles all HTTP communication with the TournamentManagerAPI.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        private sealed record TournamentCreateRequest(
            string Name,
            string? Location,
            DateTime StartDate,
            string BracketType);

        private sealed record TeamCreateForTournamentRequest(
            string Name,
            int Seed,
            List<Player> Players);

        private sealed record ScheduleMatchRequest(DateTime ScheduledStart);
        private sealed record RecordMatchResultRequest(int ScoreA, int ScoreB);

        public sealed record DashboardStatsDto(int Teams, int Players, int Matches);
        public sealed record TeamReadDto(string Id, string Name, int Seed, string DivisionId, string DivisionName, List<PlayerReadDto> Players);
        public sealed record PlayerReadDto(string Id, string DisplayName, int Number);
        private sealed record TournamentCreateResponse(string TournamentId, string DivisionId);
        public sealed record MatchReadDto(
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

        /// <summary>
        /// Initializes a new API client instance with the base address for TournamentManagerAPI.
        /// </summary>
        public ApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7077/");
        }

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// Retrieves all tournaments from the API.
        /// </summary>
        /// <returns>A list of tournaments, or an empty list if retrieval fails.</returns>
        internal async Task<List<Tournament>?> GetTournamentsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Tournament>>("tournaments", _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching tournaments: {ex.Message}");
                return new List<Tournament>();
            }
        }

        /// <summary>
        /// Creates a tournament in the API and updates the local tournament identifier with the persisted database identifier.
        /// </summary>
        /// <param name="tournament">The tournament to create.</param>
        /// <returns>True if creation succeeds; otherwise, false.</returns>
        internal async Task<bool> CreateTournamentAsync(Tournament tournament)
        {
            try
            {
                var request = new TournamentCreateRequest(
                    tournament.Name,
                    string.IsNullOrWhiteSpace(tournament.Location) ? null : tournament.Location,
                    tournament.StartDate,
                    tournament.BracketType.ToString());

                var response = await _httpClient.PostAsJsonAsync("tournaments", request);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"CreateTournament failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
                    return false;
                }

                var created = await response.Content.ReadFromJsonAsync<TournamentCreateResponse>(_jsonOptions);
                if (created == null || !Guid.TryParse(created.TournamentId, out var dbTournamentId))
                {
                    Console.WriteLine("CreateTournament failed: response did not include a valid TournamentId.");
                    return false;
                }

                tournament.Id = dbTournamentId;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating tournament: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Creates a team in the specified tournament.
        /// </summary>
        /// <param name="tournamentId">The identifier of the tournament.</param>
        /// <param name="team">The team to create.</param>
        /// <returns>True if creation succeeds; otherwise, false.</returns>
        public async Task<bool> CreateTeamForTournamentAsync(Guid tournamentId, Team team)
        {
            try
            {
                var request = new TeamCreateForTournamentRequest(team.Name, team.Seed, team.Players);
                var response = await _httpClient.PostAsJsonAsync($"tournaments/{tournamentId}/teams", request);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"CreateTeam failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating team: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves dashboard statistics for the specified tournament.
        /// </summary>
        /// <param name="tournamentId">The identifier of the tournament.</param>
        /// <returns>Dashboard statistics, or null if unavailable.</returns>
        internal async Task<DashboardStatsDto?> GetDashboardStatsAsync(Guid tournamentId)
        {
            return await _httpClient.GetFromJsonAsync<DashboardStatsDto>($"tournaments/{tournamentId}/dashboard", _jsonOptions);
        }

        /// <summary>
        /// Retrieves all teams for the specified tournament.
        /// </summary>
        /// <param name="tournamentId">The identifier of the tournament.</param>
        /// <returns>A list of teams, or null if unavailable.</returns>
        internal async Task<List<TeamReadDto>?> GetTeamsForTournamentAsync(Guid tournamentId)
        {
            return await _httpClient.GetFromJsonAsync<List<TeamReadDto>>($"tournaments/{tournamentId}/teams", _jsonOptions);
        }

        /// <summary>
        /// Retrieves all matches for the specified tournament.
        /// </summary>
        /// <param name="tournamentId">The identifier of the tournament.</param>
        /// <returns>A list of matches, or null if unavailable.</returns>
        internal async Task<List<MatchReadDto>?> GetMatchesForTournamentAsync(Guid tournamentId)
        {
            return await _httpClient.GetFromJsonAsync<List<MatchReadDto>>($"tournaments/{tournamentId}/matches", _jsonOptions);
        }

        /// <summary>
        /// Requests bracket generation for the specified tournament.
        /// </summary>
        /// <param name="tournamentId">The identifier of the tournament.</param>
        /// <returns>True if generation succeeds; otherwise, false.</returns>
        internal async Task<bool> GenerateBracketAsync(Guid tournamentId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"tournaments/{tournamentId}/generate-bracket", null);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(
                        $"GenerateBracket failed: {(int)response.StatusCode} {response.StatusCode}\n\n{body}",
                        "Generate Bracket Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error generating bracket:\n{ex}",
                    "Generate Bracket Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Schedules a match at the specified start date and time.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="scheduledStart">The scheduled start date and time.</param>
        /// <returns>True if scheduling succeeds; otherwise, false.</returns>
        internal async Task<bool> ScheduleMatchAsync(string matchId, DateTime scheduledStart)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"matches/{matchId}/schedule", new ScheduleMatchRequest(scheduledStart));

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"ScheduleMatch failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scheduling match: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Records the final score for a match.
        /// </summary>
        /// <param name="matchId">The identifier of the match.</param>
        /// <param name="scoreA">Score for Team A.</param>
        /// <param name="scoreB">Score for Team B.</param>
        /// <returns>True if recording succeeds; otherwise, false.</returns>
        internal async Task<bool> RecordMatchResultAsync(string matchId, int scoreA, int scoreB)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"matches/{matchId}/result", new RecordMatchResultRequest(scoreA, scoreB));

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"RecordMatchResult failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording match result: {ex}");
                return false;
            }
        }
    }
}