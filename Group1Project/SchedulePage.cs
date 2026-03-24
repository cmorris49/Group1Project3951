using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Group1Project
{
    public partial class SchedulePage : UserControl
    {
        private Tournament? currentTournament;
        private readonly ApiClient _apiClient = new ApiClient();
        private List<ApiClient.MatchReadDto> loadedMatches = new List<ApiClient.MatchReadDto>();

        public SchedulePage()
        {
            InitializeComponent();
            InitializeScheduleGrid();
        }

        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshScheduleAsync();
        }

        private void InitializeScheduleGrid()
        {
            dataGridViewSchedule.AllowUserToAddRows = false;
            dataGridViewSchedule.ReadOnly = true;
            dataGridViewSchedule.RowHeadersVisible = false;
            dataGridViewSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSchedule.MultiSelect = false;
            dataGridViewSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridViewSchedule.Columns.Count == 0)
            {
                dataGridViewSchedule.Columns.Add("columnTeamA", "Team A");
                dataGridViewSchedule.Columns.Add("columnTeamB", "Team B");
                dataGridViewSchedule.Columns.Add("columnScheduled", "Scheduled");
                dataGridViewSchedule.Columns.Add("columnStatus", "Status");
                dataGridViewSchedule.Columns.Add("columnScore", "Score");
            }
        }

        private async Task RefreshScheduleAsync()
        {
            dataGridViewSchedule.Rows.Clear();

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

                var rowIndex = dataGridViewSchedule.Rows.Add(
                    match.TeamAName ?? "TBD",
                    match.TeamBName ?? "TBD",
                    when,
                    match.Status,
                    score);

                dataGridViewSchedule.Rows[rowIndex].Tag = match;
            }
        }

        private async Task SetSelectedScheduleAsync()
        {
            if (dataGridViewSchedule.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a match first.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = dataGridViewSchedule.SelectedRows[0].Tag as ApiClient.MatchReadDto;
            if (selected == null)
            {
                return;
            }

            if (string.Equals(selected.Status, "Complete", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Completed matches cannot be rescheduled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ok = await _apiClient.ScheduleMatchAsync(selected.Id, dateTimePickerStart.Value);
            if (!ok)
            {
                MessageBox.Show("Could not save schedule.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await RefreshScheduleAsync();
        }

        private async void buttonSetSchedule_Click(object sender, EventArgs e)
        {
            await SetSelectedScheduleAsync();
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            await RefreshScheduleAsync();
        }
    }
}