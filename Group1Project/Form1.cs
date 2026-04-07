/// <summary>
/// Group 1 Project - Main Form
/// Author: Cameron, Jun, Jonathan 
/// Date: March 2, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents the main form of the application, providing a user interface for managing tournaments, teams, and
    /// related activities.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Gets or sets the current tournament instance being managed.
        /// </summary>
        /// <note>
        /// This property may be null if no tournament is currently active.
        /// </note>
        private Tournament? currentTournament;

        /// <summary>
        /// List of tournaments available in the application. 
        /// This collection is used to populate the tournament selection dropdown and manage multiple tournaments within the application.
        /// </summary>
        private List<Tournament> tournaments = new List<Tournament>();

        private readonly ApiClient _apiClient = new ApiClient();

        /// <summary>
        /// Form1 constructor initializes the main form and sets up the user interface components. 
        /// It also includes testing code to create a sample tournament with dummy data for populating the UI during development.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            ApplyToolbarStyle();
        }

        /// <summary>
        /// Applies visual improvements to the toolbar: background color, button spacing,
        /// separator lines, and tooltip text — without touching the Designer file.
        /// </summary>
        private void ApplyToolbarStyle()
        {
            // ── Toolbar background 
            toolMain.BackColor = Color.FromArgb(245, 246, 250);
            toolMain.Padding = new Padding(4, 2, 4, 2);
            toolMain.ImageScalingSize = new Size(24, 24);

            // ── Button styles 
            StyleToolButton(tsbSave, "💾 Save", "Save the current tournament", Color.FromArgb(52, 152, 219));
            StyleToolButton(tsbAddTeam, "➕ Add Team", "Add a new team to the tournament", Color.FromArgb(39, 174, 96));
            StyleToolButton(tsbGenerateBracket, "🏆 Generate Bracket", "Generate the tournament bracket", Color.FromArgb(142, 68, 173));
            StyleToolButton(tsbAutoSchedule, "📅 Auto Schedule", "Automatically schedule all matches", Color.FromArgb(211, 84, 0));
            StyleToolButton(tsbEnterResult, "✔ Enter Result", "Enter a match result", Color.FromArgb(192, 57, 43));

            // ── Separator between groups 
            var sep = new ToolStripSeparator();
            toolMain.Items.Insert(1, sep);
        }

        /// <summary>
        /// Styles an individual toolbar button with consistent padding, font, and tooltip.
        /// </summary>
        private static void StyleToolButton(ToolStripButton btn, string text,
            string tooltip, Color accentColor)
        {
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            btn.ForeColor = Color.FromArgb(50, 50, 60);
            btn.Padding = new Padding(8, 2, 8, 2);
            btn.AutoToolTip = false;
            btn.ToolTipText = tooltip;
            btn.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btn.TextAlign = ContentAlignment.MiddleCenter;

            // Highlight on hover via mouse events
            btn.MouseEnter += (s, e) =>
            {
                btn.ForeColor = accentColor;
                btn.Font = new Font(btn.Font, FontStyle.Bold);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.ForeColor = Color.FromArgb(50, 50, 60);
                btn.Font = new Font(btn.Font, FontStyle.Regular);
            };
        }

        /// <summary>
        /// Handles the form load event by requiring authentication and loading tournament data for the signed-in user.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for form load.</param>
        private async void Form1_Load(object? sender, EventArgs e)
        {
            if (!await EnsureLoggedInAsync())
            {
                LogoutAndExitApplication();
                return;
            }

            await ReloadTournamentsAsync();
        }

        /// <summary>
        /// Ensures that a user is authenticated by displaying the login dialog until login succeeds
        /// or the user cancels. Supports sign-up flow from the same dialog.
        /// </summary>
        /// <returns>True when a user is authenticated; otherwise, false.</returns>
        private async Task<bool> EnsureLoggedInAsync()
        {
            while (!_apiClient.IsLoggedIn)
            {
                using var loginForm = new LoginForm();
                var dialogResult = loginForm.ShowDialog(this);

                if (dialogResult == DialogResult.Cancel)
                {
                    return false;
                }

                if (dialogResult == DialogResult.Retry)
                {
                    var registerResult = await _apiClient.RegisterAsync(loginForm.UserName, loginForm.Password);
                    if (!registerResult.Success)
                    {
                        MessageBox.Show(registerResult.ErrorMessage ?? "Sign up failed.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Account created. Please log in.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    continue;
                }

                if (dialogResult == DialogResult.OK)
                {
                    var loginResult = await _apiClient.LoginAsync(loginForm.UserName, loginForm.Password);
                    if (!loginResult.Success)
                    {
                        MessageBox.Show(loginResult.ErrorMessage ?? "Login failed.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Logs out the current user and exits the application.
        /// </summary>
        private void LogoutAndExitApplication()
        {
            _apiClient.Logout();
            Application.Exit();
        }

        /// <summary>
        /// Reloads tournaments available to the current user and refreshes tournament-related UI state.
        /// </summary>
        /// <returns>A task that completes when tournament data and UI refresh are finished.</returns>
        private async Task ReloadTournamentsAsync()
        {
            var loaded = await _apiClient.GetTournamentsAsync();
            tournaments = loaded ?? new List<Tournament>();
            currentTournament = tournaments.Count > 0 ? tournaments[0] : null;

            RefreshTournamentList();
            lblTournament.Text = currentTournament != null
                ? $"Tournament: {currentTournament.Name}"
                : "Tournament: (none)";

            UpdateStatusBar();
        }

        /// <summary>
        /// Handles the Login menu action by prompting for authentication and reloading tournaments on success.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the menu click.</param>
        private async void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (await EnsureLoggedInAsync())
            {
                await ReloadTournamentsAsync();
            }
        }

        /// <summary>
        /// Handles the Logout menu action by clearing current state, prompting for login again, and exiting if login is canceled.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the menu click.</param>
        private async void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _apiClient.Logout();

            tournaments = new List<Tournament>();
            currentTournament = null;
            RefreshTournamentList();
            lblTournament.Text = "Tournament: (none)";
            panelWorkspace.Controls.Clear();
            UpdateStatusBar();

            if (!await EnsureLoggedInAsync())
            {
                LogoutAndExitApplication();
                return;
            }

            await ReloadTournamentsAsync();
        }

        /// <summary>
        /// Handles the Exit menu action by logging out the current user and closing the application.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the menu click.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogoutAndExitApplication();
        }

        /// <summary>
        /// Refreshes the tournament list in the combo box by clearing existing items and repopulating it with the current list of tournaments.
        /// </summary>
        private void RefreshTournamentList()
        {
            cboTournament.Items.Clear();

            foreach (var tournament in tournaments)
            {
                cboTournament.Items.Add(tournament.Name);
            }

            if (currentTournament != null)
            {
                int index = tournaments.IndexOf(currentTournament);
                if (index >= 0)
                {
                    cboTournament.SelectedIndex = index;
                }
            }
        }

        /// <summary>
        /// LIstens for changes in the selected tournament from the combo box and updates the current tournament, user interface, and any relevant pages accordingly when a new tournament is selected.
        /// </summary>
        /// <param name="sender">Source of the event, typically the combo box that triggered the event when the selected index changes.</param>
        /// <param name="e">The EventArgs instance containing the event data for the SelectedIndexChanged event.</param>
        private void cboTournament_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTournament.SelectedIndex >= 0 && cboTournament.SelectedIndex < tournaments.Count)
            {
                currentTournament = tournaments[cboTournament.SelectedIndex];
                lblTournament.Text = $"Tournament: {currentTournament.Name}";
                UpdateStatusBar();

                if (panelWorkspace.Controls.Count > 0)
                {
                    if (panelWorkspace.Controls[0] is TeamsPlayersPage teamsPage)
                    {
                        teamsPage.LoadTournament(currentTournament);
                    }
                    else if (panelWorkspace.Controls[0] is BracketPage bracketPage)
                    {
                        bracketPage.LoadTournament(currentTournament);
                    }
                    else if (panelWorkspace.Controls[0] is SchedulePage schedulePage)
                    {
                        schedulePage.LoadTournament(currentTournament);
                    }
                    else if (panelWorkspace.Controls[0] is ResultsPage resultsPage)
                    {
                        resultsPage.LoadTournament(currentTournament);
                    }
                    else if (panelWorkspace.Controls[0] is TeamStatsPage teamStatsPage)
                    {
                        teamStatsPage.LoadTournament(currentTournament);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the status bar to reflect the current tournament and view being displayed.
        /// This method is called whenever there is a change in the selected tournament or the view, ensuring that the status bar always shows accurate information about the current context of the application.
        /// </summary>
        private void UpdateStatusBar()
        {
            loginToolStripMenuItem.Enabled = !_apiClient.IsLoggedIn;
            logoutToolStripMenuItem.Enabled = _apiClient.IsLoggedIn;

            var authStatus = _apiClient.IsLoggedIn
                ? $"Logged in: {_apiClient.CurrentUserName}"
                : "Not logged in";

            if (currentTournament != null)
            {
                sslHint.Text = $"{authStatus} | Current Tournament: {currentTournament.Name} | {GetCurrentViewName()}";
            }
            else
            {
                sslHint.Text = $"{authStatus} | No tournament selected";
            }
        }


        /// <summary>
        /// Retrieves the name of the current view being displayed in the workspace panel. 
        /// It checks the type of the control currently loaded in the panel and returns a corresponding name for that view. If no control is loaded, it indicates that there is no view.
        /// </summary>
        /// <returns>String representing the name of the current view, such as "Dashboard", "Teams & Players", "Bracket", or "No view" if no control is loaded.</returns>
        private string GetCurrentViewName()
        {
            if (panelWorkspace.Controls.Count > 0)
            {
                var control = panelWorkspace.Controls[0];

                if (control is DashboardPage)
                {
                    return "Dashboard";
                }
                if (control is TeamsPlayersPage)
                {
                    return "Teams & Players";
                }
                if (control is BracketPage)
                {
                    return "Bracket";
                }
                if (control is SchedulePage)
                {
                    return "Schedule";
                }
                if (control is ResultsPage)
                {
                    return "Results";
                }
                if (control is TeamStatsPage)
                {
                    return "Team Stats";
                }
                return "Unknown";
            }
            return "No view";
        }

        /// <summary>
        /// Navigates to the New Tournament form when the 'New Tournament' button is clicked, allowing the user to create a new tournament.
        /// </summary>
        /// <param name="sender">Source of the event, typically the 'New Tournament' button that was clicked.</param>
        /// <param name="e">The EventArgs instance containing the event data for the Click event.</param>
        private void btnNewTournament_Click(object sender, EventArgs e)
        {
            using var dlg = new NewTournamentForm(_apiClient);

            // Show the New Tournament form as a dialog and check if the user created a tournament.
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.CreatedTournament != null)
            {
                tournaments.Add(dlg.CreatedTournament);
                currentTournament = dlg.CreatedTournament;

                RefreshTournamentList();

                lblTournament.Text = $"Tournament: {currentTournament.Name}";
            }
        }

        /// <summary>
        /// Sets the workspace title and status bar hint to reflect the current page being displayed. 
        /// This method is called when navigating to different pages within the application to ensure that the user interface accurately reflects the current context of the application.
        /// </summary>
        /// <param name="title">The title of the page being displayed, which will be shown in the workspace title label and the status bar hint.</param>
        private void ShowPage(string title)
        {
            labelWorkspaceTitle.Text = title;
            sslHint.Text = $"Viewing: {title}";
        }

        /// <summary>
        /// Replaces the current content of the workspace panel with the specified user control.
        /// </summary>
        /// <param name="page">The user control to display in the workspace panel. This control will be docked to fill the available space.</param>
        private void LoadPage(UserControl page)
        {
            panelWorkspace.Controls.Clear();
            page.Dock = DockStyle.Fill;
            panelWorkspace.Controls.Add(page);
        }

        /// <summary>
        /// Handles the Click event of the Dashboard button to display the dashboard page and update the user interface
        /// accordingly.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Dashboard button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            var page = new DashboardPage();

            if (currentTournament != null)
            {
                page.LoadTournament(currentTournament);
            }

            LoadPage(page);
            UpdateStatusBar();
        }

        /// <summary>
        /// Goes to the Teams & Players page when the corresponding button is clicked, loading the current tournament data into the page and updating the user interface to reflect the change in view.
        /// </summary>
        /// <param name="sender">Source of the event, typically the Teams & Players button that was clicked.</param>
        /// <param name="e">The EventArgs instance containing the event data for the Click event.</param>
        private void btnTeamsPlayers_Click(object sender, EventArgs e)
        {
            var page = new TeamsPlayersPage(_apiClient);

            if (currentTournament != null)
            {
                page.LoadTournament(currentTournament);
            }

            LoadPage(page);
            UpdateStatusBar();
        }

        /// <summary>
        /// Handles the Click event of the Bracket button by displaying the Bracket page and updating the user interface
        /// to reflect the current view.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Bracket button that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnBracket_Click(object sender, EventArgs e)
        {
            var page = new BracketPage();

            if (currentTournament != null)
            {
                page.LoadTournament(currentTournament);
            }

            LoadPage(page);
            UpdateStatusBar();
        }

        /// <summary>
        /// Handles the Click event of the Schedule button and navigates to the Schedule page.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Schedule button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnSchedule_Click(object sender, EventArgs e)
        {
            var page = new SchedulePage();

            if (currentTournament != null)
            {
                page.LoadTournament(currentTournament);
            }

            LoadPage(page);
            UpdateStatusBar();
        }

        /// <summary>
        /// Handles the Click event of the Team Stats button and displays the Team Stats page.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Team stats button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnTeamStats_Click(object sender, EventArgs e)
        {
            var page = new TeamStatsPage();

            if (currentTournament != null)
            {
                page.LoadTournament(currentTournament);
            }

            LoadPage(page);
            UpdateStatusBar();
        }

        /// <summary>
        /// Handles the Click event of the Results button and displays the Results page.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Results button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnResults_Click(object sender, EventArgs e)
        {
            var page = new ResultsPage();

            if (currentTournament != null)
            {
                page.LoadTournament(currentTournament);
            }

            LoadPage(page);
            UpdateStatusBar();
        }

        /// <summary>
        /// Handles the Click event of the Add Team toolbar button by displaying a popup for adding a new team.
        /// </summary>
        /// <param name="sender">The source of the event, typically the control that triggered the event.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void tsbAddTeam_Click(object sender, EventArgs e)
        {
            if (currentTournament == null)
            {
                MessageBox.Show("Please create a tournament first.",
                    "No Tournament",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (currentTournament.Divisions.Count == 0)
            {
                MessageBox.Show("Please create a division first.",
                    "No Division",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Division selectedDivision = currentTournament.Divisions[0];

            using var addTeamForm = new addTeam(selectedDivision, currentTournament.Id, _apiClient);

            if (addTeamForm.ShowDialog(this) == DialogResult.OK &&
                addTeamForm.CreatedTeam != null)
            {
                if (panelWorkspace.Controls.Count > 0 &&
                    panelWorkspace.Controls[0] is TeamsPlayersPage teamsPage)
                {
                    teamsPage.LoadTournament(currentTournament);
                }

                MessageBox.Show($"Team '{addTeamForm.CreatedTeam.Name}' added successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Requests bracket generation through the API for the currently selected tournament.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the toolbar click.</param>
        private async void tsbGenerateBracket_Click(object sender, EventArgs e)
        {
            if (currentTournament == null)
            {
                MessageBox.Show("Please create a tournament first.",
                    "No Tournament",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool ok = await _apiClient.GenerateBracketAsync(currentTournament.Id);

            if (ok)
            {
                MessageBox.Show("Bracket generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnBracket_Click(sender, e);
            }
            else
            {
                MessageBox.Show($"Could not generate bracket in database.\nTournament ID used: {currentTournament.Id}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles the click event for the Auto Schedule command and starts the bulk scheduling flow.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void autoSchedule_Click(object sender, EventArgs e)
        {
            await RunAutoScheduleAsync();
        }

        /// <summary>
        /// Opens the auto-schedule dialog, then applies schedule times to all schedulable matches
        /// using the selected start time and increment.
        /// </summary>
        /// <returns>A task representing the asynchronous auto-schedule operation.</returns>
        private async Task RunAutoScheduleAsync()
        {
            if (currentTournament == null)
            {
                MessageBox.Show("Please create/select a tournament first.",
                    "No Tournament",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var matches = await _apiClient.GetMatchesForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.MatchReadDto>();

            var schedulableMatches = matches
                .Where(match => !string.Equals(match.Status, "Complete", StringComparison.OrdinalIgnoreCase))
                .Where(match => !string.IsNullOrWhiteSpace(match.TeamAId) && !string.IsNullOrWhiteSpace(match.TeamBId))
                .OrderBy(match => match.RoundNumber)
                .ThenBy(match => match.MatchNumber)
                .ToList();

            if (schedulableMatches.Count == 0)
            {
                MessageBox.Show("No schedulable matches were found.",
                    "Auto Schedule",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            using var dialog = new AutoScheduleDialog();
            dialog.SetInitialValues(currentTournament.StartDate, 30, "Minutes");

            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            DateTime nextStart = dialog.FirstMatchStart;
            TimeSpan increment = dialog.IncrementSpan;
            int successCount = 0;
            int failCount = 0;

            foreach (var match in schedulableMatches)
            {
                bool ok = await _apiClient.ScheduleMatchAsync(match.Id, nextStart);
                if (ok)
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                }

                nextStart = nextStart.Add(increment);
            }

            if (panelWorkspace.Controls.Count > 0 &&
                panelWorkspace.Controls[0] is SchedulePage schedulePage)
            {
                schedulePage.LoadTournament(currentTournament);
            }

            MessageBox.Show(
                $"Auto-schedule complete.\nScheduled: {successCount}\nFailed: {failCount}",
                "Auto Schedule",
                MessageBoxButtons.OK,
                failCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Handles the click event for Save actions and exports the current tournament to JSON.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void saveTournament_Click(object sender, EventArgs e)
        {
            await SaveTournamentToJsonAsync();
        }

        /// <summary>
        /// Handles the click event for Open actions and imports a tournament from a JSON file.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void openTournament_Click(object sender, EventArgs e)
        {
            await OpenTournamentFromJsonAsync();
        }

        /// <summary>
        /// Exports the currently selected tournament, teams, players, and matches to a JSON file.
        /// </summary>
        /// <returns>A task representing the asynchronous export operation.</returns>
        private async Task SaveTournamentToJsonAsync()
        {
            if (currentTournament == null)
            {
                MessageBox.Show("Please select a tournament first.",
                    "Save Tournament",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var teams = await _apiClient.GetTeamsForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.TeamReadDto>();

            var matches = await _apiClient.GetMatchesForTournamentAsync(currentTournament.Id)
                ?? new List<ApiClient.MatchReadDto>();

            var export = new TournamentExportFile
            {
                SchemaVersion = 1,
                ExportedAtUtc = DateTime.UtcNow,
                Tournament = new TournamentExportDto
                {
                    Name = currentTournament.Name,
                    Location = currentTournament.Location,
                    StartDateUtc = currentTournament.StartDate,
                    BracketType = currentTournament.BracketType.ToString()
                },
                Teams = teams.Select(team => new TeamExportDto
                {
                    Name = team.Name,
                    Seed = team.Seed,
                    Players = team.Players
                        .Select(player => new PlayerExportDto
                        {
                            DisplayName = player.DisplayName,
                            Number = player.Number
                        })
                        .ToList()
                }).ToList(),
                Matches = matches.Select(match => new MatchExportDto
                {
                    RoundNumber = match.RoundNumber,
                    MatchNumber = match.MatchNumber,
                    TeamAName = match.TeamAName,
                    TeamBName = match.TeamBName,
                    ScheduledStartUtc = match.ScheduledStart,
                    Status = match.Status,
                    ScoreA = match.ScoreA,
                    ScoreB = match.ScoreB
                }).ToList()
            };

            using var saveDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"{currentTournament.Name.Replace(' ', '_')}.json",
                Title = "Export Tournament to JSON"
            };

            if (saveDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = System.Text.Json.JsonSerializer.Serialize(export, jsonOptions);
            await System.IO.File.WriteAllTextAsync(saveDialog.FileName, json);

            MessageBox.Show($"Tournament exported to:\n{saveDialog.FileName}",
                "Export Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Imports tournament data from a JSON file, creates a new tournament owned by the current user,
        /// and attempts to restore teams, schedule, and recorded results.
        /// </summary>
        /// <returns>A task representing the asynchronous import operation.</returns>
        private async Task OpenTournamentFromJsonAsync()
        {
            using var openDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Import Tournament from JSON"
            };

            if (openDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            using var loadingDialog = CreateLoadingDialog("Importing tournament data...\nThis " +
                "may take a few seconds.");

            loadingDialog.Location = new Point(
                this.Left + (this.Width - loadingDialog.Width) / 2,
                this.Top + (this.Height - loadingDialog.Height) / 2);

            loadingDialog.Show(this);
            loadingDialog.BringToFront();
            loadingDialog.Update();
            await Task.Yield();

            try
            {
                TournamentExportFile? importFile;
                try
                {
                    string json = await System.IO.File.ReadAllTextAsync(openDialog.FileName);
                    importFile = System.Text.Json.JsonSerializer.Deserialize<TournamentExportFile>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not read JSON file.\n{ex.Message}",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (importFile == null || importFile.Tournament == null)
                {
                    MessageBox.Show("Invalid import file.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                string importedName = $"{importFile.Tournament.Name} (Imported)";
                var importedTournament = new Tournament(
                    importedName,
                    importFile.Tournament.StartDateUtc,
                    importFile.Tournament.Location ?? string.Empty);

                if (Enum.TryParse<BracketType>(importFile.Tournament.BracketType, out var parsedBracketType))
                {
                    importedTournament.BracketType = parsedBracketType;
                }

                bool createdTournament = await _apiClient.CreateTournamentAsync(importedTournament);
                if (!createdTournament)
                {
                    MessageBox.Show("Failed to create imported tournament.",
                        "Import Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                int teamFailures = 0;
                foreach (var teamDto in importFile.Teams.OrderBy(t => t.Seed).ThenBy(t => t.Name))
                {
                    var team = new Team(teamDto.Name)
                    {
                        Seed = teamDto.Seed
                    };

                    foreach (var playerDto in teamDto.Players)
                    {
                        team.AddPlayer(new Player(playerDto.DisplayName, playerDto.Number));
                    }

                    bool createdTeam = await _apiClient.CreateTeamForTournamentAsync(importedTournament.Id, team);
                    if (!createdTeam)
                    {
                        teamFailures++;
                    }
                }

                int scheduleFailures = 0;
                int resultFailures = 0;

                if (importFile.Teams.Count >= 2)
                {
                    bool generatedBracket = await _apiClient.GenerateBracketAsync(importedTournament.Id);
                    if (generatedBracket)
                    {
                        var importedMatches = importFile.Matches
                            .OrderBy(m => m.RoundNumber)
                            .ThenBy(m => m.MatchNumber)
                            .ToList();

                        bool isSwiss = importedTournament.BracketType == BracketType.Swiss;
                        int maxRound = importedMatches.Count == 0 ? 0 : importedMatches.Max(m => m.RoundNumber);

                        for (int round = 1; round <= maxRound; round++)
                        {
                            var sourceRoundMatches = importedMatches
                                .Where(m => m.RoundNumber == round)
                                .OrderBy(m => m.MatchNumber)
                                .ToList();

                            var targetMatches = await _apiClient.GetMatchesForTournamentAsync(importedTournament.Id)
                                ?? new List<ApiClient.MatchReadDto>();

                            var targetByMatchNumber = targetMatches
                                .Where(m => m.RoundNumber == round)
                                .ToDictionary(m => m.MatchNumber, m => m);

                            foreach (var sourceMatch in sourceRoundMatches)
                            {
                                if (!sourceMatch.ScheduledStartUtc.HasValue)
                                {
                                    continue;
                                }

                                if (!targetByMatchNumber.TryGetValue(sourceMatch.MatchNumber, out var targetMatch))
                                {
                                    scheduleFailures++;
                                    continue;
                                }

                                bool scheduled = await _apiClient.ScheduleMatchAsync(targetMatch.Id, sourceMatch.ScheduledStartUtc.Value);
                                if (!scheduled)
                                {
                                    scheduleFailures++;
                                }
                            }

                            foreach (var sourceMatch in sourceRoundMatches)
                            {
                                if (!sourceMatch.ScoreA.HasValue || !sourceMatch.ScoreB.HasValue)
                                {
                                    continue;
                                }

                                if (!targetByMatchNumber.TryGetValue(sourceMatch.MatchNumber, out var targetMatch))
                                {
                                    resultFailures++;
                                    continue;
                                }

                                if (string.IsNullOrWhiteSpace(targetMatch.TeamAId) || string.IsNullOrWhiteSpace(targetMatch.TeamBId))
                                {
                                    continue;
                                }

                                bool recorded = await _apiClient.RecordMatchResultAsync(
                                    targetMatch.Id,
                                    sourceMatch.ScoreA.Value,
                                    sourceMatch.ScoreB.Value);

                                if (!recorded)
                                {
                                    resultFailures++;
                                }
                            }

                            if (isSwiss && round < maxRound)
                            {
                                bool nextRoundOk = await _apiClient.GenerateNextRoundAsync(importedTournament.Id);
                                if (!nextRoundOk)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                await ReloadTournamentsAsync();

                int importedIndex = tournaments.FindIndex(t => t.Id == importedTournament.Id);
                if (importedIndex >= 0)
                {
                    cboTournament.SelectedIndex = importedIndex;
                }

                MessageBox.Show(
                    $"Import complete.\n" +
                    $"Tournament: {importedTournament.Name}\n" +
                    $"Team failures: {teamFailures}\n" +
                    $"Schedule failures: {scheduleFailures}\n" +
                    $"Result failures: {resultFailures}",
                    "Import Complete",
                    MessageBoxButtons.OK,
                    (teamFailures + scheduleFailures + resultFailures) == 0
                        ? MessageBoxIcon.Information
                        : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Import failed unexpectedly.\n{ex.Message}",
                    "Import Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                loadingDialog.Close();
            }
        }

        /// <summary>
        /// Creates a small non-interactive loading dialog displayed during long-running import operations.
        /// </summary>
        /// <param name="message">Message text displayed in the dialog.</param>
        /// <returns>A configured <see cref="Form"/> instance used as a loading indicator.</returns>
        private Form CreateLoadingDialog(string message)
        {
            var dialog = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.Manual,
                ControlBox = false,
                ShowInTaskbar = false,
                TopMost = true,
                Width = 600,
                Height = 250,
                Text = "Please wait..."
            };

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(16)
            };

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // message area
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F)); // spacer
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F)); // progress bar

            var label = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Text = message
            };

            var progressBar = new ProgressBar
            {
                Dock = DockStyle.Fill,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            table.Controls.Add(label, 0, 0);
            table.Controls.Add(progressBar, 0, 2);
            dialog.Controls.Add(table);

            return dialog;
        }

        /// <summary>
        /// Represents the root JSON export/import document for a tournament snapshot.
        /// </summary>
        private sealed class TournamentExportFile
        {
            /// <summary>
            /// Gets or sets the export schema version for compatibility checks.
            /// </summary>
            public int SchemaVersion { get; set; }

            /// <summary>
            /// Gets or sets the UTC timestamp when export was generated.
            /// </summary>
            public DateTime ExportedAtUtc { get; set; }

            /// <summary>
            /// Gets or sets tournament-level metadata.
            /// </summary>
            public TournamentExportDto Tournament { get; set; } = new TournamentExportDto();

            /// <summary>
            /// Gets or sets exported teams including nested player data.
            /// </summary>
            public List<TeamExportDto> Teams { get; set; } = new List<TeamExportDto>();

            /// <summary>
            /// Gets or sets exported match rows including schedule and scores.
            /// </summary>
            public List<MatchExportDto> Matches { get; set; } = new List<MatchExportDto>();
        }

        /// <summary>
        /// Represents tournament-level fields included in the JSON snapshot.
        /// </summary>
        private sealed class TournamentExportDto
        {
            /// <summary>
            /// Gets or sets the tournament name.
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the tournament location, if available.
            /// </summary>
            public string? Location { get; set; }

            /// <summary>
            /// Gets or sets the tournament start date and time in UTC.
            /// </summary>
            public DateTime StartDateUtc { get; set; }

            /// <summary>
            /// Gets or sets the bracket type name.
            /// </summary>
            public string BracketType { get; set; } = string.Empty;
        }

        /// <summary>
        /// Represents a team entry in the JSON snapshot.
        /// </summary>
        private sealed class TeamExportDto
        {
            /// <summary>
            /// Gets or sets the team name.
            /// </summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the team seed.
            /// </summary>
            public int Seed { get; set; }

            /// <summary>
            /// Gets or sets players associated with the team.
            /// </summary>
            public List<PlayerExportDto> Players { get; set; } = new List<PlayerExportDto>();
        }

        /// <summary>
        /// Represents a player entry in the JSON snapshot.
        /// </summary>
        private sealed class PlayerExportDto
        {
            /// <summary>
            /// Gets or sets the player display name.
            /// </summary>
            public string DisplayName { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the player number.
            /// </summary>
            public int Number { get; set; }
        }

        /// <summary>
        /// Represents a match entry in the JSON snapshot.
        /// </summary>
        private sealed class MatchExportDto
        {
            /// <summary>
            /// Gets or sets the match round number.
            /// </summary>
            public int RoundNumber { get; set; }

            /// <summary>
            /// Gets or sets the match number within the round.
            /// </summary>
            public int MatchNumber { get; set; }

            /// <summary>
            /// Gets or sets Team A display name.
            /// </summary>
            public string? TeamAName { get; set; }

            /// <summary>
            /// Gets or sets Team B display name.
            /// </summary>
            public string? TeamBName { get; set; }

            /// <summary>
            /// Gets or sets scheduled match start in UTC, when available.
            /// </summary>
            public DateTime? ScheduledStartUtc { get; set; }

            /// <summary>
            /// Gets or sets the stored match status value.
            /// </summary>
            public string Status { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets Team A score, when recorded.
            /// </summary>
            public int? ScoreA { get; set; }

            /// <summary>
            /// Gets or sets Team B score, when recorded.
            /// </summary>
            public int? ScoreB { get; set; }
        }
    }
}