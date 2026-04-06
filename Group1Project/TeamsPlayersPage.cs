using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
        private readonly ApiClient _apiClient;
        private List<ApiClient.TeamReadDto> loadedTeams = new List<ApiClient.TeamReadDto>();

        /// <summary>
        /// the initializes a new instance of the TeamsPlayersPage class and sets up the user interface components,
        /// including event handlers for team selection in the teams data grid view.
        /// </summary>
        public TeamsPlayersPage() : this(new ApiClient())
        {
        }

        public TeamsPlayersPage(ApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            InitializeComponent();
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
        /// Opens the Add Team dialog for the current tournament and refreshes team/player grids after a successful add.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonAddTeam_Click(object sender, EventArgs e)
        {
            if (currentTournament == null)
            {
                MessageBox.Show("Please create/select a tournament first.",
                    "No Tournament",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (currentTournament.Divisions.Count == 0)
            {
                MessageBox.Show("No division found for this tournament.",
                    "No Division",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Division selectedDivision = currentTournament.Divisions[0];

            using var addTeamForm = new addTeam(selectedDivision, currentTournament.Id, _apiClient);

            if (addTeamForm.ShowDialog(this) == DialogResult.OK)
            {
                await RefreshTeamsAsync();
            }
        }

        /// <summary>
        /// Opens an edit dialog for the selected team name, saves the update through the API, and refreshes selection state.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonEditTeam_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedTeam(out var selectedTeam, out int teamIndex))
            {
                MessageBox.Show("Please select a team to edit.",
                    "No Team Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using var dialog = new EntityInputDialog(
                "Edit Team",
                "Team name:",
                selectedTeam!.Name,
                false);

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            bool ok = await _apiClient.UpdateTeamNameAsync(selectedTeam.Id, dialog.EntityName);
            if (!ok)
            {
                MessageBox.Show("Failed to update team name.",
                    "Update Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            await RefreshTeamsAndReselectAsync(teamIndex, null);
        }

        /// <summary>
        /// Removes the selected team after user confirmation and refreshes displayed data.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonRemoveTeam_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedTeam(out var selectedTeam, out _))
            {
                MessageBox.Show("Please select a team to remove.",
                    "No Team Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Remove team '{selectedTeam!.Name}' and all of its players?",
                "Confirm Remove Team",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            bool ok = await _apiClient.DeleteTeamAsync(selectedTeam.Id);
            if (!ok)
            {
                MessageBox.Show("Failed to remove team.",
                    "Remove Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            await RefreshTeamsAsync();
        }

        /// <summary>
        /// Opens a player input dialog for the selected team, creates the player through the API, and refreshes selection state.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonAddPlayer_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedTeam(out var selectedTeam, out int teamIndex))
            {
                MessageBox.Show("Please select a team first.",
                    "No Team Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using var dialog = new EntityInputDialog(
                "Add Player",
                "Player name:",
                string.Empty,
                true,
                0,
                "Player number:");

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            bool ok = await _apiClient.AddPlayerAsync(selectedTeam!.Id, dialog.EntityName, dialog.EntityNumber);
            if (!ok)
            {
                MessageBox.Show("Failed to add player.",
                    "Add Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            await RefreshTeamsAndReselectAsync(teamIndex, null);
        }

        /// <summary>
        /// Opens a player edit dialog for the selected player, saves changes through the API, and refreshes selection state.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonEditPlayer_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedPlayer(out var selectedTeam, out var selectedPlayer, out int teamIndex, out int playerIndex))
            {
                MessageBox.Show("Please select a player to edit.",
                    "No Player Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using var dialog = new EntityInputDialog(
                "Edit Player",
                "Player name:",
                selectedPlayer!.DisplayName,
                true,
                selectedPlayer.Number,
                "Player number:");

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            bool ok = await _apiClient.UpdatePlayerAsync(selectedPlayer.Id, dialog.EntityName, dialog.EntityNumber);
            if (!ok)
            {
                MessageBox.Show("Failed to update player.",
                    "Update Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            await RefreshTeamsAndReselectAsync(teamIndex, playerIndex);
        }

        /// <summary>
        /// Removes the selected player after user confirmation and refreshes selection state.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void buttonRemovePlayer_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedPlayer(out var selectedTeam, out var selectedPlayer, out int teamIndex, out _))
            {
                MessageBox.Show("Please select a player to remove.",
                    "No Player Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Remove player '{selectedPlayer!.DisplayName}' from team '{selectedTeam!.Name}'?",
                "Confirm Remove Player",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            bool ok = await _apiClient.DeletePlayerAsync(selectedPlayer.Id);
            if (!ok)
            {
                MessageBox.Show("Failed to remove player.",
                    "Remove Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            await RefreshTeamsAndReselectAsync(teamIndex, null);
        }

        /// <summary>
        /// Attempts to resolve the currently selected team row into a loaded team DTO.
        /// </summary>
        /// <param name="team">When this method returns, contains the selected team when found; otherwise null.</param>
        /// <param name="teamIndex">When this method returns, contains the selected team row index when valid; otherwise -1.</param>
        /// <returns>True when a valid team selection exists; otherwise false.</returns>
        private bool TryGetSelectedTeam(out ApiClient.TeamReadDto? team, out int teamIndex)
        {
            team = null;
            teamIndex = -1;

            if (dataViewTeams.SelectedRows.Count == 0)
            {
                return false;
            }

            teamIndex = dataViewTeams.SelectedRows[0].Index;
            if (teamIndex < 0 || teamIndex >= loadedTeams.Count)
            {
                return false;
            }

            team = loadedTeams[teamIndex];
            return true;
        }

        /// <summary>
        /// Attempts to resolve the currently selected team/player row into loaded DTO references and indexes.
        /// </summary>
        /// <param name="team">When this method returns, contains the selected team when found; otherwise null.</param>
        /// <param name="player">When this method returns, contains the selected player when found; otherwise null.</param>
        /// <param name="teamIndex">When this method returns, contains the selected team row index when valid; otherwise -1.</param>
        /// <param name="playerIndex">When this method returns, contains the selected player row index when valid; otherwise -1.</param>
        /// <returns>True when both selected team and selected player are valid; otherwise false.</returns>
        private bool TryGetSelectedPlayer(
            out ApiClient.TeamReadDto? team,
            out ApiClient.PlayerReadDto? player,
            out int teamIndex,
            out int playerIndex)
        {
            team = null;
            player = null;
            teamIndex = -1;
            playerIndex = -1;

            if (!TryGetSelectedTeam(out team, out teamIndex))
            {
                return false;
            }

            if (dataGridViewPlayers.SelectedRows.Count == 0)
            {
                return false;
            }

            playerIndex = dataGridViewPlayers.SelectedRows[0].Index;
            if (playerIndex < 0 || playerIndex >= team!.Players.Count)
            {
                return false;
            }

            player = team.Players[playerIndex];
            return true;
        }

        /// <summary>
        /// Refreshes teams/players data from the API and restores previously selected row indexes when available.
        /// </summary>
        /// <param name="teamIndex">The team row index to reselect after refresh.</param>
        /// <param name="playerIndex">The player row index to reselect after refresh, when provided.</param>
        /// <returns>A task representing the asynchronous refresh and reselection operation.</returns>
        private async Task RefreshTeamsAndReselectAsync(int teamIndex, int? playerIndex)
        {
            await RefreshTeamsAsync();

            if (teamIndex >= 0 && teamIndex < dataViewTeams.Rows.Count)
            {
                dataViewTeams.ClearSelection();
                dataViewTeams.Rows[teamIndex].Selected = true;
            }

            if (playerIndex.HasValue && playerIndex.Value >= 0 && playerIndex.Value < dataGridViewPlayers.Rows.Count)
            {
                dataGridViewPlayers.ClearSelection();
                dataGridViewPlayers.Rows[playerIndex.Value].Selected = true;
            }
        }
    }
}
