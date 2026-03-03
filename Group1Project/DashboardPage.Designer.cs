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
            labelCardMatchesTitle = new Label();
            cardPlayers = new Panel();
            labelCardPlayersTitle = new Label();
            cardTeams = new Panel();
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
            tableDashboard.Name = "tableDashboard";
            tableDashboard.RowCount = 3;
            tableDashboard.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tableDashboard.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            tableDashboard.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableDashboard.Size = new Size(1281, 655);
            tableDashboard.TabIndex = 0;
            // 
            // panelDashHeader
            // 
            panelDashHeader.Controls.Add(labelDashTournament);
            panelDashHeader.Controls.Add(labelDashTitle);
            panelDashHeader.Dock = DockStyle.Fill;
            panelDashHeader.Location = new Point(3, 3);
            panelDashHeader.Name = "panelDashHeader";
            panelDashHeader.Size = new Size(1275, 84);
            panelDashHeader.TabIndex = 0;
            // 
            // labelDashTournament
            // 
            labelDashTournament.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelDashTournament.AutoSize = true;
            labelDashTournament.Font = new Font("Segoe UI", 12F);
            labelDashTournament.Location = new Point(961, 27);
            labelDashTournament.Name = "labelDashTournament";
            labelDashTournament.Size = new Size(298, 45);
            labelDashTournament.TabIndex = 1;
            labelDashTournament.Text = "Tournament: (none)";
            // 
            // labelDashTitle
            // 
            labelDashTitle.AutoSize = true;
            labelDashTitle.Font = new Font("Segoe UI", 16F);
            labelDashTitle.Location = new Point(14, 18);
            labelDashTitle.Name = "labelDashTitle";
            labelDashTitle.Size = new Size(231, 59);
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
            tableLayoutPanel1.Location = new Point(3, 93);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(10);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1275, 174);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // cardNextMatch
            // 
            cardNextMatch.BackColor = SystemColors.Window;
            cardNextMatch.BorderStyle = BorderStyle.FixedSingle;
            cardNextMatch.Controls.Add(labelCardNextMatchTitle);
            cardNextMatch.Dock = DockStyle.Fill;
            cardNextMatch.Location = new Point(959, 20);
            cardNextMatch.Margin = new Padding(10);
            cardNextMatch.Name = "cardNextMatch";
            cardNextMatch.Size = new Size(296, 134);
            cardNextMatch.TabIndex = 3;
            // 
            // labelCardNextMatchTitle
            // 
            labelCardNextMatchTitle.Anchor = AnchorStyles.Top;
            labelCardNextMatchTitle.AutoSize = true;
            labelCardNextMatchTitle.Location = new Point(79, 0);
            labelCardNextMatchTitle.Name = "labelCardNextMatchTitle";
            labelCardNextMatchTitle.Size = new Size(138, 32);
            labelCardNextMatchTitle.TabIndex = 1;
            labelCardNextMatchTitle.Text = "Next Match";
            // 
            // cardMatches
            // 
            cardMatches.BackColor = SystemColors.Window;
            cardMatches.BorderStyle = BorderStyle.FixedSingle;
            cardMatches.Controls.Add(labelCardMatchesTitle);
            cardMatches.Dock = DockStyle.Fill;
            cardMatches.Location = new Point(646, 20);
            cardMatches.Margin = new Padding(10);
            cardMatches.Name = "cardMatches";
            cardMatches.Size = new Size(293, 134);
            cardMatches.TabIndex = 2;
            // 
            // labelCardMatchesTitle
            // 
            labelCardMatchesTitle.Anchor = AnchorStyles.Top;
            labelCardMatchesTitle.AutoSize = true;
            labelCardMatchesTitle.Location = new Point(90, 0);
            labelCardMatchesTitle.Name = "labelCardMatchesTitle";
            labelCardMatchesTitle.Size = new Size(104, 32);
            labelCardMatchesTitle.TabIndex = 1;
            labelCardMatchesTitle.Text = "Matches";
            // 
            // cardPlayers
            // 
            cardPlayers.BackColor = SystemColors.Window;
            cardPlayers.BorderStyle = BorderStyle.FixedSingle;
            cardPlayers.Controls.Add(labelCardPlayersTitle);
            cardPlayers.Dock = DockStyle.Fill;
            cardPlayers.Location = new Point(333, 20);
            cardPlayers.Margin = new Padding(10);
            cardPlayers.Name = "cardPlayers";
            cardPlayers.Size = new Size(293, 134);
            cardPlayers.TabIndex = 1;
            // 
            // labelCardPlayersTitle
            // 
            labelCardPlayersTitle.Anchor = AnchorStyles.Top;
            labelCardPlayersTitle.AutoSize = true;
            labelCardPlayersTitle.Location = new Point(105, 0);
            labelCardPlayersTitle.Name = "labelCardPlayersTitle";
            labelCardPlayersTitle.Size = new Size(88, 32);
            labelCardPlayersTitle.TabIndex = 1;
            labelCardPlayersTitle.Text = "Players";
            // 
            // cardTeams
            // 
            cardTeams.BackColor = SystemColors.Window;
            cardTeams.BorderStyle = BorderStyle.FixedSingle;
            cardTeams.Controls.Add(labelCardTeamsTitle);
            cardTeams.Dock = DockStyle.Fill;
            cardTeams.Location = new Point(20, 20);
            cardTeams.Margin = new Padding(10);
            cardTeams.Name = "cardTeams";
            cardTeams.Size = new Size(293, 134);
            cardTeams.TabIndex = 0;
            // 
            // labelCardTeamsTitle
            // 
            labelCardTeamsTitle.Anchor = AnchorStyles.Top;
            labelCardTeamsTitle.AutoSize = true;
            labelCardTeamsTitle.Location = new Point(111, 0);
            labelCardTeamsTitle.Name = "labelCardTeamsTitle";
            labelCardTeamsTitle.Size = new Size(81, 32);
            labelCardTeamsTitle.TabIndex = 0;
            labelCardTeamsTitle.Text = "Teams";
            // 
            // splitDashBottom
            // 
            splitDashBottom.Dock = DockStyle.Fill;
            splitDashBottom.Location = new Point(3, 273);
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
            splitDashBottom.Size = new Size(1275, 379);
            splitDashBottom.SplitterDistance = 520;
            splitDashBottom.TabIndex = 2;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.Location = new Point(0, 32);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(520, 347);
            listBox1.TabIndex = 1;
            // 
            // labelRecent
            // 
            labelRecent.AutoSize = true;
            labelRecent.Dock = DockStyle.Top;
            labelRecent.Location = new Point(0, 0);
            labelRecent.Name = "labelRecent";
            labelRecent.Size = new Size(164, 32);
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
            flowLayoutPanel1.Location = new Point(0, 32);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(10);
            flowLayoutPanel1.Size = new Size(751, 347);
            flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.WrapContents = false;
            // 
            // labelUpcomingEvents
            // 
            labelUpcomingEvents.AutoSize = true;
            labelUpcomingEvents.Dock = DockStyle.Top;
            labelUpcomingEvents.Location = new Point(0, 0);
            labelUpcomingEvents.Name = "labelUpcomingEvents";
            labelUpcomingEvents.Size = new Size(200, 32);
            labelUpcomingEvents.TabIndex = 0;
            labelUpcomingEvents.Text = "Upcoming Events";
            // 
            // DashboardPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableDashboard);
            Name = "DashboardPage";
            Size = new Size(1281, 655);
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
    }
}
