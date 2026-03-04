using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Group 1 Project - TeamsPlayersPage UserControl
/// Author: Cameron
///         Jun
///         Jonathan 
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
        private Tournament? currentTournament;

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
            RefreshTeams();
        }

        /// <summary>
        /// Refreshes the teams grid with current tournament data
        /// </summary>
        private void RefreshTeams()
        {
            dataViewTeams.Rows.Clear();

            if (currentTournament == null)
                return;

            foreach (var division in currentTournament.Divisions)
            {
                foreach (var team in division.Teams)
                {
                    dataViewTeams.Rows.Add(
                        team.Name,
                        team.seed,
                        team.Players.Count
                    );
                }
            }
        }

        /// <summary>
        /// Handles team selection and displays players for the selected team
        /// </summary>
        private void DataViewTeams_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewPlayers.Rows.Clear();

            if (dataViewTeams.SelectedRows.Count == 0 || currentTournament == null)
                return;

            int selectedIndex = dataViewTeams.SelectedRows[0].Index;

            Team? selectedTeam = null;
            int currentIndex = 0;

            foreach (var division in currentTournament.Divisions)
            {
                foreach (var team in division.Teams)
                {
                    if (currentIndex == selectedIndex)
                    {
                        selectedTeam = team;
                        break;
                    }
                    currentIndex++;
                }
                if (selectedTeam != null)
                    break;
            }

            if (selectedTeam != null)
            {
                foreach (var player in selectedTeam.Players)
                {
                    dataGridViewPlayers.Rows.Add(
                        player.displayName,
                        player.number
                    );
                }
            }
        }

        private void buttonAddTeam_Click(object sender, EventArgs e)
        { 
            MessageBox.Show("Please use the Add Team button in the main toolbar.",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
