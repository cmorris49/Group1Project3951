using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Group 1 Project - ResultsPage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 24, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=vs-2022
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents the results user interface for entering and reviewing completed match scores.
    /// </summary>
    public partial class ResultsPage : UserControl
    {
        private Tournament? currentTournament;
        private readonly ApiClient _apiClient = new ApiClient();
        private List<ApiClient.MatchReadDto> loadedMatches = new List<ApiClient.MatchReadDto>();
        private ApiClient.MatchReadDto? selectedMatch;

        /// <summary>
        /// Initializes a new instance of the results page and configures result controls.
        /// </summary>
        public ResultsPage()
        {
            InitializeComponent();
            InitializeResultsGrid();
            InitializeScoreInputs();
        }

        /// <summary>
        /// Loads the selected tournament context into the results page and refreshes result data.
        /// </summary>
        /// <param name="tournament">The tournament to display.</param>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshResultsAsync();
        }

        /// <summary>
        /// Configures the results grid columns and behavior.
        /// </summary>
        private void InitializeResultsGrid()
        {
            dataGridViewResults.AllowUserToAddRows = false;
            dataGridViewResults.ReadOnly = true;
            dataGridViewResults.RowHeadersVisible = false;
            dataGridViewResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewResults.MultiSelect = false;
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridViewResults.Columns.Count == 0)
            {
                dataGridViewResults.Columns.Add("columnTeamA", "Team A");
                dataGridViewResults.Columns.Add("columnTeamB", "Team B");
                dataGridViewResults.Columns.Add("columnScheduled", "Scheduled");
                dataGridViewResults.Columns.Add("columnScore", "Score");
                dataGridViewResults.Columns.Add("columnWinner", "Winner");
                dataGridViewResults.Columns.Add("columnStatus", "Status");
            }
        }

        /// <summary>
        /// Configures score input ranges for result entry.
        /// </summary>
        private void InitializeScoreInputs()
        {
            numericScoreA.Minimum = 0;
            numericScoreA.Maximum = 999;

            numericScoreB.Minimum = 0;
            numericScoreB.Maximum = 999;
        }

        /// <summary>
        /// Retrieves match result data for the current tournament and populates the results grid.
        /// </summary>
        /// <returns>A task representing the asynchronous refresh operation.</returns>
        private async Task RefreshResultsAsync()
        {
            dataGridViewResults.Rows.Clear();
            selectedMatch = null;

            if (labelSelectedMatch != null)
            {
                labelSelectedMatch.Text = "Selected: (none)";
            }

            if (currentTournament == null)
            {
                return;
            }

            loadedMatches = await _apiClient.GetMatchesForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.MatchReadDto>();

            foreach (var match in loadedMatches)
            {
                var when = match.ScheduledStart.HasValue
                    ? match.ScheduledStart.Value.ToString("g")
                    : "(unscheduled)";

                var score = (match.ScoreA.HasValue && match.ScoreB.HasValue)
                    ? $"{match.ScoreA.Value} - {match.ScoreB.Value}"
                    : "-";

                var winner = GetWinnerText(match);

                var rowIndex = dataGridViewResults.Rows.Add(
                    match.TeamAName ?? "TBD",
                    match.TeamBName ?? "TBD",
                    when,
                    score,
                    winner,
                    match.Status);

                dataGridViewResults.Rows[rowIndex].Tag = match;
            }
        }

        /// <summary>
        /// Computes the winner display text for a match based on recorded scores.
        /// </summary>
        /// <param name="match">The match used to determine winner text.</param>
        /// <returns>The winner name, draw marker, or placeholder text.</returns>
        private static string GetWinnerText(ApiClient.MatchReadDto match)
        {
            if (!match.ScoreA.HasValue || !match.ScoreB.HasValue)
            {
                return "-";
            }

            if (match.ScoreA.Value > match.ScoreB.Value)
            {
                return match.TeamAName ?? "Team A";
            }

            if (match.ScoreB.Value > match.ScoreA.Value)
            {
                return match.TeamBName ?? "Team B";
            }

            return "Draw";
        }

        /// <summary>
        /// Records a result for the currently selected match.
        /// </summary>
        /// <returns>A task representing the asynchronous update operation.</returns>
        private async Task SetSelectedResultAsync()
        {
            if (selectedMatch == null)
            {
                MessageBox.Show("Select a match first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedMatch.TeamAId) || string.IsNullOrWhiteSpace(selectedMatch.TeamBId))
            {
                MessageBox.Show("Cannot set result for an incomplete matchup (TBD team).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ok = await _apiClient.RecordMatchResultAsync(
                selectedMatch.Id,
                (int)numericScoreA.Value,
                (int)numericScoreB.Value);

            if (!ok)
            {
                MessageBox.Show("Could not save result.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await RefreshResultsAsync();
        }

        /// <summary>
        /// Handles result grid selection changes and updates selected match context.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments for the selection change.</param>
        private void dataGridViewResults_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count == 0)
            {
                selectedMatch = null;
                if (labelSelectedMatch != null)
                {
                    labelSelectedMatch.Text = "Selected: (none)";
                }

                return;
            }

            selectedMatch = dataGridViewResults.SelectedRows[0].Tag as ApiClient.MatchReadDto;
            if (selectedMatch == null)
            {
                return;
            }

            numericScoreA.Value = selectedMatch.ScoreA ?? 0;
            numericScoreB.Value = selectedMatch.ScoreB ?? 0;

            if (labelSelectedMatch != null)
            {
                labelSelectedMatch.Text = $"Selected: {selectedMatch.TeamAName ?? "TBD"} vs {selectedMatch.TeamBName ?? "TBD"}";
            }
        }

        /// <summary>
        /// Handles the click event for setting a selected match result.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonSetResult_Click(object sender, EventArgs e)
        {
            await SetSelectedResultAsync();
        }

        /// <summary>
        /// Handles the click event for refreshing result data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            await RefreshResultsAsync();
        }
    }
}