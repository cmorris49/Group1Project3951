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

            // For testing purposes, we can create a sample tournament with dummy data to populate the UI.
            //var testTournament = Tournament.CreateTestTournament();
            //tournaments.Add(testTournament);
            //currentTournament = testTournament;
            // End of testing code - remove when done

            this.Load += Form1_Load;
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
            using var dlg = new NewTournamentForm();

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
            var page = new TeamsPlayersPage();

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

            using var addTeamForm = new addTeam(selectedDivision, currentTournament.Id);

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
    }
}