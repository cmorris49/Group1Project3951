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
    /// <summary>
    /// Represents the bracket user interface for viewing tournament matches in grid and visual bracket formats.
    /// </summary>
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

        private readonly ApiClient _apiClient = new ApiClient();

        private List<ApiClient.MatchReadDto> _apiMatches = new List<ApiClient.MatchReadDto>();

        private readonly List<GroupBox> _renderedMatchBoxes = new List<GroupBox>();

        private readonly Dictionary<(int Round, int Match), GroupBox> _matchBoxes = new Dictionary<(int Round, int Match), GroupBox>();

        private readonly List<(GroupBox From, GroupBox To)> _connectorPairs = new List<(GroupBox From, GroupBox To)>();

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
            { 
                comboBoxView.SelectedIndex = 0;
            }
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
            _matchBoxes.Clear();
            _connectorPairs.Clear();

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

            var rounds = _apiMatches
                .OrderBy(m => m.RoundNumber)
                .ThenBy(m => m.MatchNumber)
                .GroupBy(m => m.RoundNumber)
                .OrderBy(g => g.Key)
                .ToList();

            int firstRoundCount = rounds.First().Count();
            int verticalGap = MATCH_BOX_HEIGHT + MATCH_VERTICAL_SPACING;
            int totalRoundHeight = firstRoundCount * verticalGap;
            int startY = Math.Max(50, (panelBracketContainer.Height - totalRoundHeight) / 2);

            foreach (var roundGroup in rounds)
            {
                int roundNumber = roundGroup.Key;
                int x = 50 + ((roundNumber - 1) * ROUND_SPACING);

                var matchesInRound = roundGroup.OrderBy(m => m.MatchNumber).ToList();

                for (int i = 0; i < matchesInRound.Count; i++)
                {
                    var match = matchesInRound[i];
                    int y;

                    if (roundNumber == 1)
                    {
                        y = startY + (i * verticalGap);
                    }
                    else
                    {
                        var parentAKey = (roundNumber - 1, (match.MatchNumber * 2) - 1);
                        var parentBKey = (roundNumber - 1, match.MatchNumber * 2);

                        bool hasA = _matchBoxes.TryGetValue(parentAKey, out GroupBox? parentA);
                        bool hasB = _matchBoxes.TryGetValue(parentBKey, out GroupBox? parentB);

                        if (hasA && hasB)
                        {
                            int centerA = parentA!.Top + (parentA.Height / 2);
                            int centerB = parentB!.Top + (parentB.Height / 2);
                            y = ((centerA + centerB) / 2) - (MATCH_BOX_HEIGHT / 2);
                        }
                        else if (hasA)
                        {
                            y = (parentA!.Top + (parentA.Height / 2)) - (MATCH_BOX_HEIGHT / 2);
                        }
                        else if (hasB)
                        {
                            y = (parentB!.Top + (parentB.Height / 2)) - (MATCH_BOX_HEIGHT / 2);
                        }
                        else
                        {
                            y = startY + (i * verticalGap);
                        }
                    }

                    GroupBox box = new GroupBox
                    {
                        Location = new Point(x, y),
                        Size = new Size(MATCH_BOX_WIDTH, MATCH_BOX_HEIGHT),
                        Text = $"R{roundNumber}-{match.MatchNumber}",
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

                    if (string.Equals(match.Status, "Complete", StringComparison.OrdinalIgnoreCase) &&
                        match.ScoreA.HasValue &&
                        match.ScoreB.HasValue)
                    {
                        if (match.ScoreA.Value > match.ScoreB.Value)
                        {
                            teamALabel.Font = new Font(teamALabel.Font, FontStyle.Bold);
                        }
                        else if (match.ScoreB.Value > match.ScoreA.Value)
                        {
                            teamBLabel.Font = new Font(teamBLabel.Font, FontStyle.Bold);
                        }

                        box.BackColor = Color.LightGreen;
                    }

                    box.Controls.Add(teamALabel);
                    box.Controls.Add(teamBLabel);
                    panelBracketContainer.Controls.Add(box);

                    _renderedMatchBoxes.Add(box);
                    _matchBoxes[(roundNumber, match.MatchNumber)] = box;

                    if (roundNumber > 1)
                    {
                        var parentAKey = (roundNumber - 1, (match.MatchNumber * 2) - 1);
                        var parentBKey = (roundNumber - 1, match.MatchNumber * 2);

                        if (_matchBoxes.TryGetValue(parentAKey, out GroupBox? parentA))
                        {
                            _connectorPairs.Add((parentA, box));
                        }

                        if (_matchBoxes.TryGetValue(parentBKey, out GroupBox? parentB))
                        {
                            _connectorPairs.Add((parentB, box));
                        }
                    }
                }
            }

            panelBracketContainer.Invalidate();
        }

        /// <summary>
        /// Draws connector lines between related rendered match boxes across rounds.
        /// </summary>
        /// <param name="g">Graphics surface for drawing lines.</param>
        private void DrawConnectingLinesFromRenderedBoxes(Graphics g)
        {
            if (_connectorPairs.Count == 0)
            {
                return;
            }

            using Pen linePen = new Pen(Color.Black, 2);

            foreach (var pair in _connectorPairs)
            {
                Point from = panelBracketContainer.PointToClient(
                    pair.From.PointToScreen(new Point(pair.From.Width, pair.From.Height / 2)));

                Point to = panelBracketContainer.PointToClient(
                    pair.To.PointToScreen(new Point(0, pair.To.Height / 2)));

                int midX = from.X + 30;

                g.DrawLine(linePen, from.X, from.Y, midX, from.Y);
                g.DrawLine(linePen, midX, from.Y, midX, to.Y);
                g.DrawLine(linePen, midX, to.Y, to.X, to.Y);
            }
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

        

    

        


