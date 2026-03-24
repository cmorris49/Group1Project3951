using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Group 1 Project - TeamStatsPage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 24, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=vs-2022
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents the team statistics user interface for calculating and displaying team performance metrics.
    /// </summary>
    public partial class TeamStatsPage : UserControl
    {
        private Tournament? currentTournament;
        private readonly ApiClient _apiClient = new ApiClient();

        /// <summary>
        /// Initializes a new instance of the team stats page and configures the stats grid.
        /// </summary>
        public TeamStatsPage()
        {
            InitializeComponent();
            InitializeTeamStatsGrid();
        }

        /// <summary>
        /// Loads the selected tournament context into the team stats page and refreshes team statistics.
        /// </summary>
        /// <param name="tournament">The tournament to display.</param>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshTeamStatsAsync();
        }

        /// <summary>
        /// Configures the team statistics grid columns and behavior.
        /// </summary>
        private void InitializeTeamStatsGrid()
        {
            dataGridViewTeamStats.AllowUserToAddRows = false;
            dataGridViewTeamStats.ReadOnly = true;
            dataGridViewTeamStats.RowHeadersVisible = false;
            dataGridViewTeamStats.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTeamStats.MultiSelect = false;
            dataGridViewTeamStats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridViewTeamStats.Columns.Count == 0)
            {
                dataGridViewTeamStats.Columns.Add("columnTeam", "Team");
                dataGridViewTeamStats.Columns.Add("columnGP", "GP");
                dataGridViewTeamStats.Columns.Add("columnW", "W");
                dataGridViewTeamStats.Columns.Add("columnL", "L");
                dataGridViewTeamStats.Columns.Add("columnD", "D");
                dataGridViewTeamStats.Columns.Add("columnPF", "PF");
                dataGridViewTeamStats.Columns.Add("columnPA", "PA");
                dataGridViewTeamStats.Columns.Add("columnPD", "PD");
                dataGridViewTeamStats.Columns.Add("columnWinPct", "Win %");
            }
        }

        /// <summary>
        /// Recalculates and displays team statistics using completed match data from the API.
        /// </summary>
        /// <returns>A task representing the asynchronous refresh operation.</returns>
        private async Task RefreshTeamStatsAsync()
        {
            dataGridViewTeamStats.Rows.Clear();

            if (currentTournament == null)
            {
                return;
            }

            var teams = await _apiClient.GetTeamsForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.TeamReadDto>();

            var matches = await _apiClient.GetMatchesForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.MatchReadDto>();

            var statsByTeamId = teams.ToDictionary(
                t => t.Id,
                t => new TeamStatRow { Team = t.Name },
                StringComparer.OrdinalIgnoreCase);

            foreach (var match in matches)
            {
                if (!string.Equals(match.Status, "Complete", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!match.ScoreA.HasValue || !match.ScoreB.HasValue)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(match.TeamAId) || string.IsNullOrWhiteSpace(match.TeamBId))
                {
                    continue;
                }

                if (!statsByTeamId.TryGetValue(match.TeamAId, out var teamAStats) ||
                    !statsByTeamId.TryGetValue(match.TeamBId, out var teamBStats))
                {
                    continue;
                }

                var scoreA = match.ScoreA.Value;
                var scoreB = match.ScoreB.Value;

                teamAStats.GP++;
                teamBStats.GP++;

                teamAStats.PF += scoreA;
                teamAStats.PA += scoreB;

                teamBStats.PF += scoreB;
                teamBStats.PA += scoreA;

                if (scoreA > scoreB)
                {
                    teamAStats.W++;
                    teamBStats.L++;
                }
                else if (scoreB > scoreA)
                {
                    teamBStats.W++;
                    teamAStats.L++;
                }
                else
                {
                    teamAStats.D++;
                    teamBStats.D++;
                }
            }

            var ordered = statsByTeamId.Values
                .OrderByDescending(x => x.W)
                .ThenByDescending(x => x.PD)
                .ThenByDescending(x => x.PF)
                .ThenBy(x => x.Team, StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var row in ordered)
            {
                var winPct = row.GP == 0
                    ? "0.0%"
                    : $"{(100.0 * row.W / row.GP):0.0}%";

                dataGridViewTeamStats.Rows.Add(
                    row.Team,
                    row.GP,
                    row.W,
                    row.L,
                    row.D,
                    row.PF,
                    row.PA,
                    row.PD,
                    winPct);
            }
        }

        /// <summary>
        /// Handles the click event for refreshing team statistics.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonRefreshStats_Click(object sender, EventArgs e)
        {
            await RefreshTeamStatsAsync();
        }

        private sealed class TeamStatRow
        {
            public string Team { get; set; } = string.Empty;
            public int GP { get; set; }
            public int W { get; set; }
            public int L { get; set; }
            public int D { get; set; }
            public int PF { get; set; }
            public int PA { get; set; }
            public int PD => PF - PA;
        }
    }
}