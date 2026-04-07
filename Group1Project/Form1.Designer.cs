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
            teamStatsToolStripMenuItem = new ToolStripMenuItem();
            resultsToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            generateBracketToolStripMenuItem = new ToolStripMenuItem();
            autoScheduleToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            accounToolStripMenuItem = new ToolStripMenuItem();
            loginToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            toolMain = new ToolStrip();
            tsbSave = new ToolStripButton();
            tsbAddTeam = new ToolStripButton();
            tsbGenerateBracket = new ToolStripButton();
            tsbAutoSchedule = new ToolStripButton();
            tsbEnterResult = new ToolStripButton();
            statusMain = new StatusStrip();
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
            lblTournament = new Label();
            btnSaveTournament = new Button();
            btnOpenTournament = new Button();
            btnNewTournament = new Button();
            cboTournament = new ComboBox();
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
            menuMain.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, newToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem, accounToolStripMenuItem });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Padding = new Padding(11, 4, 0, 4);
            menuMain.Size = new Size(1655, 44);
            menuMain.TabIndex = 1;
            menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newTournamentToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(71, 36);
            fileToolStripMenuItem.Text = "File";
            // 
            // newTournamentToolStripMenuItem
            // 
            newTournamentToolStripMenuItem.Name = "newTournamentToolStripMenuItem";
            newTournamentToolStripMenuItem.Size = new Size(359, 44);
            newTournamentToolStripMenuItem.Text = "New Tournament";
            newTournamentToolStripMenuItem.Click += btnNewTournament_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(359, 44);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openTournament_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(359, 44);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveTournament_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(359, 44);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dashboardToolStripMenuItem, teamsPlayersToolStripMenuItem, bracketsToolStripMenuItem, scheduleToolStripMenuItem, teamStatsToolStripMenuItem, resultsToolStripMenuItem });
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(85, 36);
            newToolStripMenuItem.Text = "View";
            // 
            // dashboardToolStripMenuItem
            // 
            dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            dashboardToolStripMenuItem.Size = new Size(342, 44);
            dashboardToolStripMenuItem.Text = "Dashboard";
            dashboardToolStripMenuItem.Click += btnDashboard_Click;
            // 
            // teamsPlayersToolStripMenuItem
            // 
            teamsPlayersToolStripMenuItem.Name = "teamsPlayersToolStripMenuItem";
            teamsPlayersToolStripMenuItem.Size = new Size(342, 44);
            teamsPlayersToolStripMenuItem.Text = "Teams and Players";
            teamsPlayersToolStripMenuItem.Click += btnTeamsPlayers_Click;
            // 
            // bracketsToolStripMenuItem
            // 
            bracketsToolStripMenuItem.Name = "bracketsToolStripMenuItem";
            bracketsToolStripMenuItem.Size = new Size(342, 44);
            bracketsToolStripMenuItem.Text = "Bracket";
            bracketsToolStripMenuItem.Click += btnBracket_Click;
            // 
            // scheduleToolStripMenuItem
            // 
            scheduleToolStripMenuItem.Name = "scheduleToolStripMenuItem";
            scheduleToolStripMenuItem.Size = new Size(342, 44);
            scheduleToolStripMenuItem.Text = "Schedule";
            scheduleToolStripMenuItem.Click += btnSchedule_Click;
            // 
            // teamStatsToolStripMenuItem
            // 
            teamStatsToolStripMenuItem.Name = "teamStatsToolStripMenuItem";
            teamStatsToolStripMenuItem.Size = new Size(342, 44);
            teamStatsToolStripMenuItem.Text = "Team Stats";
            teamStatsToolStripMenuItem.Click += btnTeamStats_Click;
            // 
            // resultsToolStripMenuItem
            // 
            resultsToolStripMenuItem.Name = "resultsToolStripMenuItem";
            resultsToolStripMenuItem.Size = new Size(342, 44);
            resultsToolStripMenuItem.Text = "Results";
            resultsToolStripMenuItem.Click += btnResults_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { generateBracketToolStripMenuItem, autoScheduleToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(89, 36);
            toolsToolStripMenuItem.Text = "Tools";
            // 
            // generateBracketToolStripMenuItem
            // 
            generateBracketToolStripMenuItem.Name = "generateBracketToolStripMenuItem";
            generateBracketToolStripMenuItem.Size = new Size(329, 44);
            generateBracketToolStripMenuItem.Text = "Generate Bracket";
            generateBracketToolStripMenuItem.Click += tsbGenerateBracket_Click;
            // 
            // autoScheduleToolStripMenuItem
            // 
            autoScheduleToolStripMenuItem.Name = "autoScheduleToolStripMenuItem";
            autoScheduleToolStripMenuItem.Size = new Size(329, 44);
            autoScheduleToolStripMenuItem.Text = "Auto Schedule";
            autoScheduleToolStripMenuItem.Click += autoSchedule_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(84, 36);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(212, 44);
            aboutToolStripMenuItem.Text = "About";
            // 
            // accounToolStripMenuItem
            // 
            accounToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginToolStripMenuItem, logoutToolStripMenuItem });
            accounToolStripMenuItem.Name = "accounToolStripMenuItem";
            accounToolStripMenuItem.Size = new Size(121, 36);
            accounToolStripMenuItem.Text = "Account";
            // 
            // loginToolStripMenuItem
            // 
            loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            loginToolStripMenuItem.Size = new Size(222, 44);
            loginToolStripMenuItem.Text = "Login";
            loginToolStripMenuItem.Click += loginToolStripMenuItem_Click;
            // 
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(222, 44);
            logoutToolStripMenuItem.Text = "Logout";
            logoutToolStripMenuItem.Click += logoutToolStripMenuItem_Click;
            // 
            // toolMain
            // 
            toolMain.GripStyle = ToolStripGripStyle.Hidden;
            toolMain.ImageScalingSize = new Size(32, 32);
            toolMain.Items.AddRange(new ToolStripItem[] { tsbSave, tsbAddTeam, tsbGenerateBracket, tsbAutoSchedule, tsbEnterResult });
            toolMain.Location = new Point(0, 44);
            toolMain.Name = "toolMain";
            toolMain.Padding = new Padding(0, 0, 4, 0);
            toolMain.Size = new Size(1655, 42);
            toolMain.TabIndex = 2;
            toolMain.Text = "toolStrip1";
            // 
            // tsbSave
            // 
            tsbSave.Image = Properties.Resources.save;
            tsbSave.ImageTransparentColor = Color.Magenta;
            tsbSave.Name = "tsbSave";
            tsbSave.Size = new Size(100, 36);
            tsbSave.Text = "Save";
            tsbSave.Click += saveTournament_Click;
            // 
            // tsbAddTeam
            // 
            tsbAddTeam.Image = Properties.Resources.add_group;
            tsbAddTeam.ImageTransparentColor = Color.Magenta;
            tsbAddTeam.Name = "tsbAddTeam";
            tsbAddTeam.Size = new Size(157, 36);
            tsbAddTeam.Text = "Add Team";
            tsbAddTeam.Click += tsbAddTeam_Click;
            // 
            // tsbGenerateBracket
            // 
            tsbGenerateBracket.Image = Properties.Resources.New_bracket;
            tsbGenerateBracket.ImageTransparentColor = Color.Magenta;
            tsbGenerateBracket.Name = "tsbGenerateBracket";
            tsbGenerateBracket.Size = new Size(232, 36);
            tsbGenerateBracket.Text = "Generate Bracket";
            tsbGenerateBracket.Click += tsbGenerateBracket_Click;
            // 
            // tsbAutoSchedule
            // 
            tsbAutoSchedule.Image = Properties.Resources.schedule;
            tsbAutoSchedule.ImageTransparentColor = Color.Magenta;
            tsbAutoSchedule.Name = "tsbAutoSchedule";
            tsbAutoSchedule.Size = new Size(206, 36);
            tsbAutoSchedule.Text = "Auto Schedule";
            tsbAutoSchedule.Click += autoSchedule_Click;
            // 
            // tsbEnterResult
            // 
            tsbEnterResult.Image = Properties.Resources.enter;
            tsbEnterResult.ImageTransparentColor = Color.Magenta;
            tsbEnterResult.Name = "tsbEnterResult";
            tsbEnterResult.Size = new Size(176, 36);
            tsbEnterResult.Text = "Enter Result";
            tsbEnterResult.Click += btnResults_Click;
            // 
            // statusMain
            // 
            statusMain.ImageScalingSize = new Size(32, 32);
            statusMain.Items.AddRange(new ToolStripItem[] { sslHint });
            statusMain.Location = new Point(0, 1104);
            statusMain.Name = "statusMain";
            statusMain.Padding = new Padding(2, 0, 15, 0);
            statusMain.Size = new Size(1655, 42);
            statusMain.TabIndex = 3;
            statusMain.Text = "statusStrip1";
            // 
            // sslHint
            // 
            sslHint.Name = "sslHint";
            sslHint.Size = new Size(78, 32);
            sslHint.Text = "Ready";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.FixedPanel = FixedPanel.Panel1;
            splitMain.Location = new Point(0, 86);
            splitMain.Margin = new Padding(7, 9, 7, 9);
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
            splitMain.Size = new Size(1655, 1018);
            splitMain.SplitterDistance = 487;
            splitMain.SplitterWidth = 13;
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
            panelNav.Location = new Point(0, 151);
            panelNav.Margin = new Padding(4);
            panelNav.Name = "panelNav";
            panelNav.Padding = new Padding(13, 15, 13, 15);
            panelNav.Size = new Size(487, 867);
            panelNav.TabIndex = 1;
            // 
            // btnResults
            // 
            btnResults.Dock = DockStyle.Top;
            btnResults.Location = new Point(13, 240);
            btnResults.Margin = new Padding(4);
            btnResults.Name = "btnResults";
            btnResults.Size = new Size(461, 45);
            btnResults.TabIndex = 5;
            btnResults.Text = "Results";
            btnResults.UseVisualStyleBackColor = true;
            btnResults.Click += btnResults_Click;
            // 
            // btnStandings
            // 
            btnStandings.Dock = DockStyle.Top;
            btnStandings.Location = new Point(13, 195);
            btnStandings.Margin = new Padding(4);
            btnStandings.Name = "btnStandings";
            btnStandings.Size = new Size(461, 45);
            btnStandings.TabIndex = 4;
            btnStandings.Text = "Team Stats";
            btnStandings.UseVisualStyleBackColor = true;
            btnStandings.Click += btnTeamStats_Click;
            // 
            // btnSchedule
            // 
            btnSchedule.Dock = DockStyle.Top;
            btnSchedule.Location = new Point(13, 150);
            btnSchedule.Margin = new Padding(4);
            btnSchedule.Name = "btnSchedule";
            btnSchedule.Size = new Size(461, 45);
            btnSchedule.TabIndex = 3;
            btnSchedule.Text = "Schedule";
            btnSchedule.UseVisualStyleBackColor = true;
            btnSchedule.Click += btnSchedule_Click;
            // 
            // btnBracket
            // 
            btnBracket.Dock = DockStyle.Top;
            btnBracket.Location = new Point(13, 105);
            btnBracket.Margin = new Padding(4);
            btnBracket.Name = "btnBracket";
            btnBracket.Size = new Size(461, 45);
            btnBracket.TabIndex = 2;
            btnBracket.Text = "Bracket";
            btnBracket.UseVisualStyleBackColor = true;
            btnBracket.Click += btnBracket_Click;
            // 
            // btnTeamsPlayers
            // 
            btnTeamsPlayers.Dock = DockStyle.Top;
            btnTeamsPlayers.Location = new Point(13, 60);
            btnTeamsPlayers.Margin = new Padding(4);
            btnTeamsPlayers.Name = "btnTeamsPlayers";
            btnTeamsPlayers.Size = new Size(461, 45);
            btnTeamsPlayers.TabIndex = 1;
            btnTeamsPlayers.Text = "Teams and Players";
            btnTeamsPlayers.UseVisualStyleBackColor = true;
            btnTeamsPlayers.Click += btnTeamsPlayers_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.Dock = DockStyle.Top;
            btnDashboard.Location = new Point(13, 15);
            btnDashboard.Margin = new Padding(4);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(461, 45);
            btnDashboard.TabIndex = 0;
            btnDashboard.Text = "Dashboard";
            btnDashboard.UseVisualStyleBackColor = true;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // grpTournament
            // 
            grpTournament.Controls.Add(lblTournament);
            grpTournament.Controls.Add(btnSaveTournament);
            grpTournament.Controls.Add(btnOpenTournament);
            grpTournament.Controls.Add(btnNewTournament);
            grpTournament.Controls.Add(cboTournament);
            grpTournament.Dock = DockStyle.Top;
            grpTournament.Location = new Point(0, 0);
            grpTournament.Margin = new Padding(4);
            grpTournament.Name = "grpTournament";
            grpTournament.Padding = new Padding(4);
            grpTournament.Size = new Size(487, 151);
            grpTournament.TabIndex = 0;
            grpTournament.TabStop = false;
            // 
            // lblTournament
            // 
            lblTournament.AutoSize = true;
            lblTournament.Location = new Point(13, 21);
            lblTournament.Margin = new Padding(4, 0, 4, 0);
            lblTournament.Name = "lblTournament";
            lblTournament.Size = new Size(148, 32);
            lblTournament.TabIndex = 5;
            lblTournament.Text = "Tournament:";
            // 
            // btnSaveTournament
            // 
            btnSaveTournament.Location = new Point(191, 105);
            btnSaveTournament.Margin = new Padding(4);
            btnSaveTournament.Name = "btnSaveTournament";
            btnSaveTournament.Size = new Size(85, 45);
            btnSaveTournament.TabIndex = 4;
            btnSaveTournament.Text = "Save";
            btnSaveTournament.UseVisualStyleBackColor = true;
            btnSaveTournament.Click += saveTournament_Click;
            // 
            // btnOpenTournament
            // 
            btnOpenTournament.Location = new Point(102, 105);
            btnOpenTournament.Margin = new Padding(4);
            btnOpenTournament.Name = "btnOpenTournament";
            btnOpenTournament.Size = new Size(85, 45);
            btnOpenTournament.TabIndex = 3;
            btnOpenTournament.Text = "Open";
            btnOpenTournament.UseVisualStyleBackColor = true;
            btnOpenTournament.Click += openTournament_Click;
            // 
            // btnNewTournament
            // 
            btnNewTournament.Location = new Point(11, 105);
            btnNewTournament.Margin = new Padding(4);
            btnNewTournament.Name = "btnNewTournament";
            btnNewTournament.Size = new Size(85, 45);
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
            cboTournament.Location = new Point(11, 58);
            cboTournament.Margin = new Padding(4);
            cboTournament.Name = "cboTournament";
            cboTournament.Size = new Size(457, 40);
            cboTournament.TabIndex = 1;
            cboTournament.SelectedIndexChanged += cboTournament_SelectedIndexChanged;
            // 
            // panelWorkspace
            // 
            panelWorkspace.Controls.Add(labelWorkspaceTitle);
            panelWorkspace.Dock = DockStyle.Fill;
            panelWorkspace.Location = new Point(0, 0);
            panelWorkspace.Margin = new Padding(4);
            panelWorkspace.Name = "panelWorkspace";
            panelWorkspace.Size = new Size(1155, 1018);
            panelWorkspace.TabIndex = 0;
            // 
            // labelWorkspaceTitle
            // 
            labelWorkspaceTitle.AutoSize = true;
            labelWorkspaceTitle.Font = new Font("Segoe UI", 16F);
            labelWorkspaceTitle.Location = new Point(17, 15);
            labelWorkspaceTitle.Margin = new Padding(4, 0, 4, 0);
            labelWorkspaceTitle.Name = "labelWorkspaceTitle";
            labelWorkspaceTitle.Size = new Size(231, 59);
            labelWorkspaceTitle.TabIndex = 0;
            labelWorkspaceTitle.Text = "Dashboard";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1655, 1146);
            Controls.Add(splitMain);
            Controls.Add(statusMain);
            Controls.Add(toolMain);
            Controls.Add(menuMain);
            Margin = new Padding(4);
            MinimumSize = new Size(1075, 619);
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
        private ToolStripMenuItem teamStatsToolStripMenuItem;
        private ToolStripMenuItem resultsToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem generateBracketToolStripMenuItem;
        private ToolStripMenuItem autoScheduleToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip toolMain;
        private ToolStripButton tsbSave;
        private ToolStripButton tsbAddTeam;
        private ToolStripButton tsbGenerateBracket;
        private ToolStripButton tsbAutoSchedule;
        private ToolStripButton tsbEnterResult;
        private StatusStrip statusMain;
        private ToolStripStatusLabel sslHint;
        private SplitContainer splitMain;
        private GroupBox grpTournament;
        private Button btnNewTournament;
        private ComboBox cboTournament;
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
        private Label lblTournament;
        private ToolStripMenuItem accounToolStripMenuItem;
        private ToolStripMenuItem loginToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
    }
}
