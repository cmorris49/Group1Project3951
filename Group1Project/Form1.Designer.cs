/// <summary>
/// Group 1 Project - Main Form designer
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuMain = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newTournamentToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            dashboardToolStripMenuItem = new ToolStripMenuItem();
            teamsPlayersToolStripMenuItem = new ToolStripMenuItem();
            bracketsToolStripMenuItem = new ToolStripMenuItem();
            scheduleToolStripMenuItem = new ToolStripMenuItem();
            standingsToolStripMenuItem = new ToolStripMenuItem();
            resultsToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            generateBracketToolStripMenuItem = new ToolStripMenuItem();
            autoScheduleToolStripMenuItem = new ToolStripMenuItem();
            recalculateStandingsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolMain = new ToolStrip();
            tsbSave = new ToolStripButton();
            tsbAddTeam = new ToolStripButton();
            tsbGenerateBracket = new ToolStripButton();
            tsbAutoSchedule = new ToolStripButton();
            tsbEnterResult = new ToolStripButton();
            statusMain = new StatusStrip();
            sslTournament = new ToolStripStatusLabel();
            sslHint = new ToolStripStatusLabel();
            splitMain = new SplitContainer();
            panelNav = new Panel();
            btnResults = new Button();
            btnStandings = new Button();
            btnSchedule = new Button();
            btnBracket = new Button();
            btnTeamsPlayers = new Button();
            btnDashboard = new Button();
            grpTournament = new GroupBox();
            btnSaveTournament = new Button();
            btnOpenTournament = new Button();
            btnNewTournament = new Button();
            cboTournament = new ComboBox();
            lblTournament = new Label();
            panelWorkspace = new Panel();
            labelWorkspaceTitle = new Label();
            menuMain.SuspendLayout();
            toolMain.SuspendLayout();
            statusMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            panelNav.SuspendLayout();
            grpTournament.SuspendLayout();
            panelWorkspace.SuspendLayout();
            SuspendLayout();
            // 
            // menuMain
            // 
            menuMain.ImageScalingSize = new Size(32, 32);
            menuMain.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, newToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Padding = new Padding(3, 1, 0, 1);
            menuMain.Size = new Size(828, 24);
            menuMain.TabIndex = 1;
            menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newTournamentToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(39, 22);
            fileToolStripMenuItem.Text = "File";
            // 
            // newTournamentToolStripMenuItem
            // 
            newTournamentToolStripMenuItem.Name = "newTournamentToolStripMenuItem";
            newTournamentToolStripMenuItem.Size = new Size(181, 24);
            newTournamentToolStripMenuItem.Text = "New Tournament";
            newTournamentToolStripMenuItem.Click += btnNewTournament_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(181, 24);
            openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(181, 24);
            saveToolStripMenuItem.Text = "Save";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(181, 24);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dashboardToolStripMenuItem, teamsPlayersToolStripMenuItem, bracketsToolStripMenuItem, scheduleToolStripMenuItem, standingsToolStripMenuItem, resultsToolStripMenuItem });
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(50, 22);
            newToolStripMenuItem.Text = "New ";
            // 
            // dashboardToolStripMenuItem
            // 
            dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            dashboardToolStripMenuItem.Size = new Size(168, 24);
            dashboardToolStripMenuItem.Text = "Dashboard";
            // 
            // teamsPlayersToolStripMenuItem
            // 
            teamsPlayersToolStripMenuItem.Name = "teamsPlayersToolStripMenuItem";
            teamsPlayersToolStripMenuItem.Size = new Size(168, 24);
            teamsPlayersToolStripMenuItem.Text = "Teams & Players";
            // 
            // bracketsToolStripMenuItem
            // 
            bracketsToolStripMenuItem.Name = "bracketsToolStripMenuItem";
            bracketsToolStripMenuItem.Size = new Size(168, 24);
            bracketsToolStripMenuItem.Text = "Bracket";
            // 
            // scheduleToolStripMenuItem
            // 
            scheduleToolStripMenuItem.Name = "scheduleToolStripMenuItem";
            scheduleToolStripMenuItem.Size = new Size(168, 24);
            scheduleToolStripMenuItem.Text = "Schedule";
            // 
            // standingsToolStripMenuItem
            // 
            standingsToolStripMenuItem.Name = "standingsToolStripMenuItem";
            standingsToolStripMenuItem.Size = new Size(168, 24);
            standingsToolStripMenuItem.Text = "Standings";
            // 
            // resultsToolStripMenuItem
            // 
            resultsToolStripMenuItem.Name = "resultsToolStripMenuItem";
            resultsToolStripMenuItem.Size = new Size(168, 24);
            resultsToolStripMenuItem.Text = "Results";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { generateBracketToolStripMenuItem, autoScheduleToolStripMenuItem, recalculateStandingsToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(51, 22);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // generateBracketToolStripMenuItem
            // 
            generateBracketToolStripMenuItem.Name = "generateBracketToolStripMenuItem";
            generateBracketToolStripMenuItem.Size = new Size(208, 24);
            generateBracketToolStripMenuItem.Text = "Generate Bracket";
            // 
            // autoScheduleToolStripMenuItem
            // 
            autoScheduleToolStripMenuItem.Name = "autoScheduleToolStripMenuItem";
            autoScheduleToolStripMenuItem.Size = new Size(208, 24);
            autoScheduleToolStripMenuItem.Text = "Auto Schedule";
            // 
            // recalculateStandingsToolStripMenuItem
            // 
            recalculateStandingsToolStripMenuItem.Name = "recalculateStandingsToolStripMenuItem";
            recalculateStandingsToolStripMenuItem.Size = new Size(208, 24);
            recalculateStandingsToolStripMenuItem.Text = "Recalculate Standings";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(47, 22);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(117, 24);
            aboutToolStripMenuItem.Text = "About";
            // 
            // toolMain
            // 
            toolMain.GripStyle = ToolStripGripStyle.Hidden;
            toolMain.ImageScalingSize = new Size(32, 32);
            toolMain.Items.AddRange(new ToolStripItem[] { tsbSave, tsbAddTeam, tsbGenerateBracket, tsbAutoSchedule, tsbEnterResult });
            toolMain.Location = new Point(0, 24);
            toolMain.Name = "toolMain";
            toolMain.Padding = new Padding(0, 0, 2, 0);
            toolMain.Size = new Size(828, 39);
            toolMain.TabIndex = 2;
            toolMain.Text = "toolStrip1";
            // 
            // tsbSave
            // 
            tsbSave.Image = Properties.Resources.save;
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(71, 36);
            tsbSave.Text = "Save";
            // 
            // tsbAddTeam
            // 
            tsbAddTeam.Image = Properties.Resources.add_group;
            tsbAddTeam.ImageTransparentColor = Color.Magenta;
            tsbAddTeam.Name = "tsbAddTeam";
            tsbAddTeam.Size = new Size(103, 36);
            tsbAddTeam.Text = "Add Team";
            tsbAddTeam.Click += tsbAddTeam_Click;
            // 
            // tsbGenerateBracket
            // 
            tsbGenerateBracket.Image = Properties.Resources.New_bracket;
            tsbGenerateBracket.ImageTransparentColor = Color.Magenta;
            tsbGenerateBracket.Name = "tsbGenerateBracket";
            tsbGenerateBracket.Size = new Size(143, 36);
            tsbGenerateBracket.Text = "Generate Bracket";
            tsbGenerateBracket.Click += tsbGenerateBracket_Click;
            // 
            // tsbAutoSchedule
            // 
            tsbAutoSchedule.Image = Properties.Resources.schedule;
            tsbAutoSchedule.ImageTransparentColor = Color.Magenta;
            tsbAutoSchedule.Name = "tsbAutoSchedule";
            tsbAutoSchedule.Size = new Size(127, 36);
            tsbAutoSchedule.Text = "Auto Schedule";
            // 
            // tsbEnterResult
            // 
            tsbEnterResult.Image = Properties.Resources.enter;
            tsbEnterResult.ImageTransparentColor = Color.Magenta;
            tsbEnterResult.Name = "tsbEnterResult";
            tsbEnterResult.Size = new Size(113, 36);
            tsbEnterResult.Text = "Enter Result";
            // 
            // statusMain
            // 
            statusMain.ImageScalingSize = new Size(32, 32);
            statusMain.Items.AddRange(new ToolStripItem[] { sslTournament, sslHint });
            statusMain.Location = new Point(0, 541);
            statusMain.Name = "statusMain";
            statusMain.Padding = new Padding(1, 0, 8, 0);
            statusMain.Size = new Size(828, 22);
            statusMain.TabIndex = 3;
            statusMain.Text = "statusStrip1";
            // 
            // sslTournament
            // 
            sslTournament.Name = "sslTournament";
            sslTournament.Size = new Size(121, 17);
            sslTournament.Text = "Tournament: (none)";
            // 
            // sslHint
            // 
            sslHint.Name = "sslHint";
            sslHint.Size = new Size(44, 17);
            sslHint.Text = "Ready";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.FixedPanel = FixedPanel.Panel1;
            splitMain.Location = new Point(0, 63);
            splitMain.Margin = new Padding(2);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(panelNav);
            splitMain.Panel1.Controls.Add(grpTournament);
            splitMain.Panel1MinSize = 260;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(panelWorkspace);
            splitMain.Size = new Size(828, 478);
            splitMain.SplitterDistance = 260;
            splitMain.TabIndex = 4;
            // 
            // panelNav
            // 
            panelNav.Controls.Add(btnResults);
            panelNav.Controls.Add(btnStandings);
            panelNav.Controls.Add(btnSchedule);
            panelNav.Controls.Add(btnBracket);
            panelNav.Controls.Add(btnTeamsPlayers);
            panelNav.Controls.Add(btnDashboard);
            panelNav.Dock = DockStyle.Fill;
            panelNav.Location = new Point(0, 81);
            panelNav.Margin = new Padding(2);
            panelNav.Name = "panelNav";
            panelNav.Padding = new Padding(4);
            panelNav.Size = new Size(260, 397);
            panelNav.TabIndex = 1;
            // 
            // btnResults
            // 
            btnResults.Dock = DockStyle.Top;
            btnResults.Location = new Point(4, 124);
            btnResults.Margin = new Padding(2);
            btnResults.Name = "btnResults";
            btnResults.Size = new Size(252, 24);
            btnResults.TabIndex = 5;
            btnResults.Text = "Results";
            btnResults.UseVisualStyleBackColor = true;
            btnResults.Click += btnResults_Click;
            // 
            // btnStandings
            // 
            btnStandings.Dock = DockStyle.Top;
            btnStandings.Location = new Point(4, 100);
            btnStandings.Margin = new Padding(2);
            btnStandings.Name = "btnStandings";
            btnStandings.Size = new Size(252, 24);
            btnStandings.TabIndex = 4;
            btnStandings.Text = "Standings";
            btnStandings.UseVisualStyleBackColor = true;
            btnStandings.Click += btnStandings_Click;
            // 
            // btnSchedule
            // 
            btnSchedule.Dock = DockStyle.Top;
            btnSchedule.Location = new Point(4, 76);
            btnSchedule.Margin = new Padding(2);
            btnSchedule.Name = "btnSchedule";
            btnSchedule.Size = new Size(252, 24);
            btnSchedule.TabIndex = 3;
            btnSchedule.Text = "Schedule";
            btnSchedule.UseVisualStyleBackColor = true;
            btnSchedule.Click += btnSchedule_Click;
            // 
            // btnBracket
            // 
            btnBracket.Dock = DockStyle.Top;
            btnBracket.Location = new Point(4, 52);
            btnBracket.Margin = new Padding(2);
            btnBracket.Name = "btnBracket";
            btnBracket.Size = new Size(252, 24);
            btnBracket.TabIndex = 2;
            btnBracket.Text = "Bracket";
            btnBracket.UseVisualStyleBackColor = true;
            btnBracket.Click += btnBracket_Click;
            // 
            // btnTeamsPlayers
            // 
            btnTeamsPlayers.Dock = DockStyle.Top;
            btnTeamsPlayers.Location = new Point(4, 28);
            btnTeamsPlayers.Margin = new Padding(2);
            btnTeamsPlayers.Name = "btnTeamsPlayers";
            btnTeamsPlayers.Size = new Size(252, 24);
            btnTeamsPlayers.TabIndex = 1;
            btnTeamsPlayers.Text = "Teams & Players";
            btnTeamsPlayers.UseVisualStyleBackColor = true;
            btnTeamsPlayers.Click += btnTeamsPlayers_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.Location = new Point(4, 4);
            btnDashboard.Margin = new Padding(2);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(252, 24);
            btnDashboard.TabIndex = 0;
            btnDashboard.Text = "Dashboard";
            btnDashboard.UseVisualStyleBackColor = true;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // grpTournament
            // 
            grpTournament.Controls.Add(btnSaveTournament);
            grpTournament.Controls.Add(btnOpenTournament);
            grpTournament.Controls.Add(btnNewTournament);
            grpTournament.Controls.Add(cboTournament);
            grpTournament.Controls.Add(lblTournament);
            grpTournament.Dock = DockStyle.Top;
            grpTournament.Location = new Point(0, 0);
            grpTournament.Margin = new Padding(2);
            grpTournament.Name = "grpTournament";
            grpTournament.Padding = new Padding(2);
            grpTournament.Size = new Size(260, 81);
            grpTournament.TabIndex = 0;
            grpTournament.TabStop = false;
            grpTournament.Text = "Current Tournament";
            // 
            // btnSaveTournament
            // 
            btnSaveTournament.Location = new Point(103, 55);
            btnSaveTournament.Margin = new Padding(2);
            btnSaveTournament.Name = "btnSaveTournament";
            btnSaveTournament.Size = new Size(46, 24);
            btnSaveTournament.TabIndex = 4;
            btnSaveTournament.Text = "Save";
            btnSaveTournament.UseVisualStyleBackColor = true;
            // 
            // btnOpenTournament
            // 
            btnOpenTournament.Location = new Point(55, 55);
            btnOpenTournament.Margin = new Padding(2);
            btnOpenTournament.Name = "btnOpenTournament";
            btnOpenTournament.Size = new Size(46, 24);
            btnOpenTournament.TabIndex = 3;
            btnOpenTournament.Text = "Open";
            btnOpenTournament.UseVisualStyleBackColor = true;
            // 
            // btnNewTournament
            // 
            btnNewTournament.Location = new Point(6, 55);
            btnNewTournament.Margin = new Padding(2);
            btnNewTournament.Name = "btnNewTournament";
            btnNewTournament.Size = new Size(46, 24);
            btnNewTournament.TabIndex = 2;
            btnNewTournament.Text = "New";
            btnNewTournament.UseVisualStyleBackColor = true;
            btnNewTournament.Click += btnNewTournament_Click;
            // 
            // cboTournament
            // 
            cboTournament.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cboTournament.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTournament.FormattingEnabled = true;
            cboTournament.Location = new Point(6, 31);
            cboTournament.Margin = new Padding(2);
            cboTournament.Name = "cboTournament";
            cboTournament.Size = new Size(245, 25);
            cboTournament.TabIndex = 1;
            cboTournament.SelectedIndexChanged += cboTournament_SelectedIndexChanged;
            // 
            // lblTournament
            // 
            lblTournament.AutoSize = true;
            lblTournament.Location = new Point(6, 15);
            lblTournament.Margin = new Padding(2, 0, 2, 0);
            lblTournament.Name = "lblTournament";
            lblTournament.Size = new Size(80, 17);
            lblTournament.TabIndex = 0;
            lblTournament.Text = "Tournament:";
            // 
            // panelWorkspace
            // 
            panelWorkspace.Controls.Add(labelWorkspaceTitle);
            panelWorkspace.Dock = DockStyle.Fill;
            panelWorkspace.Location = new Point(0, 0);
            panelWorkspace.Margin = new Padding(2);
            panelWorkspace.Name = "panelWorkspace";
            panelWorkspace.Size = new Size(564, 478);
            panelWorkspace.TabIndex = 0;
            // 
            // labelWorkspaceTitle
            // 
            labelWorkspaceTitle.AutoSize = true;
            labelWorkspaceTitle.Font = new Font("Segoe UI", 16F);
            labelWorkspaceTitle.Location = new Point(9, 8);
            labelWorkspaceTitle.Margin = new Padding(2, 0, 2, 0);
            labelWorkspaceTitle.Name = "labelWorkspaceTitle";
            labelWorkspaceTitle.Size = new Size(129, 32);
            labelWorkspaceTitle.TabIndex = 0;
            labelWorkspaceTitle.Text = "Dashboard";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 563);
            Controls.Add(splitMain);
            Controls.Add(statusMain);
            Controls.Add(toolMain);
            Controls.Add(menuMain);
            Margin = new Padding(2);
            MinimumSize = new Size(595, 377);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Tournament Bracket Manager";
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            toolMain.ResumeLayout(false);
            toolMain.PerformLayout();
            statusMain.ResumeLayout(false);
            statusMain.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            panelNav.ResumeLayout(false);
            grpTournament.ResumeLayout(false);
            grpTournament.PerformLayout();
            panelWorkspace.ResumeLayout(false);
            panelWorkspace.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuMain;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newTournamentToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem dashboardToolStripMenuItem;
        private ToolStripMenuItem teamsPlayersToolStripMenuItem;
        private ToolStripMenuItem bracketsToolStripMenuItem;
        private ToolStripMenuItem scheduleToolStripMenuItem;
        private ToolStripMenuItem standingsToolStripMenuItem;
        private ToolStripMenuItem resultsToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem generateBracketToolStripMenuItem;
        private ToolStripMenuItem autoScheduleToolStripMenuItem;
        private ToolStripMenuItem recalculateStandingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip toolMain;
        private ToolStripButton tsbSave;
        private ToolStripButton tsbAddTeam;
        private ToolStripButton tsbGenerateBracket;
        private ToolStripButton tsbAutoSchedule;
        private ToolStripButton tsbEnterResult;
        private StatusStrip statusMain;
        private ToolStripStatusLabel sslTournament;
        private ToolStripStatusLabel sslHint;
        private SplitContainer splitMain;
        private GroupBox grpTournament;
        private Button btnNewTournament;
        private ComboBox cboTournament;
        private Label lblTournament;
        private Panel panelNav;
        private Button btnSaveTournament;
        private Button btnOpenTournament;
        private Button btnResults;
        private Button btnStandings;
        private Button btnSchedule;
        private Button btnBracket;
        private Button btnTeamsPlayers;
        private Button btnDashboard;
        private Panel panelWorkspace;
        private Label labelWorkspaceTitle;
    }
}
