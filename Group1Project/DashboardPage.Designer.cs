/// <summary>
/// Group 1 Project - DashBroadPage Disigner
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    partial class DashboardPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableDashboard = new TableLayoutPanel();
            panelDashHeader = new Panel();
            labelDashTournament = new Label();
            labelDashTitle = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            cardNextMatch = new Panel();
            labelCardNextMatchTitle = new Label();
            cardMatches = new Panel();
            lblMatchesCount = new Label();
            labelCardMatchesTitle = new Label();
            cardPlayers = new Panel();
            lblPlayersCount = new Label();
            labelCardPlayersTitle = new Label();
            cardTeams = new Panel();
            lblTeamsCount = new Label();
            labelCardTeamsTitle = new Label();
            splitDashBottom = new SplitContainer();
            listBox1 = new ListBox();
            labelRecent = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            labelUpcomingEvents = new Label();
            tableDashboard.SuspendLayout();
            panelDashHeader.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            cardNextMatch.SuspendLayout();
            cardMatches.SuspendLayout();
            cardPlayers.SuspendLayout();
            cardTeams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitDashBottom).BeginInit();
            splitDashBottom.Panel1.SuspendLayout();
            splitDashBottom.Panel2.SuspendLayout();
            splitDashBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tableDashboard
            // 
            tableDashboard.ColumnCount = 1;
            tableDashboard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableDashboard.Controls.Add(panelDashHeader, 0, 0);
            tableDashboard.Controls.Add(tableLayoutPanel1, 0, 1);
            tableDashboard.Controls.Add(splitDashBottom, 0, 2);
            tableDashboard.Dock = DockStyle.Fill;
            tableDashboard.Location = new Point(0, 0);
            tableDashboard.Margin = new Padding(2);
            tableDashboard.Name = "tableDashboard";
            tableDashboard.RowCount = 3;
            tableDashboard.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableDashboard.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            tableDashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableDashboard.Size = new Size(690, 307);
            tableDashboard.TabIndex = 0;
            // 
            // panelDashHeader
            // 
            panelDashHeader.Controls.Add(labelDashTournament);
            panelDashHeader.Controls.Add(labelDashTitle);
            panelDashHeader.Dock = DockStyle.Fill;
            panelDashHeader.Location = new Point(2, 2);
            panelDashHeader.Margin = new Padding(2);
            panelDashHeader.Name = "panelDashHeader";
            panelDashHeader.Size = new Size(686, 38);
            panelDashHeader.TabIndex = 0;
            // 
            // labelDashTournament
            // 
            labelDashTournament.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelDashTournament.AutoSize = true;
            labelDashTournament.Font = new Font("Segoe UI", 12F);
            labelDashTournament.Location = new Point(516, 12);
            labelDashTournament.Margin = new Padding(2, 0, 2, 0);
            labelDashTournament.Name = "labelDashTournament";
            labelDashTournament.Size = new Size(145, 21);
            labelDashTournament.TabIndex = 1;
            labelDashTournament.Text = "Tournament: (none)";
            // 
            // labelDashTitle
            // 
            labelDashTitle.AutoSize = true;
            labelDashTitle.Font = new Font("Segoe UI", 16F);
            labelDashTitle.Location = new Point(8, 9);
            labelDashTitle.Margin = new Padding(2, 0, 2, 0);
            labelDashTitle.Name = "labelDashTitle";
            labelDashTitle.Size = new Size(118, 30);
            labelDashTitle.TabIndex = 0;
            labelDashTitle.Text = "Dashboard";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(cardNextMatch, 3, 0);
            tableLayoutPanel1.Controls.Add(cardMatches, 2, 0);
            tableLayoutPanel1.Controls.Add(cardPlayers, 1, 0);
            tableLayoutPanel1.Controls.Add(cardTeams, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(2, 44);
            tableLayoutPanel1.Margin = new Padding(2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(5, 4, 5, 4);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(686, 81);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // cardNextMatch
            // 
            cardNextMatch.BackColor = SystemColors.Window;
            cardNextMatch.BorderStyle = BorderStyle.FixedSingle;
            cardNextMatch.Controls.Add(labelCardNextMatchTitle);
            cardNextMatch.Dock = DockStyle.Fill;
            cardNextMatch.Location = new Point(517, 8);
            cardNextMatch.Margin = new Padding(5, 4, 5, 4);
            cardNextMatch.Name = "cardNextMatch";
            cardNextMatch.Size = new Size(159, 65);
            cardNextMatch.TabIndex = 3;
            // 
            // labelCardNextMatchTitle
            // 
            labelCardNextMatchTitle.Anchor = AnchorStyles.Top;
            labelCardNextMatchTitle.AutoSize = true;
            labelCardNextMatchTitle.Location = new Point(42, 0);
            labelCardNextMatchTitle.Margin = new Padding(2, 0, 2, 0);
            labelCardNextMatchTitle.Name = "labelCardNextMatchTitle";
            labelCardNextMatchTitle.Size = new Size(68, 15);
            labelCardNextMatchTitle.TabIndex = 1;
            labelCardNextMatchTitle.Text = "Next Match";
            // 
            // cardMatches
            // 
            cardMatches.BackColor = SystemColors.Window;
            cardMatches.BorderStyle = BorderStyle.FixedSingle;
            cardMatches.Controls.Add(lblMatchesCount);
            cardMatches.Controls.Add(labelCardMatchesTitle);
            cardMatches.Dock = DockStyle.Fill;
            cardMatches.Location = new Point(348, 8);
            cardMatches.Margin = new Padding(5, 4, 5, 4);
            cardMatches.Name = "cardMatches";
            cardMatches.Size = new Size(159, 65);
            cardMatches.TabIndex = 2;
            // 
            // lblMatchesCount
            // 
            lblMatchesCount.Anchor = AnchorStyles.None;
            lblMatchesCount.AutoSize = true;
            lblMatchesCount.Font = new Font("Segoe UI", 16F);
            lblMatchesCount.Location = new Point(59, 23);
            lblMatchesCount.Name = "lblMatchesCount";
            lblMatchesCount.Size = new Size(25, 30);
            lblMatchesCount.TabIndex = 2;
            lblMatchesCount.Text = "3";
            // 
            // labelCardMatchesTitle
            // 
            labelCardMatchesTitle.Anchor = AnchorStyles.Top;
            labelCardMatchesTitle.AutoSize = true;
            labelCardMatchesTitle.Location = new Point(47, 0);
            labelCardMatchesTitle.Margin = new Padding(2, 0, 2, 0);
            labelCardMatchesTitle.Name = "labelCardMatchesTitle";
            labelCardMatchesTitle.Size = new Size(52, 15);
            labelCardMatchesTitle.TabIndex = 1;
            labelCardMatchesTitle.Text = "Matches";
            // 
            // cardPlayers
            // 
            cardPlayers.BackColor = SystemColors.Window;
            cardPlayers.BorderStyle = BorderStyle.FixedSingle;
            cardPlayers.Controls.Add(lblPlayersCount);
            cardPlayers.Controls.Add(labelCardPlayersTitle);
            cardPlayers.Dock = DockStyle.Fill;
            cardPlayers.Location = new Point(179, 8);
            cardPlayers.Margin = new Padding(5, 4, 5, 4);
            cardPlayers.Name = "cardPlayers";
            cardPlayers.Size = new Size(159, 65);
            cardPlayers.TabIndex = 1;
            // 
            // lblPlayersCount
            // 
            lblPlayersCount.Anchor = AnchorStyles.None;
            lblPlayersCount.AutoSize = true;
            lblPlayersCount.Font = new Font("Segoe UI", 16F);
            lblPlayersCount.Location = new Point(61, 23);
            lblPlayersCount.Name = "lblPlayersCount";
            lblPlayersCount.Size = new Size(25, 30);
            lblPlayersCount.TabIndex = 2;
            lblPlayersCount.Text = "2";
            // 
            // labelCardPlayersTitle
            // 
            labelCardPlayersTitle.Anchor = AnchorStyles.Top;
            labelCardPlayersTitle.AutoSize = true;
            labelCardPlayersTitle.Location = new Point(56, 0);
            labelCardPlayersTitle.Margin = new Padding(2, 0, 2, 0);
            labelCardPlayersTitle.Name = "labelCardPlayersTitle";
            labelCardPlayersTitle.Size = new Size(44, 15);
            labelCardPlayersTitle.TabIndex = 1;
            labelCardPlayersTitle.Text = "Players";
            // 
            // cardTeams
            // 
            cardTeams.BackColor = SystemColors.Window;
            cardTeams.BorderStyle = BorderStyle.FixedSingle;
            cardTeams.Controls.Add(lblTeamsCount);
            cardTeams.Controls.Add(labelCardTeamsTitle);
            cardTeams.Dock = DockStyle.Fill;
            cardTeams.Location = new Point(10, 8);
            cardTeams.Margin = new Padding(5, 4, 5, 4);
            cardTeams.Name = "cardTeams";
            cardTeams.Size = new Size(159, 65);
            cardTeams.TabIndex = 0;
            // 
            // lblTeamsCount
            // 
            lblTeamsCount.Anchor = AnchorStyles.None;
            lblTeamsCount.AutoSize = true;
            lblTeamsCount.Font = new Font("Segoe UI", 16F);
            lblTeamsCount.Location = new Point(65, 23);
            lblTeamsCount.Name = "lblTeamsCount";
            lblTeamsCount.Size = new Size(25, 30);
            lblTeamsCount.TabIndex = 1;
            lblTeamsCount.Text = "1";
            // 
            // labelCardTeamsTitle
            // 
            labelCardTeamsTitle.Anchor = AnchorStyles.Top;
            labelCardTeamsTitle.AutoSize = true;
            labelCardTeamsTitle.Location = new Point(59, 0);
            labelCardTeamsTitle.Margin = new Padding(2, 0, 2, 0);
            labelCardTeamsTitle.Name = "labelCardTeamsTitle";
            labelCardTeamsTitle.Size = new Size(41, 15);
            labelCardTeamsTitle.TabIndex = 0;
            labelCardTeamsTitle.Text = "Teams";
            // 
            // splitDashBottom
            // 
            splitDashBottom.Dock = DockStyle.Fill;
            splitDashBottom.Location = new Point(2, 129);
            splitDashBottom.Margin = new Padding(2);
            splitDashBottom.Name = "splitDashBottom";
            // 
            // splitDashBottom.Panel1
            // 
            splitDashBottom.Panel1.Controls.Add(listBox1);
            splitDashBottom.Panel1.Controls.Add(labelRecent);
            // 
            // splitDashBottom.Panel2
            // 
            splitDashBottom.Panel2.Controls.Add(flowLayoutPanel1);
            splitDashBottom.Panel2.Controls.Add(labelUpcomingEvents);
            splitDashBottom.Size = new Size(686, 176);
            splitDashBottom.SplitterDistance = 278;
            splitDashBottom.SplitterWidth = 2;
            splitDashBottom.TabIndex = 2;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(0, 15);
            listBox1.Margin = new Padding(2);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(278, 161);
            listBox1.TabIndex = 1;
            // 
            // labelRecent
            // 
            labelRecent.AutoSize = true;
            labelRecent.Dock = DockStyle.Top;
            labelRecent.Location = new Point(0, 0);
            labelRecent.Margin = new Padding(2, 0, 2, 0);
            labelRecent.Name = "labelRecent";
            labelRecent.Size = new Size(83, 15);
            labelRecent.TabIndex = 0;
            labelRecent.Text = "RecentActivity";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.BackColor = SystemColors.Window;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 15);
            flowLayoutPanel1.Margin = new Padding(2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5, 4, 5, 4);
            flowLayoutPanel1.Size = new Size(406, 161);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.WrapContents = false;
            // 
            // labelUpcomingEvents
            // 
            labelUpcomingEvents.AutoSize = true;
            labelUpcomingEvents.Dock = DockStyle.Top;
            labelUpcomingEvents.Location = new Point(0, 0);
            labelUpcomingEvents.Margin = new Padding(2, 0, 2, 0);
            labelUpcomingEvents.Name = "labelUpcomingEvents";
            labelUpcomingEvents.Size = new Size(100, 15);
            labelUpcomingEvents.TabIndex = 0;
            labelUpcomingEvents.Text = "Upcoming Events";
            // 
            // DashboardPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableDashboard);
            Margin = new Padding(2);
            Name = "DashboardPage";
            Size = new Size(690, 307);
            tableDashboard.ResumeLayout(false);
            panelDashHeader.ResumeLayout(false);
            panelDashHeader.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            cardNextMatch.ResumeLayout(false);
            cardNextMatch.PerformLayout();
            cardMatches.ResumeLayout(false);
            cardMatches.PerformLayout();
            cardPlayers.ResumeLayout(false);
            cardPlayers.PerformLayout();
            cardTeams.ResumeLayout(false);
            cardTeams.PerformLayout();
            splitDashBottom.Panel1.ResumeLayout(false);
            splitDashBottom.Panel1.PerformLayout();
            splitDashBottom.Panel2.ResumeLayout(false);
            splitDashBottom.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitDashBottom).EndInit();
            splitDashBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableDashboard;
        private Panel panelDashHeader;
        private Label labelDashTournament;
        private Label labelDashTitle;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel cardNextMatch;
        private Panel cardMatches;
        private Panel cardPlayers;
        private Panel cardTeams;
        private Label labelCardTeamsTitle;
        private Label labelCardNextMatchTitle;
        private Label labelCardMatchesTitle;
        private Label labelCardPlayersTitle;
        private SplitContainer splitDashBottom;
        private ListBox listBox1;
        private Label labelRecent;
        private Label labelUpcomingEvents;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label lblMatchesCount;
        private Label lblPlayersCount;
        private Label lblTeamsCount;
    }
}
