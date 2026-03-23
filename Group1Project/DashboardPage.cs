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

        private readonly ApiClient _apiClient = new ApiClient();

        /// <summary>
        /// The DashboardPage class represents a user control that serves as the main dashboard for the tournament management application.
        /// </summary>
        public DashboardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the selected tournament context into the dashboard and refreshes database-backed metrics.
        /// </summary>
        /// <param name="tournament">The tournament to display.</param>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshDashboardAsync();
        }

        /// <summary>
        /// Fetches and displays dashboard statistics from the API for the currently loaded tournament.
        /// </summary>
        /// <returns>A task representing the asynchronous refresh operation.</returns>
        private async Task RefreshDashboardAsync()
        {
            if (currentTournament == null)
            {
                SetStatistic(lblTeamsCount, "0");
                SetStatistic(lblPlayersCount, "0");
                SetStatistic(lblMatchesCount, "0");
                return;
            }

            var stats = await _apiClient.GetDashboardStatsAsync(currentTournament.Id);
            if (stats == null)
            {
                SetStatistic(lblTeamsCount, "0");
                SetStatistic(lblPlayersCount, "0");
                SetStatistic(lblMatchesCount, "0");
                return;
            }

            SetStatistic(lblTeamsCount, stats.Teams.ToString());
            SetStatistic(lblPlayersCount, stats.Players.ToString());
            SetStatistic(lblMatchesCount, stats.Matches.ToString());
        }

        //private int GetTotalTeams()
        //{
        //    if (currentTournament == null) return 0;

        //    int count = 0;
        //    foreach (var division in currentTournament.Divisions)
        //    {
        //        count += division.Teams.Count;
        //    }
        //    return count;
        //}

        //private int GetTotalPlayers()
        //{
        //    if (currentTournament == null) return 0;

        //    int count = 0;
        //    foreach (var division in currentTournament.Divisions)
        //    {
        //        foreach (var team in division.Teams)
        //        {
        //            count += team.Players.Count;
        //        }
        //    }
        //    return count;
        //}

        //private int GetTotalMatches()
        //{
        //    if (currentTournament?.Bracket == null) return 0;
        //    return currentTournament.Bracket.Matches.Count;
        //}

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
