using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

        public sealed record DashboardStatsDto(int Teams, int Players, int Matches);
        public sealed record TeamReadDto(string Id, string Name, int Seed, string DivisionId, string DivisionName, List<PlayerReadDto> Players);
        public sealed record PlayerReadDto(string Id, string DisplayName, int Number);
        public sealed record MatchReadDto(string Id, string? TeamAId, 
            string? TeamAName, string? TeamBId,
            string? TeamBName, DateTime? ScheduledStart,
            string Status, int? ScoreA, int? ScoreB);

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
        /// Fetches all tournaments from the database.
        /// </summary>
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
        /// Saves a new tournament to the database.
        /// </summary>
        internal async Task<bool> CreateTournamentAsync(Tournament tournament)
        {
            try
            {
                // Send payload shape expected by API
                var request = new TournamentCreateRequest(
                    tournament.Name,
                    string.IsNullOrWhiteSpace(tournament.Location) ? null : tournament.Location,
                    tournament.StartDate,
                    tournament.BracketType.ToString());

                // API route is /tournaments
                var response = await _httpClient.PostAsJsonAsync("tournaments", request);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"CreateTournament failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating tournament: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Creates a team for the specified tournament by calling the tournament-scoped API endpoint.
        /// </summary>
        /// <param name="tournamentId">The target tournament identifier.</param>
        /// <param name="team">The team data to persist, including players.</param>
        /// <returns>True when the API accepts and saves the team; otherwise false.</returns>
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
        /// Retrieves dashboard aggregate statistics (teams, players, matches) for a tournament.
        /// </summary>
        /// <param name="tournamentId">The target tournament identifier.</param>
        /// <returns>A dashboard DTO, or null if the request fails.</returns>
        internal async Task<DashboardStatsDto?> GetDashboardStatsAsync(Guid tournamentId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<DashboardStatsDto>($"tournaments/{tournamentId}/dashboard", _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching dashboard stats: {ex.Message}");
                return null;
            }

        }

        /// <summary>
        /// Retrieves all teams (and nested players) for a tournament.
        /// </summary>
        /// <param name="tournamentId">The target tournament identifier.</param>
        /// <returns>A list of team DTOs, or null if the request fails.</returns>
        internal async Task<List<TeamReadDto>?> GetTeamsForTournamentAsync(Guid tournamentId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<TeamReadDto>>($"tournaments/{tournamentId}/teams", _jsonOptions);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching teams: {ex.Message}");
                return new List<TeamReadDto>();
            }
        }

        /// <summary>
        /// Retrieves persisted bracket matches for a tournament.
        /// </summary>
        /// <param name="tournamentId">The target tournament identifier.</param>
        /// <returns>A list of match DTOs, or null if the request fails.</returns>
        internal async Task<List<MatchReadDto>?> GetMatchesForTournamentAsync(Guid tournamentId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<MatchReadDto>>($"tournaments/{tournamentId}/matches", _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching matches: {ex.Message}");
                return new List<MatchReadDto>();
            }
        }

        /// <summary>
        /// Requests bracket generation for a tournament in the API/database layer.
        /// </summary>
        /// <param name="tournamentId">The target tournament identifier.</param>
        /// <returns>True when bracket generation succeeds; otherwise false.</returns>
        internal async Task<bool> GenerateBracketAsync(Guid tournamentId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"tournaments/{tournamentId}/generate-bracket", null);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"GenerateBracket failed: {(int)response.StatusCode} {response.StatusCode}. Body: {body}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating bracket: {ex}");
                return false;
            }
        }
    }
}




