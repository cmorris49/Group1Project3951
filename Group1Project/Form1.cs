using Group1Project;
using System.Net.NetworkInformation;

/// <summary>
/// Group 1 Project - Main Form
/// Author: Cameron
///         Jun
///         Jonathan 
/// Date: March 2, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    public partial class Form1 : Form
    {
        private Tournament? currentTournament;
        private List<Tournament> tournaments = new List<Tournament>();

        public Form1()
        {
            InitializeComponent();

            var testTournament = Tournament.CreateTestTournament();
            tournaments.Add(testTournament);

            currentTournament = testTournament;

            RefreshTournamentList();

            lblTournament.Text = $"Tournament: {currentTournament.Name}";
        }

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

        private string GetCurrentViewName()
        {
            if (panelWorkspace.Controls.Count > 0)
            {
                var control = panelWorkspace.Controls[0];
                if (control is DashboardPage) return "Dashboard";
                if (control is TeamsPlayersPage) return "Teams & Players";
                if (control is BracketPage) return "Bracket";
                return "Unknown";
            }
            return "No view";
        }

        private void btnNewTournament_Click(object sender, EventArgs e)
        {
            using var dlg = new NewTournamentForm();

            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.CreatedTournament != null)
            {
                tournaments.Add(dlg.CreatedTournament);
                currentTournament = dlg.CreatedTournament;

                RefreshTournamentList();

                lblTournament.Text = $"Tournament: {currentTournament.Name}";
            }
        }

        private void ShowPage(string title)
        {
            labelWorkspaceTitle.Text = title;
            sslHint.Text = $"Viewing: {title}";
        }

        private void LoadPage(UserControl page)
        {
            panelWorkspace.Controls.Clear();
            page.Dock = DockStyle.Fill;
            panelWorkspace.Controls.Add(page);
        }

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

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            ShowPage("Schedule");
        }

        private void btnStandings_Click(object sender, EventArgs e)
        {
            ShowPage("Standings");
        }

        private void btnResults_Click(object sender, EventArgs e)
        {
            ShowPage("Results");
        }

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

            using var addTeamForm = new addTeam(selectedDivision);

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

        private void tsbGenerateBracket_Click(object sender, EventArgs e)
        {
            if (currentTournament == null)
            {
                MessageBox.Show("Please create a tournament first.", "No Tournament", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                currentTournament.GenerateBracket();
                MessageBox.Show("Bracket generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnBracket_Click(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cannot Generate Bracket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}