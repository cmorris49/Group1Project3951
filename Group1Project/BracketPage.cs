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
        public BracketPage()
        {
            InitializeComponent();

            InitializeMatchesGrid();

            comboBoxView.SelectedIndexChanged += (s, e) =>
            {
                var view = comboBoxView.SelectedItem?.ToString() ?? "Matches";
                ShowView(view);
            };

            panelBracketContainer.Resize += (s, e) =>
            {
                if (currentTournament?.Bracket != null && panelBracketContainer.Visible)
                {
                    DrawBracketTree();
                }
            };

            if (comboBoxView.Items.Count > 0)
                comboBoxView.SelectedIndex = 0;

            ShowView("Bracket");
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

        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            RefreshBracket();
        }

        private void RefreshBracket()
        {
            dataGridViewStageMatches.Rows.Clear();
            panelBracketContainer.Controls.Clear();

            if (currentTournament?.Bracket == null)
            {
                MessageBox.Show("No bracket generated yet. Please generate a bracket first.",
                    "No Bracket", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Display matches in the grid view
            foreach (var match in currentTournament.Bracket.Matches)
            {
                dataGridViewStageMatches.Rows.Add(
                    match.TeamA?.Name ?? "TBD",
                    match.TeamB?.Name ?? "TBD",
                    match.Status.ToString(),
                    match.Score != null ? $"{match.Score.ScoreA} - {match.Score.ScoreB}" : "-"
                );
            }

            // Draw visual bracket tree
            DrawBracketTree();
        }

        private void DrawBracketTree()
        {
            panelBracketContainer.Controls.Clear();

            if (currentTournament?.Bracket == null || currentTournament.Bracket.Matches.Count == 0)
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

            int totalRounds = 1;
            int firstRoundMatches = currentTournament.Bracket.Matches.Count;

            int availableWidth = panelBracketContainer.Width;
            int availableHeight = panelBracketContainer.Height;

            int minWidth = (totalRounds * ROUND_SPACING) + ROUND_SPACING + 100;

            int totalWidth = Math.Max(minWidth, availableWidth);
            int totalHeight = availableHeight; 

            Panel drawingPanel = new Panel
            {
                Width = totalWidth,
                Height = totalHeight,
                Location = new Point(0, 0),
                BackColor = Color.White
            };

            drawingPanel.Paint += (s, e) => DrawBracketOnPanel(e.Graphics);

            panelBracketContainer.Controls.Add(drawingPanel);

            DrawMatchBoxes(drawingPanel);

            drawingPanel.Invalidate();
        }

        private void DrawBracketOnPanel(Graphics g)
        {
            if (currentTournament?.Bracket == null)
                return;

            // Draw connecting lines between rounds
            DrawConnectingLines(g);
        }
        private void DrawMatchBoxes(Panel drawingPanel)
        {
            if (currentTournament?.Bracket == null)
                return;

            int round = 1;
            var matchesInRound = GetMatchesForRound(round);
            int matchCount = matchesInRound.Count;

            if (matchCount == 0) return;

            int availableHeight = drawingPanel.Height - 100;
            int totalMatchHeight = matchCount * MATCH_BOX_HEIGHT;
            int totalSpacingNeeded = availableHeight - totalMatchHeight;
            int verticalGap = MATCH_BOX_HEIGHT + (totalSpacingNeeded / Math.Max(1, matchCount));

            verticalGap = Math.Max(verticalGap, MATCH_BOX_HEIGHT + 20);

            int totalRoundHeight = matchCount * verticalGap;
            int startY = Math.Max(50, (drawingPanel.Height - totalRoundHeight) / 2);

            for (int i = 0; i < matchCount; i++)
            {
                var match = matchesInRound[i];

                int x = 50;
                int y = startY + (i * verticalGap);

                GroupBox matchBox = CreateMatchBox(match, x, y, round, i);
                drawingPanel.Controls.Add(matchBox);
            }
        }

        private GroupBox CreateMatchBox(Match match, int x, int y, int round, int matchIndex)
        {
            bool isBye = match.TeamA == match.TeamB;

            GroupBox box = new GroupBox
            {
                Location = new Point(x, y),
                Size = new Size(MATCH_BOX_WIDTH, MATCH_BOX_HEIGHT),
                Text = isBye ? $"R{round}-BYE" : $"R{round}-{matchIndex + 1}",
                Padding = new Padding(5)
            };

            if (isBye)
            {
                Label byeLabel = new Label
                {
                    Text = $"{match.TeamA?.Name ?? "TBD"}\n(BYE - Auto Advance)",
                    Location = new Point(10, 50), 
                    AutoSize = false,
                    Width = MATCH_BOX_WIDTH - 20,
                    Height = MATCH_BOX_HEIGHT - 60,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                box.Controls.Add(byeLabel);
                box.BackColor = Color.LightYellow;
            }
            else
            {
                Label teamALabel = new Label
                {
                    Text = match.TeamA?.Name ?? "TBD",
                    Location = new Point(10, 45),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F)
                };

                Label teamBLabel = new Label
                {
                    Text = match.TeamB?.Name ?? "TBD",
                    Location = new Point(10, 85),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9F)
                };

                box.Controls.Add(teamALabel);
                box.Controls.Add(teamBLabel);

                if (match.IsComplete())
                {
                    var winner = match.GetWinner();
                    if (winner != null)
                    {
                        if (winner == match.TeamA)
                            teamALabel.Font = new Font(teamALabel.Font, FontStyle.Bold);
                        else if (winner == match.TeamB)
                            teamBLabel.Font = new Font(teamBLabel.Font, FontStyle.Bold);
                    }

                    box.BackColor = Color.LightGreen;
                }
            }

            return box;
        }

        private void DrawConnectingLines(Graphics g)
        {
            if (currentTournament?.Bracket == null)
                return;

            int round = 1;
            var currentRoundMatches = GetMatchesForRound(round);
            int matchCount = currentRoundMatches.Count;

            if (matchCount < 2) return;

            Pen linePen = new Pen(Color.Black, 2);

            int availableHeight = (int)g.VisibleClipBounds.Height - 100;
            int totalMatchHeight = matchCount * MATCH_BOX_HEIGHT;
            int totalSpacingNeeded = availableHeight - totalMatchHeight;
            int verticalGap = MATCH_BOX_HEIGHT + (totalSpacingNeeded / Math.Max(1, matchCount));

            verticalGap = Math.Max(verticalGap, MATCH_BOX_HEIGHT + 20);

            int totalRoundHeight = matchCount * verticalGap;
            int startY = Math.Max(50, ((int)g.VisibleClipBounds.Height - totalRoundHeight) / 2);

            for (int i = 0; i < matchCount; i += 2)
            {
                if (i + 1 >= matchCount) break;

                int x1 = 50 + MATCH_BOX_WIDTH;
                int y1 = startY + (i * verticalGap) + MATCH_BOX_HEIGHT / 2;

                int x2 = 50 + MATCH_BOX_WIDTH;
                int y2 = startY + ((i + 1) * verticalGap) + MATCH_BOX_HEIGHT / 2;

                int x3 = 50 + MATCH_BOX_WIDTH + 100;
                int y3 = (y1 + y2) / 2;

                g.DrawLine(linePen, x1, y1, x1 + 50, y1);
                g.DrawLine(linePen, x2, y2, x2 + 50, y2);
                g.DrawLine(linePen, x1 + 50, y1, x1 + 50, y2);
                g.DrawLine(linePen, x1 + 50, y3, x3, y3);
            }
        }

        private List<Match> GetMatchesForRound(int roundNumber)
        {
            if (currentTournament?.Bracket == null)
                return new List<Match>();

            if (roundNumber == 1)
                return currentTournament.Bracket.Matches.ToList();

            return new List<Match>();
        }

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

        

    

        


