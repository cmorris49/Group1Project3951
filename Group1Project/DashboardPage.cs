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
    /// <summary>
    /// Represents the dashboard user interface for displaying tournament summary metrics and match activity.
    /// </summary>
    public partial class DashboardPage : UserControl
    {
        private Tournament? currentTournament;

        private readonly ApiClient _apiClient = new ApiClient();

        private readonly Label _labelNextMatchValue = new Label();

        /// <summary>
        /// The DashboardPage class represents a user control that serves as the main dashboard for the tournament management application.
        /// </summary>
        public DashboardPage()
        {
            InitializeComponent();
            InitializeNextMatchCard();
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
        /// Fetches and displays dashboard statistics and activity feeds from the API for the currently loaded tournament.
        /// </summary>
        /// <returns>A task representing the asynchronous refresh operation.</returns>
        private async Task RefreshDashboardAsync()
        {
            if (currentTournament == null)
            {
                SetStatistic(lblTeamsCount, "0");
                SetStatistic(lblPlayersCount, "0");
                SetStatistic(lblMatchesCount, "0");
                labelDashTournament.Text = "Tournament: (none)";
                ResetMatchSections();
                return;
            }

            labelDashTournament.Text = $"Tournament: {currentTournament.Name}";

            var stats = await _apiClient.GetDashboardStatsAsync(currentTournament.Id);
            if (stats == null)
            {
                SetStatistic(lblTeamsCount, "0");
                SetStatistic(lblPlayersCount, "0");
                SetStatistic(lblMatchesCount, "0");
            }
            else
            {
                SetStatistic(lblTeamsCount, stats.Teams.ToString());
                SetStatistic(lblPlayersCount, stats.Players.ToString());
                SetStatistic(lblMatchesCount, stats.Matches.ToString());
            }

            var matches = await _apiClient.GetMatchesForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.MatchReadDto>();

            PopulateDashboardMatchSections(matches);
        }

        /// <summary>
        /// Configures a value label inside the Next Match card at runtime.
        /// </summary>
        private void InitializeNextMatchCard()
        {
            _labelNextMatchValue.AutoSize = false;
            _labelNextMatchValue.TextAlign = ContentAlignment.TopCenter;
            _labelNextMatchValue.Font = new Font("Segoe UI", 9F);

            _labelNextMatchValue.Location = new Point(6, 24);
            _labelNextMatchValue.Size = new Size(cardNextMatch.Width - 12, cardNextMatch.Height - 28);
            _labelNextMatchValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _labelNextMatchValue.Text = "No upcoming match";

            cardNextMatch.Controls.Add(_labelNextMatchValue);
            labelCardNextMatchTitle.BringToFront();
        }

        /// <summary>
        /// Clears dashboard match sections to an empty state.
        /// </summary>
        private void ResetMatchSections()
        {
            _labelNextMatchValue.Text = "No upcoming match";
            listBox1.Items.Clear();
            flowLayoutPanel1.Controls.Clear();
        }

        /// <summary>
        /// Fills Next Match, Recent Activity, and Upcoming Events sections.
        /// </summary>
        /// <param name="matches">The tournament matches loaded from API.</param>
        private void PopulateDashboardMatchSections(List<ApiClient.MatchReadDto> matches)
        {
            listBox1.Items.Clear();
            flowLayoutPanel1.Controls.Clear();

            var recentCompleted = matches
                .Where(IsCompletedWithResult)
                .OrderByDescending(m => m.ScheduledStart ?? DateTime.MinValue)
                .ThenByDescending(m => m.RoundNumber)
                .ThenByDescending(m => m.MatchNumber)
                .ToList();

            if (recentCompleted.Count == 0)
            {
                listBox1.Items.Add("No completed matches yet.");
            }
            else
            {
                foreach (var match in recentCompleted.Take(20))
                {
                    var winner = GetWinnerName(match);
                    var score = $"{match.ScoreA!.Value}-{match.ScoreB!.Value}";
                    var row = $"R{match.RoundNumber} M{match.MatchNumber}: {winner} won {score}";
                    listBox1.Items.Add(row);
                }
            }

            var upcomingScheduled = matches
                .Where(m => m.ScheduledStart.HasValue && !IsCompletedWithResult(m))
                .OrderBy(m => m.ScheduledStart!.Value)
                .ThenBy(m => m.RoundNumber)
                .ThenBy(m => m.MatchNumber)
                .ToList();

            if (upcomingScheduled.Count == 0)
            {
                AddUpcomingEventRow("No upcoming scheduled matches.");
            }
            else
            {
                foreach (var match in upcomingScheduled.Take(20))
                {
                    var text = $"{match.ScheduledStart!.Value:g}  |  R{match.RoundNumber} M{match.MatchNumber}  |  {FormatTeams(match)}";
                    AddUpcomingEventRow(text);
                }
            }

            var nextMatch = upcomingScheduled.FirstOrDefault();
            if (nextMatch == null)
            {
                _labelNextMatchValue.Text = "No upcoming match";
            }
            else
            {
                _labelNextMatchValue.Text =
                    $"{nextMatch.ScheduledStart!.Value:g}\nR{nextMatch.RoundNumber} M{nextMatch.MatchNumber}\n{FormatTeams(nextMatch)}";
            }
        }

        /// <summary>
        /// Adds a single text row to the Upcoming Events panel.
        /// </summary>
        /// <param name="text">Display text for an event row.</param>
        private void AddUpcomingEventRow(string text)
        {
            var label = new Label
            {
                AutoSize = true,
                Text = text,
                Margin = new Padding(3, 2, 3, 6)
            };

            flowLayoutPanel1.Controls.Add(label);
        }

        /// <summary>
        /// Determines whether a match has a final result.
        /// </summary>
        /// <param name="match">The match to test.</param>
        /// <returns>True if match status is Complete and scores are present.</returns>
        private static bool IsCompletedWithResult(ApiClient.MatchReadDto match)
        {
            return string.Equals(match.Status, "Complete", StringComparison.OrdinalIgnoreCase)
                && match.ScoreA.HasValue
                && match.ScoreB.HasValue;
        }

        /// <summary>
        /// Formats team names for display.
        /// </summary>
        /// <param name="match">The match to format.</param>
        /// <returns>Formatted "Team A vs Team B" text.</returns>
        private static string FormatTeams(ApiClient.MatchReadDto match)
        {
            return $"{match.TeamAName ?? "TBD"} vs {match.TeamBName ?? "TBD"}";
        }

        /// <summary>
        /// Gets winner display name from a completed match.
        /// </summary>
        /// <param name="match">The completed match.</param>
        /// <returns>Winner team name, or "Draw" if tied.</returns>
        private static string GetWinnerName(ApiClient.MatchReadDto match)
        {
            if (!match.ScoreA.HasValue || !match.ScoreB.HasValue)
            {
                return "Unknown";
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