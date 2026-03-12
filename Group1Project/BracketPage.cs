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

        /// <summary>
        /// Tournament object that holds the current tournament data, including the bracket and matches.
        /// This is used to populate both the DataGridView and the visual bracket panel. It is nullable to allow for the case where no tournament has been loaded or generated yet.
        /// </summary>
        private Tournament? currentTournament;

        /// <summary>
        /// BracketPage constructor initializes the user control, sets up event handlers for the view selection and resizing, and defaults to showing the bracket view.
        /// It also initializes the matches grid columns if they haven't been set up yet.
        /// </summary>
        public BracketPage()
        {
            InitializeComponent();

            InitializeMatchesGrid();

            // Set up event handler for view selection changes
            comboBoxView.SelectedIndexChanged += (s, e) =>
            {
                var view = comboBoxView.SelectedItem?.ToString() ?? "Matches";
                ShowView(view);
            };

            // Set up event handler for resizing the bracket panel to redraw the bracket tree when the size changes
            panelBracketContainer.Resize += (s, e) =>
            {
                if (currentTournament?.Bracket != null && panelBracketContainer.Visible)
                {
                    DrawBracketTree();
                }
            };

            if (comboBoxView.Items.Count > 0)
            { 
                comboBoxView.SelectedIndex = 0;
            }
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

        /// <summary>
        /// Loads the provided Tournament object into the BracketPage, updating the current tournament reference and refreshing the UI to display the new tournament data. 
        /// This includes populating the matches grid and redrawing the visual bracket tree based on the matches in the tournament's bracket.
        /// </summary>
        /// <param name="tournament">
        ///     The Tournament object containing the bracket and matches to be displayed. This method updates the current tournament reference and refreshes the UI to show the new tournament data.
        /// </param>
        internal void LoadTournament(Tournament tournament)
        {
            currentTournament = tournament;
            RefreshBracket();
        }

        /// <summary>
        /// Refreshes the bracket display by clearing existing data and repopulating the matches grid and visual bracket panel based on the current tournament's bracket data.
        /// </summary>
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

        /// <summary>
        /// Draws the visual representation of the tournament bracket in the panelBracketContainer. 
        /// It creates a custom drawing panel where match boxes are displayed for each match in the current tournament's bracket, and connecting lines are drawn to represent the progression of teams through the rounds. If there are no matches to display, it shows a message indicating that there are no matches available. 
        /// The method also handles resizing of the panel to ensure the bracket is redrawn appropriately when the size changes.
        /// </summary>
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

        /// <summary>
        /// Custom paint event handler for the drawing panel that renders the visual bracket tree. 
        /// It draws connecting lines between match boxes to represent the progression of teams through the rounds. 
        /// The method checks if there is a valid tournament and bracket before attempting to draw, and it uses the Graphics object provided by the Paint event to render the lines on the panel.
        /// </summary>
        /// <param name="g">Graphics object used for drawing the bracket lines on the panel. This is provided by the Paint event of the drawing panel.</param>
        private void DrawBracketOnPanel(Graphics g)
        {
            if (currentTournament?.Bracket == null) 
            { 
                return;
            }

            // Draw connecting lines between rounds
            DrawConnectingLines(g);
        }

        /// <summary>
        /// Draws match boxes for each match in the first round of the tournament bracket on the provided drawing panel.
        /// </summary>
        /// <param name="drawingPanel">
        ///     Panel on which the match boxes will be drawn. 
        ///     This panel is created in the DrawBracketTree method and is where the visual representation of the matches will be rendered.
        /// </param>
        private void DrawMatchBoxes(Panel drawingPanel)
        {
            if (currentTournament?.Bracket == null)
            {
                return;
            }

            int round = 1;
            var matchesInRound = GetMatchesForRound(round);
            int matchCount = matchesInRound.Count;

            if (matchCount == 0) 
            { 
                return; 
            }

            // Calculate vertical spacing to distribute match boxes evenly within the available height of the drawing panel
            int availableHeight = drawingPanel.Height - 100;
            int totalMatchHeight = matchCount * MATCH_BOX_HEIGHT;
            int totalSpacingNeeded = availableHeight - totalMatchHeight;
            int verticalGap = MATCH_BOX_HEIGHT + (totalSpacingNeeded / Math.Max(1, matchCount));

            // Ensure a minimum vertical gap to prevent overlap of match boxes
            verticalGap = Math.Max(verticalGap, MATCH_BOX_HEIGHT + 20);

            int totalRoundHeight = matchCount * verticalGap;
            int startY = Math.Max(50, (drawingPanel.Height - totalRoundHeight) / 2);

            // Draw match boxes for the first round
            for (int i = 0; i < matchCount; i++)
            {
                var match = matchesInRound[i];

                int x = 50;
                int y = startY + (i * verticalGap);

                GroupBox matchBox = CreateMatchBox(match, x, y, round, i);
                drawingPanel.Controls.Add(matchBox);
            }
        }

        /// <summary>
        /// Creates a GroupBox control representing a match in the tournament bracket.
        /// </summary>
        /// <param name="match">Match object containing the details of the match, including the teams involved and the match status. This information is used to populate the content of the match box.</param>
        /// <param name="x">Pixel X coordinate for the location of the match box on the drawing panel. This determines the horizontal position of the box in the bracket layout.</param>
        /// <param name="y">Pixel Y coordinate for the location of the match box on the drawing panel. This determines the vertical position of the box in the bracket layout.</param>
        /// <param name="round">Numeric identifier for the round of the tournament that this match belongs to. This is used to label the match box with the appropriate round information.</param>
        /// <param name="matchIndex">Index of the match within its round, used for labeling purposes to differentiate between multiple matches in the same round. This helps in identifying the match as "R1-1", "R1-2", etc. for the first round.</param>
        /// <returns>
        ///     GroupBox control populated with the match information, styled according to the match status (e.g., completed matches highlighted in green, bye matches highlighted in yellow). 
        ///     This control is added to the drawing panel to visually represent the match in the bracket.
        /// </returns>
        private GroupBox CreateMatchBox(Match match, int x, int y, int round, int matchIndex)
        {
            // Determine if the match is a bye (where both teams are the same or one team is null), which indicates that one team automatically advances to the next round without playing a match.
            bool isBye = match.TeamA == match.TeamB;

            // Create a GroupBox to represent the match, with appropriate styling based on whether it's a bye or a regular match.
            GroupBox box = new GroupBox
            {
                Location = new Point(x, y),
                Size = new Size(MATCH_BOX_WIDTH, MATCH_BOX_HEIGHT),
                Text = isBye ? $"R{round}-BYE" : $"R{round}-{matchIndex + 1}",
                Padding = new Padding(5)
            };

            // If it's a bye match, display a special message and style the box differently. Otherwise, display the team names and highlight the winner if the match is complete.
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
                // Display team names and match status
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

                // If the match is complete, highlight the winner and change the background color of the box to indicate completion.
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

        /// <summary>
        /// Draws connecting lines between match boxes on the provided Graphics object to visually represent the progression of teams through the rounds in the tournament bracket.
        /// </summary>
        /// <param name="g">
        ///     Graphics object used for drawing the connecting lines on the panel.
        ///     This is provided by the Paint event of the drawing panel and allows for custom rendering of the bracket connections.
        /// </param>
        private void DrawConnectingLines(Graphics g)
        {
            if (currentTournament?.Bracket == null)
            {
                return;
            }

            // For simplicity, this example only draws lines for the first round of matches.
            // In a complete implementation, you would need to recursively draw lines for subsequent rounds based on the structure of the bracket and the matches that have been completed.
            int round = 1;
            var currentRoundMatches = GetMatchesForRound(round);
            int matchCount = currentRoundMatches.Count;

            if (matchCount < 2)
            {
                return;
            }

            // Calculate vertical spacing to position the connecting lines appropriately between match boxes
            Pen linePen = new Pen(Color.Black, 2);

            int availableHeight = (int)g.VisibleClipBounds.Height - 100;
            int totalMatchHeight = matchCount * MATCH_BOX_HEIGHT;
            int totalSpacingNeeded = availableHeight - totalMatchHeight;
            int verticalGap = MATCH_BOX_HEIGHT + (totalSpacingNeeded / Math.Max(1, matchCount));

            verticalGap = Math.Max(verticalGap, MATCH_BOX_HEIGHT + 20);

            int totalRoundHeight = matchCount * verticalGap;
            int startY = Math.Max(50, ((int)g.VisibleClipBounds.Height - totalRoundHeight) / 2);

            // Draw lines connecting pairs of matches in the first round to the next round.
            // This is a simplified example and assumes a single-elimination bracket where each pair of matches feeds into one match in the next round.
            // In a complete implementation, you would need to handle more complex bracket structures and multiple rounds.
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

        /// <summary>
        /// Gets the list of matches for a specific round number from the current tournament's bracket.
        /// </summary>
        /// <param name="roundNumber">Integer representing the round number for which to retrieve matches. Round numbers typically start at 1 for the first round of the tournament.</param>
        /// <returns>LIst of Match objects that belong to the specified round. If there are no matches for the given round or if the tournament/bracket is not available, it returns an empty list.</returns>
        private List<Match> GetMatchesForRound(int roundNumber)
        {
            if (currentTournament?.Bracket == null)
            {
                return new List<Match>();
            }

            if (roundNumber == 1)
            {
                return currentTournament.Bracket.Matches.ToList();
            }

            return new List<Match>();
        }

        /// <summary>
        /// Creates the columns for the matches DataGridView if they haven't been created yet.
        /// This method checks if the columns already exist to avoid adding duplicate columns, 
        /// it also sets up the necessary columns to display team names, match status, and scores for the matches in the tournament bracket.
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

        

    

        


