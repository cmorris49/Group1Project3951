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
/// Group 1 Project - DashBroadPage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    public partial class DashboardPage : UserControl
    {
        private Tournament? currentTournament;

        /// <summary>
        /// The DashboardPage class represents a user control that serves as the main dashboard for the tournament management application.
        /// </summary>
        public DashboardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tournament"></param>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            RefreshDashboard();
        }

        /// <summary>
        /// Refreshes the dashboard by updating the displayed statistics for total teams, total players, and total matches based on the current tournament data.
        /// </summary>
        private void RefreshDashboard()
        {
            if (currentTournament == null)
            {
                SetStatistic(lblTeamsCount, "0");
                SetStatistic(lblPlayersCount, "0");
                SetStatistic(lblMatchesCount, "0");
                return;
            }

            int totalTeams = GetTotalTeams();
            int totalPlayers = GetTotalPlayers();
            int totalMatches = GetTotalMatches();

            SetStatistic(lblTeamsCount, totalTeams.ToString());
            SetStatistic(lblPlayersCount, totalPlayers.ToString());
            SetStatistic(lblMatchesCount, totalMatches.ToString());
        }

        /// <summary>
        /// Gets the total number of teams in the current tournament by checking if the tournament is not null and counting all teams in each division of the tournament.
        /// </summary>
        /// <returns></returns>
        private int GetTotalTeams()
        {
            if (currentTournament == null)
            {
                return 0;
            }

            int count = 0;
            
            foreach (var division in currentTournament.Divisions)
            {
                count += division.Teams.Count;
            }
            return count;
        }

        /// <summary>
        /// Gets the total number of players in the current tournament by checking if the tournament are not null and count all players in each division in the tournament.
        /// </summary>
        /// <returns>Count of players in the tournament. If the tournament is null, it returns 0 to indicate that there are no players to count.</returns>
        private int GetTotalPlayers()
        {
            if (currentTournament == null)
            {
                return 0; 
            }

            int count = 0;
            foreach (var division in currentTournament.Divisions)
            {
                foreach (var team in division.Teams)
                {
                    count += team.Players.Count;
                }
            }
            return count;
        }

        /// <summary>
        /// Gets the total number of matches in the current tournament by checking if the tournament and its bracket are not null, and then returning the count of matches in the bracket. 
        /// If either the tournament or the bracket is null, it returns 0 to indicate that there are no matches to count.
        /// </summary>
        /// <returns></returns>
        private int GetTotalMatches()
        {
            if (currentTournament?.Bracket == null)
            {   
                return 0; 
            }
            return currentTournament.Bracket.Matches.Count;
        }

        /// <summary>
        /// Sets the text of a given label to the specified value, ensuring that the label is not null before attempting to update its text property.
        /// </summary>
        /// <param name="label">Control label whose text property is to be updated. This parameter must not be null, as the method checks for null before attempting to set the text.</param>
        /// <param name="value">Data value to be displayed on the label. This parameter is expected to be a string representation of the statistic being displayed (e.g., total teams, total players, total matches).</param>
        private void SetStatistic(Label label, string value)
        {
            if (label != null)
            {
                label.Text = value;
            }
        }
    }
}
