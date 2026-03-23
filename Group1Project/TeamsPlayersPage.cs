using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Group1Project.ApiClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

/// <summary>
/// Group 1 Project - TeamsPlayersPage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 2, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents a user control for managing teams and their players within the application.
    /// </summary>
    public partial class TeamsPlayersPage : UserControl
    {
        /// <summary>
        /// Property to hold the current tournament data being displayed and managed by this page. It may be null if no tournament is currently loaded.
        /// </summary>
        private Tournament? currentTournament;
        private readonly ApiClient _apiClient = new ApiClient();
        private List<ApiClient.TeamReadDto> loadedTeams = new List<ApiClient.TeamReadDto>();

        /// <summary>
        /// the initializes a new instance of the TeamsPlayersPage class and sets up the user interface components,
        /// including event handlers for team selection in the teams data grid view.
        /// </summary>
        public TeamsPlayersPage()
        {
            InitializeComponent();

            dataViewTeams.SelectionChanged += DataViewTeams_SelectionChanged;
        }

        /// <summary>
        /// Loads tournament data into the page
        /// </summary>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshTeamsAsync();
        }

        /// <summary>
        /// Fetches teams and players for the current tournament from the API and binds them to the grids.
        /// </summary>
        /// <returns>A task representing the asynchronous refresh operation.</returns>
        private async Task RefreshTeamsAsync()
        {
            dataViewTeams.Rows.Clear();
            dataGridViewPlayers.Rows.Clear();

            if (currentTournament == null)
                return;

            loadedTeams = await _apiClient.GetTeamsForTournamentAsync(currentTournament.Id) ?? new List<ApiClient.TeamReadDto>();

            foreach (var team in loadedTeams)
            {
                dataViewTeams.Rows.Add(team.Name, team.Seed, team.Players.Count);
            }
        }

        /// <summary>
        /// Handles team selection and displays players for the selected team
        /// </summary>
        private void DataViewTeams_SelectionChanged(object? sender, EventArgs e)
        {
            dataGridViewPlayers.Rows.Clear();

            if (dataViewTeams.SelectedRows.Count == 0)
                return;

            int selectedIndex = dataViewTeams.SelectedRows[0].Index;
            if (selectedIndex < 0 || selectedIndex >= loadedTeams.Count)
                return;

            foreach (var player in loadedTeams[selectedIndex].Players)
            {
                dataGridViewPlayers.Rows.Add(player.DisplayName, player.Number);
            }
        }

        /// <summary>
        /// The event handler for the "Add Team" button click event. It displays an informational message prompting the user to use the main toolbar for adding teams,
        /// rather than allowing team addition directly from this page.
        /// </summary>
        /// <param name="sender">The source of event, typically the addteam button</param>
        /// <param name="e"> An EventArgs instance containing the event data.</param>
        private void buttonAddTeam_Click(object sender, EventArgs e)
        { 
            MessageBox.Show("Please use the Add Team button in the main toolbar.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
