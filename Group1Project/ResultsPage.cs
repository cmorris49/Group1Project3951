using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Group1Project
{
    public partial class ResultsPage : UserControl
    {
        private Tournament? currentTournament;
        private readonly ApiClient _apiClient = new ApiClient();
        private List<ApiClient.MatchReadDto> loadedMatches = new List<ApiClient.MatchReadDto>();
        private ApiClient.MatchReadDto? selectedMatch;

        public ResultsPage()
        {
            InitializeComponent();
            InitializeResultsGrid();
            InitializeScoreInputs();
        }

        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshResultsAsync();
        }

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

        private void InitializeScoreInputs()
        {
            numericScoreA.Minimum = 0;
            numericScoreA.Maximum = 999;

            numericScoreB.Minimum = 0;
            numericScoreB.Maximum = 999;
        }

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

        private async void buttonSetResult_Click(object sender, EventArgs e)
        {
            await SetSelectedResultAsync();
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            await RefreshResultsAsync();
        }
    }
}