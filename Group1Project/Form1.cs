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
        public Form1()
        {
            InitializeComponent();
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
            LoadPage(new DashboardPage());
            sslHint.Text = "Viewing: Dashboard";
        }

        private void btnTeamsPlayers_Click(object sender, EventArgs e)
        {
            var page = new TeamsPlayersPage();

            if (currentTournament != null)
            {
                page.LoadTournament(currentTournament);
            }

            LoadPage(page);
            sslHint.Text = "Viewing: Teams & Players";
        }

        private void btnBracket_Click(object sender, EventArgs e)
        {
            LoadPage(new BracketPage());
            sslHint.Text = "Viewing: Bracket";
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
        private void btnNewTournament_Click(object sender, EventArgs e)
        {
            using var dlg = new NewTournamentForm();

            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.CreatedTournament != null)
            {
                currentTournament = dlg.CreatedTournament;

                lblTournament.Text = $"Tournament: {currentTournament.Name}";
            }
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
    }
}
