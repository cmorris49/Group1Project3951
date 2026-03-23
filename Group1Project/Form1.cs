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
        /// Loads tournaments from the API when the main form finishes loading and initializes the current selection.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for form load.</param>
        private async void Form1_Load(object? sender, EventArgs e)
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
                }
            }
        }

        /// <summary>
        /// Updates the status bar to reflect the current tournament and view being displayed.
        /// This method is called whenever there is a change in the selected tournament or the view, ensuring that the status bar always shows accurate information about the current context of the application.
        /// </summary>
        private void UpdateStatusBar()
        {
            if (currentTournament != null)
            {
                sslHint.Text = $"Current Tournament: {currentTournament.Name} | {GetCurrentViewName()}";
            }
            else
            {
                sslHint.Text = "No tournament selected";
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
            ShowPage("Schedule");
        }

        /// <summary>
        /// Handles the Click event of the Standings button and displays the Standings page.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Standings button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnStandings_Click(object sender, EventArgs e)
        {
            ShowPage("Standings");
        }

        /// <summary>
        /// Handles the Click event of the Results button and displays the Results page.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Results button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void btnResults_Click(object sender, EventArgs e)
        {
            ShowPage("Results");
        }

        /// <summary>
        /// Handles the Click event of the 'New Tournament' menu item to initiate the process of creating a new
        /// tournament.
        /// </summary>
        /// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void newTournamentToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                MessageBox.Show("Please create a tournament first.", "No Tournament", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Could not generate bracket in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}