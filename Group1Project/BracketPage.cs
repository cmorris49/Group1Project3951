using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

        // "Generate Next Round" button shown only for Swiss brackets
        private Button _btnNextRound = new Button();

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
                    if (currentTournament?.BracketType == BracketType.RoundRobin)
                        DrawRoundRobinPanel();
                    else if (currentTournament?.BracketType == BracketType.Swiss)
                        DrawSwissPanel();
                    else
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

            string bracketTypeName = currentTournament.BracketType.ToString();

            switch(bracketTypeName)
            {
                case "RoundRobin":
                    DrawRoundRobinPanel();
                    break;
                case "Swiss":
                    buttonNextRound.Visible = true;
                    DrawSwissPanel();
                    break;
                default:
                    DrawBracketTreeFromApiMatches();
                    break;
            }
        }

        /// <summary>
        /// Renders a round-by-round table layout for Round Robin tournaments.
        /// Each round is shown as a labelled group of matches.
        /// </summary>
        private void DrawRoundRobinPanel()
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
                .GroupBy(m => m.RoundNumber)
                .OrderBy(g => g.Key)
                .ToList();

            int y = 10;
            const int rowHeight = 24;
            const int headerHeight = 28;
            const int roundPadding = 12;

            foreach (var roundGroup in rounds)
            {
                var matchesInRound = roundGroup.OrderBy(m => m.MatchNumber).ToList();

                // Round header label
                Label roundLabel = new Label
                {
                    Text = $"Round {roundGroup.Key}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location = new Point(10, y),
                    AutoSize = true,
                    ForeColor = Color.DarkBlue
                };
                panelBracketContainer.Controls.Add(roundLabel);
                y += headerHeight;

                foreach (var match in matchesInRound)
                {
                    string score = (match.ScoreA.HasValue && match.ScoreB.HasValue)
                        ? $"{match.ScoreA.Value} - {match.ScoreB.Value}"
                        : "vs";

                    string status = string.Equals(match.Status, "Complete",
                        StringComparison.OrdinalIgnoreCase) ? "✓" : "·";

                    Label matchLabel = new Label
                    {
                        Text = $"  {status}  {(match.TeamAName ?? "TBD").PadRight(20)} {score.PadLeft(7)}  {match.TeamBName ?? "TBD"}",
                        Font = new Font("Consolas", 9),
                        Location = new Point(20, y),
                        AutoSize = true,
                        ForeColor = string.Equals(match.Status, "Complete",
                            StringComparison.OrdinalIgnoreCase) ? Color.DarkGreen : Color.Black
                    };
                    panelBracketContainer.Controls.Add(matchLabel);
                    y += rowHeight;
                }

                y += roundPadding;
            }
        }

        /// <summary>
        /// Renders a round-by-round table layout for Swiss tournaments.
        /// Shows win counts next to each team name and highlights the current (latest) round.
        /// </summary>
        private void DrawSwissPanel()
        {
            panelBracketContainer.Controls.Clear();
            _renderedMatchBoxes.Clear();
            _matchBoxes.Clear();
            _connectorPairs.Clear();

            if (_apiMatches.Count == 0)
            {
                Label noMatches = new Label
                {
                    Text = "No matches yet — click Generate to create Round 1.",
                    AutoSize = true,
                    Location = new Point(20, 20),
                    Font = new Font("Segoe UI", 11)
                };
                panelBracketContainer.Controls.Add(noMatches);
                return;
            }

            panelBracketContainer.AutoScroll = true;

            // Build win counts from completed matches
            var wins = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var m in _apiMatches.Where(m =>
                string.Equals(m.Status, "Complete", StringComparison.OrdinalIgnoreCase)
                && m.ScoreA.HasValue && m.ScoreB.HasValue
                && m.TeamAId != null && m.TeamBId != null))
            {
                wins.TryAdd(m.TeamAId!, 0);
                wins.TryAdd(m.TeamBId!, 0);
                if (m.ScoreA > m.ScoreB) wins[m.TeamAId!]++;
                else if (m.ScoreB > m.ScoreA) wins[m.TeamBId!]++;
            }

            var rounds = _apiMatches
                .GroupBy(m => m.RoundNumber)
                .OrderBy(g => g.Key)
                .ToList();

            int latestRound = rounds.Max(g => g.Key);

            int y = 10;
            const int rowHeight = 26;
            const int headerHeight = 30;
            const int roundPadding = 14;

            foreach (var roundGroup in rounds)
            {
                bool isCurrentRound = roundGroup.Key == latestRound;
                var matchesInRound = roundGroup.OrderBy(m => m.MatchNumber).ToList();

                // Round header
                Label roundLabel = new Label
                {
                    Text = $"Round {roundGroup.Key}{(isCurrentRound ? "  ◀ current" : "")}",
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location = new Point(10, y),
                    AutoSize = true,
                    ForeColor = isCurrentRound ? Color.DarkRed : Color.DarkBlue
                };
                panelBracketContainer.Controls.Add(roundLabel);
                y += headerHeight;

                foreach (var match in matchesInRound)
                {
                    bool complete = string.Equals(match.Status, "Complete", StringComparison.OrdinalIgnoreCase);
                    string status = complete ? "✓" : "·";

                    string nameA = match.TeamAName ?? "TBD";
                    string nameB = match.TeamBName ?? "TBD";

                    // Append win count if known
                    int wA = match.TeamAId != null && wins.TryGetValue(match.TeamAId, out var wa) ? wa : 0;
                    int wB = match.TeamBId != null && wins.TryGetValue(match.TeamBId, out var wb) ? wb : 0;
                    string labelA = $"{nameA} ({wA}W)";
                    string labelB = $"{nameB} ({wB}W)";

                    string score = (match.ScoreA.HasValue && match.ScoreB.HasValue)
                        ? $"{match.ScoreA.Value} - {match.ScoreB.Value}"
                        : "vs";

                    Label matchLabel = new Label
                    {
                        Text = $"  {status}  {labelA.PadRight(22)} {score.PadLeft(7)}  {labelB}",
                        Font = new Font("Consolas", 9),
                        Location = new Point(20, y),
                        AutoSize = true,
                        ForeColor = complete ? Color.DarkGreen : Color.Black
                    };
                    panelBracketContainer.Controls.Add(matchLabel);
                    y += rowHeight;
                }

                y += roundPadding;
            }

            // Standings summary at the bottom
            if (wins.Count > 0)
            {
                y += 10;
                Label standingsHeader = new Label
                {
                    Text = "── Standings ──",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Location = new Point(10, y),
                    AutoSize = true,
                    ForeColor = Color.Gray
                };
                panelBracketContainer.Controls.Add(standingsHeader);
                y += 24;

                foreach (var entry in wins.OrderByDescending(kv => kv.Value))
                {
                    // Find team name from matches
                    string teamName = _apiMatches
                        .Where(m => string.Equals(m.TeamAId, entry.Key, StringComparison.OrdinalIgnoreCase))
                        .Select(m => m.TeamAName)
                        .FirstOrDefault()
                        ?? _apiMatches
                            .Where(m => string.Equals(m.TeamBId, entry.Key, StringComparison.OrdinalIgnoreCase))
                            .Select(m => m.TeamBName)
                            .FirstOrDefault()
                        ?? entry.Key;

                    Label row = new Label
                    {
                        Text = $"  {teamName.PadRight(20)} {entry.Value} W",
                        Font = new Font("Consolas", 9),
                        Location = new Point(20, y),
                        AutoSize = true
                    };
                    panelBracketContainer.Controls.Add(row);
                    y += 22;
                }
            }
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

        /// <summary>
        /// Handles the click event for the Export Bracket button, exporting the current tournament's details,
        /// divisions, teams, players, and matches to a text file.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Export Bracket button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private async void buttonExportBracket_Click(object sender, EventArgs e)
        {
            if (currentTournament == null)
            {
                MessageBox.Show("No tournament loaded to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _btnNextRound.Enabled = false;
            buttonExportBracket.Enabled = false;

            try
            {
                // Fetch latest teams and matches from API to ensure up-to-date export
                var teams = await _apiClient.GetTeamsForTournamentAsync(currentTournament.Id) ?? new List<ApiClient.TeamReadDto>();
                var matches = await _apiClient.GetMatchesForTournamentAsync(currentTournament.Id) ?? new List<ApiClient.MatchReadDto>();

                // Build export string with tournament details, divisions, teams, players, and matches
                var sb = new StringBuilder();
                sb.AppendLine($"Tournament: {currentTournament.Name}");
                sb.AppendLine($"Id: {currentTournament.Id}");
                sb.AppendLine($"Location: {currentTournament.Location}");
                sb.AppendLine($"Start: {currentTournament.StartDate:u}");
                sb.AppendLine($"Bracket Type: {currentTournament.BracketType}");
                sb.AppendLine(new string('-', 60));

                // Get divisions and teams for export
                // Format: Division: [Division Name] (Id: [Division Id])
                //         Team: [Team Name]  Seed: [Seed]  Id: [Team Id]
                //         Player: [Player Name]  #[Player Number]  Id: [Player Id]
                //         ~~~~Repeat for each team in the division~~~
                // Teams are grouped under their respective divisions. If a division has no teams, it is noted as "(no teams)".
                sb.AppendLine("Divisions and Teams:");
                foreach (var d in currentTournament.Divisions)
                {
                    sb.AppendLine($" Division: {d.Name} (Id: {d.Id})");
                    var divTeams = teams.Where(t => string.Equals(t.DivisionId, d.Id.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
                    if (divTeams.Count == 0)
                    {
                        sb.AppendLine("  (no teams)");
                    }
                    else
                    {
                        foreach (var t in divTeams)
                        {
                            sb.AppendLine($"  Team: {t.Name}  Seed: {t.Seed}  Id: {t.Id}");
                            if (t.Players != null && t.Players.Count > 0)
                            {
                                foreach (var p in t.Players)
                                {
                                    sb.AppendLine($"    Player: {p.DisplayName}  #{p.Number}  Id: {p.Id}");
                                }
                            }
                        }
                    }
                }

                // Break line between teams and matches
                sb.AppendLine(new string('-', 60));

                // Get matches for export
                // Format: R[Round Number] M[Match Number]: [Team A Name] vs [Team B Name]   Status: [Status]   Score: [ScoreA-ScoreB or - if not available]   Id:[Match Id]
                sb.AppendLine("Matches:");
                foreach (var m in matches.OrderBy(x => x.RoundNumber).ThenBy(x => x.MatchNumber))
                {
                    string score = (m.ScoreA.HasValue && m.ScoreB.HasValue) ? $"{m.ScoreA}-{m.ScoreB}" : "-";
                    sb.AppendLine($"R{m.RoundNumber} M{m.MatchNumber}: {m.TeamAName ?? "TBD"} vs {m.TeamBName ?? "TBD"}   Status: {m.Status}   Score: {score}   Id:{m.Id}");
                }

                sb.AppendLine();

                // Footer with export timestamp
                DateTime exportTime = DateTime.Now;
                sb.AppendLine($"Exported on {exportTime:u}");

                using var sfd = new SaveFileDialog()
                {
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    FileName = $"{currentTournament.Name?.Replace(' ', '_')}_export.txt",
                    Title = "Export Tournament"
                };

                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                await File.WriteAllTextAsync(sfd.FileName, sb.ToString(), Encoding.UTF8);

                MessageBox.Show($"Tournament exported to:\n{sfd.FileName}", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnNextRound.Enabled = true;
                buttonExportBracket.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the click event for the Next Round button, generating the next round in a Swiss tournament if
        /// applicable.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Next Round button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private async void buttonNextRound_Click(object sender, EventArgs e)
        {
            if (currentTournament == null)
            {
                MessageBox.Show("No tournament loaded.", "Generate Next Round", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentTournament.BracketType != BracketType.Swiss)
            {
                MessageBox.Show("Generate Next Round is only available for Swiss tournaments.", "Generate Next Round", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                buttonNextRound.Enabled = false;
                _btnNextRound.Enabled = false;

                bool ok = await _apiClient.GenerateNextRoundAsync(currentTournament.Id);
                if (ok)
                {
                    await RefreshBracketAsync();
                    MessageBox.Show("Next round generated.", "Generate Next Round", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // ApiClient already shows details on failure; provide a brief fallback message
                    MessageBox.Show("Failed to generate next round. See server response for details.", "Generate Next Round", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                buttonNextRound.Enabled = true;
                _btnNextRound.Enabled = true;
            }
        }
    }
}