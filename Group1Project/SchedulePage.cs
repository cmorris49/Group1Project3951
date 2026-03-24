using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Group 1 Project - SchedulePage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 24, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=vs-2022
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents the scheduling user interface for viewing and updating match start times.
    /// </summary>
    public partial class SchedulePage : UserControl
    {
        private Tournament? currentTournament;
        private readonly ApiClient _apiClient = new ApiClient();
        private List<ApiClient.MatchReadDto> loadedMatches = new List<ApiClient.MatchReadDto>();

        /// <summary>
        /// Initializes a new instance of the schedule page and configures the schedule grid.
        /// </summary>
        public SchedulePage()
        {
            InitializeComponent();
            InitializeScheduleGrid();
        }

        /// <summary>
        /// Loads the selected tournament context into the schedule page and refreshes schedule data.
        /// </summary>
        /// <param name="tournament">The tournament to display.</param>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshScheduleAsync();
        }

        /// <summary>
        /// Configures the schedule grid columns and behavior.
        /// </summary>
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

        /// <summary>
        /// Retrieves scheduled match data for the current tournament and populates the schedule grid.
        /// </summary>
        /// <returns>A task representing the asynchronous refresh operation.</returns>
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

        /// <summary>
        /// Applies the selected schedule date and time to the currently selected match.
        /// </summary>
        /// <returns>A task representing the asynchronous update operation.</returns>
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

        /// <summary>
        /// Handles the click event for setting a selected match schedule.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonSetSchedule_Click(object sender, EventArgs e)
        {
            await SetSelectedScheduleAsync();
        }

        /// <summary>
        /// Handles the click event for refreshing schedule data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            await RefreshScheduleAsync();
        }
    }
}