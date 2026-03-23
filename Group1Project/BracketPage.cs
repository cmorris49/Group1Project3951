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
/// Group 1 Project - BracketPage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    
    public partial class BracketPage : UserControl
    {
        private const int MATCH_BOX_WIDTH = 150;
        private const int MATCH_BOX_HEIGHT = 150;
        private const int ROUND_SPACING = 200;
        private const int MATCH_VERTICAL_SPACING = 20;

        private Tournament? currentTournament;

        private readonly ApiClient _apiClient = new ApiClient();

        private List<ApiClient.MatchReadDto> _apiMatches = new List<ApiClient.MatchReadDto>();

        private readonly List<GroupBox> _renderedMatchBoxes = new List<GroupBox>();

        /// <summary>
        /// Initializes a new instance of the BracketPage user control, configures UI defaults,
        /// and wires event handlers for view switching, panel painting, and resize redraw behavior.
        /// </summary>
        public BracketPage()
        {
            InitializeComponent();

            InitializeMatchesGrid();

            comboBoxView.SelectedIndexChanged += (s, e) =>
            {
                var view = comboBoxView.SelectedItem?.ToString() ?? "Matches";
                ShowView(view);
            };

            panelBracketContainer.Paint += PanelBracketContainer_Paint;

            panelBracketContainer.Resize += (s, e) =>
            {
                if (_apiMatches.Count > 0 && panelBracketContainer.Visible)
                {
                    DrawBracketTreeFromApiMatches();
                }
            };

            if (comboBoxView.Items.Count > 0)
                comboBoxView.SelectedIndex = 0;

            ShowView("Bracket");
        }

        /// <summary>
        /// Loads the selected tournament context into the bracket page and triggers an API-backed refresh.
        /// </summary>
        /// <param name="tournament">The tournament to display.</param>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            _ = RefreshBracketAsync();
        }

        /// <summary>
        /// Toggles the visibility of the user control's containers to display the requested view.
        /// </summary>
        /// <param name="viewName">The name of the view to display. Expected values are "Matches" to show the DataGridView, or "Bracket" to show the visual panel.</param>
        private void ShowView(string viewName)
        {
            dataGridViewStageMatches.Visible = false;
            panelBracketContainer.Visible = false;

            switch (viewName)
            {
                case "Matches":
                    dataGridViewStageMatches.Visible = true;
                    dataGridViewStageMatches.BringToFront();
                    break;

                case "Bracket":
                    panelBracketContainer.Visible = true;
                    panelBracketContainer.BringToFront();
                    break;

                default:
                    dataGridViewStageMatches.Visible = true;
                    dataGridViewStageMatches.BringToFront();
                    break;
            }
        }

        /// <summary>
        /// Fetches persisted matches for the current tournament and updates both grid and bracket views.
        /// </summary>
        /// <returns>A task representing the asynchronous refresh operation.</returns>
        private async Task RefreshBracketAsync()
        {
            dataGridViewStageMatches.Rows.Clear();
            panelBracketContainer.Controls.Clear();

            if (currentTournament == null)
                return;

            _apiMatches = await _apiClient.GetMatchesForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.MatchReadDto>();

            foreach (var match in _apiMatches)
            {
                var scoreText = (match.ScoreA.HasValue && match.ScoreB.HasValue)
                    ? $"{match.ScoreA.Value} - {match.ScoreB.Value}"
                    : "-";

                dataGridViewStageMatches.Rows.Add(
                    match.TeamAName ?? "TBD",
                    match.TeamBName ?? "TBD",
                    match.Status,
                    scoreText);
            }

            DrawBracketTreeFromApiMatches();
        }

        /// <summary>
        /// Renders a bracket-style visual layout from API match DTOs currently loaded in memory.
        /// </summary>
        private void DrawBracketTreeFromApiMatches()
        {
            panelBracketContainer.Controls.Clear();
            _renderedMatchBoxes.Clear();

            if (_apiMatches.Count == 0)
            {
                Label noMatches = new Label
                {
                    Text = "No matches to display",
                    AutoSize = true,
                    Location = new Point(20, 20),
                    Font = new Font("Segoe UI", 14)
                };
                panelBracketContainer.Controls.Add(noMatches);
                return;
            }

            panelBracketContainer.AutoScroll = true;

            int matchCount = _apiMatches.Count;
            int availableHeight = panelBracketContainer.Height - 100;
            int totalMatchHeight = matchCount * MATCH_BOX_HEIGHT;
            int totalSpacingNeeded = availableHeight - totalMatchHeight;
            int verticalGap = MATCH_BOX_HEIGHT + (totalSpacingNeeded / Math.Max(1, matchCount));
            verticalGap = Math.Max(verticalGap, MATCH_BOX_HEIGHT + 20);

            int totalRoundHeight = matchCount * verticalGap;
            int startY = Math.Max(50, (panelBracketContainer.Height - totalRoundHeight) / 2);

            for (int i = 0; i < matchCount; i++)
            {
                var match = _apiMatches[i];

                GroupBox box = new GroupBox
                {
                    Location = new Point(50, startY + (i * verticalGap)),
                    Size = new Size(MATCH_BOX_WIDTH, MATCH_BOX_HEIGHT),
                    Text = $"R1-{i + 1}",
                    Padding = new Padding(5)
                };

                Label teamALabel = new Label
                {
                    Text = match.TeamAName ?? "TBD",
                    Location = new Point(10, 45),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F)
                };

                Label teamBLabel = new Label
                {
                    Text = match.TeamBName ?? "TBD",
                    Location = new Point(10, 85),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F)
                };

                if (string.Equals(match.Status, "Complete", StringComparison.OrdinalIgnoreCase))
                {
                    if (match.ScoreA.HasValue && match.ScoreB.HasValue)
                    {
                        if (match.ScoreA.Value > match.ScoreB.Value)
                            teamALabel.Font = new Font(teamALabel.Font, FontStyle.Bold);
                        else if (match.ScoreB.Value > match.ScoreA.Value)
                            teamBLabel.Font = new Font(teamBLabel.Font, FontStyle.Bold);
                    }

                    box.BackColor = Color.LightGreen;
                }

                box.Controls.Add(teamALabel);
                box.Controls.Add(teamBLabel);
                panelBracketContainer.Controls.Add(box);
                _renderedMatchBoxes.Add(box);
            }
            panelBracketContainer.Invalidate();
        }

        /// <summary>
        /// Paint handler for bracket connector lines based on rendered match box positions.
        /// </summary>
        /// <param name="sender">The source panel.</param>
        /// <param name="e">Paint event data.</param>
        private void PanelBracketContainer_Paint(object? sender, PaintEventArgs e)
        {
            if (!panelBracketContainer.Visible || _renderedMatchBoxes.Count < 2)
            {
                return;
            }

            DrawConnectingLinesFromRenderedBoxes(e.Graphics);
        }

        /// <summary>
        /// Draws connector lines between adjacent rendered match boxes.
        /// </summary>
        /// <param name="g">Graphics surface for drawing lines.</param>
        private void DrawConnectingLinesFromRenderedBoxes(Graphics g)
        {
            if (_renderedMatchBoxes.Count < 2)
            {
                return;
            }

            using Pen linePen = new Pen(Color.Black, 2);

            // Keep original insertion order so pairing stays consistent
            for (int i = 0; i < _renderedMatchBoxes.Count; i += 2)
            {
                if (i + 1 >= _renderedMatchBoxes.Count)
                {
                    break;
                }

                GroupBox topBox = _renderedMatchBoxes[i];
                GroupBox bottomBox = _renderedMatchBoxes[i + 1];

                // Convert box points to panel client coordinates (handles autoscroll correctly)
                Point topCenterRight = panelBracketContainer.PointToClient(
                    topBox.PointToScreen(new Point(topBox.Width, topBox.Height / 2)));

                Point bottomCenterRight = panelBracketContainer.PointToClient(
                    bottomBox.PointToScreen(new Point(bottomBox.Width, bottomBox.Height / 2)));

                int x1 = topCenterRight.X;
                int y1 = topCenterRight.Y;

                int x2 = bottomCenterRight.X;
                int y2 = bottomCenterRight.Y;

                int midX = x1 + 50;
                int outX = midX + 50;
                int midY = (y1 + y2) / 2;

                g.DrawLine(linePen, x1, y1, midX, y1);
                g.DrawLine(linePen, x2, y2, midX, y2);
                g.DrawLine(linePen, midX, y1, midX, y2);
                g.DrawLine(linePen, midX, midY, outX, midY);
            }
        }

        /// <summary>
        /// Initializes the stage matches grid columns if they have not already been added.
        /// This prevents duplicate columns when the control is reloaded.
        /// </summary>
        private void InitializeMatchesGrid()
        {
            // Only add columns if they don't exist
            if (dataGridViewStageMatches.Columns.Count == 0)
            {
                dataGridViewStageMatches.Columns.Add("columnTeamA", "Team A");
                dataGridViewStageMatches.Columns.Add("columnTeamB", "Team B");
                dataGridViewStageMatches.Columns.Add("columnStatus", "Status");
                dataGridViewStageMatches.Columns.Add("columnScore", "Score");
            }
        }
    }
}

        

    

        


