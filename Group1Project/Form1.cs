using System.Net.NetworkInformation;

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
            LoadPage(new TeamsPlayersPage());
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
    }
}
