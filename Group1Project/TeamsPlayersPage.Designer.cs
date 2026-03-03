namespace Group1Project
{
    partial class TeamsPlayersPage
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
            splitTeamsPlayers = new SplitContainer();
            dataViewTeams = new DataGridView();
            columnTeamName = new DataGridViewTextBoxColumn();
            columnSeed = new DataGridViewTextBoxColumn();
            columnPlayerCount = new DataGridViewTextBoxColumn();
            panelTeamsHeader = new Panel();
            buttonRemoveTeam = new Button();
            buttonAddTeam = new Button();
            buttonEditTeam = new Button();
            labelTeams = new Label();
            dataGridViewPlayers = new DataGridView();
            columnPlayerDisplayName = new DataGridViewTextBoxColumn();
            columnPlayerNumber = new DataGridViewTextBoxColumn();
            panelPlayersHeader = new Panel();
            buttonEditPlayer = new Button();
            buttonAddPlayer = new Button();
            buttonRemovePlayer = new Button();
            labelPlayers = new Label();
            ((System.ComponentModel.ISupportInitialize)splitTeamsPlayers).BeginInit();
            splitTeamsPlayers.Panel1.SuspendLayout();
            splitTeamsPlayers.Panel2.SuspendLayout();
            splitTeamsPlayers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataViewTeams).BeginInit();
            panelTeamsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayers).BeginInit();
            panelPlayersHeader.SuspendLayout();
            SuspendLayout();
            // 
            // splitTeamsPlayers
            // 
            splitTeamsPlayers.Dock = DockStyle.Fill;
            splitTeamsPlayers.Location = new Point(0, 0);
            splitTeamsPlayers.Margin = new Padding(2, 2, 2, 2);
            splitTeamsPlayers.Name = "splitTeamsPlayers";
            splitTeamsPlayers.Orientation = Orientation.Horizontal;
            // 
            // splitTeamsPlayers.Panel1
            // 
            splitTeamsPlayers.Panel1.Controls.Add(dataViewTeams);
            splitTeamsPlayers.Panel1.Controls.Add(panelTeamsHeader);
            splitTeamsPlayers.Panel1MinSize = 220;
            // 
            // splitTeamsPlayers.Panel2
            // 
            splitTeamsPlayers.Panel2.Controls.Add(dataGridViewPlayers);
            splitTeamsPlayers.Panel2.Controls.Add(panelPlayersHeader);
            splitTeamsPlayers.Panel2MinSize = 220;
            splitTeamsPlayers.Size = new Size(409, 316);
            splitTeamsPlayers.SplitterDistance = 138;
            splitTeamsPlayers.SplitterWidth = 2;
            splitTeamsPlayers.TabIndex = 0;
            // 
            // dataViewTeams
            // 
            dataViewTeams.AllowUserToAddRows = false;
            dataViewTeams.AllowUserToDeleteRows = false;
            dataViewTeams.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataViewTeams.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataViewTeams.Columns.AddRange(new DataGridViewColumn[] { columnTeamName, columnSeed, columnPlayerCount });
            dataViewTeams.Dock = DockStyle.Fill;
            dataViewTeams.Location = new Point(0, 30);
            dataViewTeams.Margin = new Padding(2, 2, 2, 2);
            dataViewTeams.Name = "dataViewTeams";
            dataViewTeams.ReadOnly = true;
            dataViewTeams.RowHeadersVisible = false;
            dataViewTeams.RowHeadersWidth = 82;
            dataViewTeams.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataViewTeams.Size = new Size(409, 108);
            dataViewTeams.TabIndex = 1;
            // 
            // columnTeamName
            // 
            columnTeamName.HeaderText = "Team";
            columnTeamName.MinimumWidth = 10;
            columnTeamName.Name = "columnTeamName";
            columnTeamName.ReadOnly = true;
            // 
            // columnSeed
            // 
            columnSeed.HeaderText = "Seed";
            columnSeed.MinimumWidth = 10;
            columnSeed.Name = "columnSeed";
            columnSeed.ReadOnly = true;
            // 
            // columnPlayerCount
            // 
            columnPlayerCount.HeaderText = "Players";
            columnPlayerCount.MinimumWidth = 10;
            columnPlayerCount.Name = "columnPlayerCount";
            columnPlayerCount.ReadOnly = true;
            // 
            // panelTeamsHeader
            // 
            panelTeamsHeader.Controls.Add(buttonRemoveTeam);
            panelTeamsHeader.Controls.Add(buttonAddTeam);
            panelTeamsHeader.Controls.Add(buttonEditTeam);
            panelTeamsHeader.Controls.Add(labelTeams);
            panelTeamsHeader.Dock = DockStyle.Top;
            panelTeamsHeader.Location = new Point(0, 0);
            panelTeamsHeader.Margin = new Padding(2, 2, 2, 2);
            panelTeamsHeader.Name = "panelTeamsHeader";
            panelTeamsHeader.Size = new Size(409, 30);
            panelTeamsHeader.TabIndex = 0;
            // 
            // buttonRemoveTeam
            // 
            buttonRemoveTeam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRemoveTeam.Location = new Point(348, 3);
            buttonRemoveTeam.Margin = new Padding(2, 2, 2, 2);
            buttonRemoveTeam.Name = "buttonRemoveTeam";
            buttonRemoveTeam.Size = new Size(59, 24);
            buttonRemoveTeam.TabIndex = 3;
            buttonRemoveTeam.Text = "Remove";
            buttonRemoveTeam.UseVisualStyleBackColor = true;
            // 
            // buttonAddTeam
            // 
            buttonAddTeam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAddTeam.Location = new Point(223, 3);
            buttonAddTeam.Margin = new Padding(2, 2, 2, 2);
            buttonAddTeam.Name = "buttonAddTeam";
            buttonAddTeam.Size = new Size(59, 24);
            buttonAddTeam.TabIndex = 2;
            buttonAddTeam.Text = "Add";
            buttonAddTeam.UseVisualStyleBackColor = true;
            buttonAddTeam.Click += buttonAddTeam_Click;
            // 
            // buttonEditTeam
            // 
            buttonEditTeam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonEditTeam.Location = new Point(285, 3);
            buttonEditTeam.Margin = new Padding(2, 2, 2, 2);
            buttonEditTeam.Name = "buttonEditTeam";
            buttonEditTeam.Size = new Size(59, 24);
            buttonEditTeam.TabIndex = 1;
            buttonEditTeam.Text = "Edit";
            buttonEditTeam.UseVisualStyleBackColor = true;
            // 
            // labelTeams
            // 
            labelTeams.AutoSize = true;
            labelTeams.Font = new Font("Segoe UI", 12F);
            labelTeams.Location = new Point(5, 3);
            labelTeams.Margin = new Padding(2, 0, 2, 0);
            labelTeams.Name = "labelTeams";
            labelTeams.Size = new Size(61, 25);
            labelTeams.TabIndex = 0;
            labelTeams.Text = "Teams";
            // 
            // dataGridViewPlayers
            // 
            dataGridViewPlayers.AllowUserToAddRows = false;
            dataGridViewPlayers.AllowUserToDeleteRows = false;
            dataGridViewPlayers.AllowUserToResizeRows = false;
            dataGridViewPlayers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewPlayers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPlayers.Columns.AddRange(new DataGridViewColumn[] { columnPlayerDisplayName, columnPlayerNumber });
            dataGridViewPlayers.Dock = DockStyle.Fill;
            dataGridViewPlayers.Location = new Point(0, 30);
            dataGridViewPlayers.Margin = new Padding(2, 2, 2, 2);
            dataGridViewPlayers.MultiSelect = false;
            dataGridViewPlayers.Name = "dataGridViewPlayers";
            dataGridViewPlayers.ReadOnly = true;
            dataGridViewPlayers.RowHeadersVisible = false;
            dataGridViewPlayers.RowHeadersWidth = 82;
            dataGridViewPlayers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlayers.Size = new Size(409, 145);
            dataGridViewPlayers.TabIndex = 1;
            // 
            // columnPlayerDisplayName
            // 
            columnPlayerDisplayName.HeaderText = "Name";
            columnPlayerDisplayName.MinimumWidth = 10;
            columnPlayerDisplayName.Name = "columnPlayerDisplayName";
            columnPlayerDisplayName.ReadOnly = true;
            // 
            // columnPlayerNumber
            // 
            columnPlayerNumber.HeaderText = "#";
            columnPlayerNumber.MinimumWidth = 10;
            columnPlayerNumber.Name = "columnPlayerNumber";
            columnPlayerNumber.ReadOnly = true;
            // 
            // panelPlayersHeader
            // 
            panelPlayersHeader.Controls.Add(buttonEditPlayer);
            panelPlayersHeader.Controls.Add(buttonAddPlayer);
            panelPlayersHeader.Controls.Add(buttonRemovePlayer);
            panelPlayersHeader.Controls.Add(labelPlayers);
            panelPlayersHeader.Dock = DockStyle.Top;
            panelPlayersHeader.Location = new Point(0, 0);
            panelPlayersHeader.Margin = new Padding(2, 2, 2, 2);
            panelPlayersHeader.Name = "panelPlayersHeader";
            panelPlayersHeader.Size = new Size(409, 30);
            panelPlayersHeader.TabIndex = 0;
            // 
            // buttonEditPlayer
            // 
            buttonEditPlayer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonEditPlayer.Location = new Point(285, 2);
            buttonEditPlayer.Margin = new Padding(2, 2, 2, 2);
            buttonEditPlayer.Name = "buttonEditPlayer";
            buttonEditPlayer.Size = new Size(59, 24);
            buttonEditPlayer.TabIndex = 3;
            buttonEditPlayer.Text = "Edit";
            buttonEditPlayer.UseVisualStyleBackColor = true;
            // 
            // buttonAddPlayer
            // 
            buttonAddPlayer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAddPlayer.Location = new Point(223, 2);
            buttonAddPlayer.Margin = new Padding(2, 2, 2, 2);
            buttonAddPlayer.Name = "buttonAddPlayer";
            buttonAddPlayer.Size = new Size(59, 24);
            buttonAddPlayer.TabIndex = 2;
            buttonAddPlayer.Text = "Add";
            buttonAddPlayer.UseVisualStyleBackColor = true;
            // 
            // buttonRemovePlayer
            // 
            buttonRemovePlayer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRemovePlayer.Location = new Point(348, 2);
            buttonRemovePlayer.Margin = new Padding(2, 2, 2, 2);
            buttonRemovePlayer.Name = "buttonRemovePlayer";
            buttonRemovePlayer.Size = new Size(59, 24);
            buttonRemovePlayer.TabIndex = 1;
            buttonRemovePlayer.Text = "Remove";
            buttonRemovePlayer.UseVisualStyleBackColor = true;
            // 
            // labelPlayers
            // 
            labelPlayers.AutoSize = true;
            labelPlayers.Font = new Font("Segoe UI", 12F);
            labelPlayers.Location = new Point(5, 3);
            labelPlayers.Margin = new Padding(2, 0, 2, 0);
            labelPlayers.Name = "labelPlayers";
            labelPlayers.Size = new Size(194, 25);
            labelPlayers.TabIndex = 0;
            labelPlayers.Text = "Players (Selected Team)";
            // 
            // TeamsPlayersPage
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitTeamsPlayers);
            Margin = new Padding(2, 2, 2, 2);
            Name = "TeamsPlayersPage";
            Size = new Size(409, 316);
            splitTeamsPlayers.Panel1.ResumeLayout(false);
            splitTeamsPlayers.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitTeamsPlayers).EndInit();
            splitTeamsPlayers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataViewTeams).EndInit();
            panelTeamsHeader.ResumeLayout(false);
            panelTeamsHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayers).EndInit();
            panelPlayersHeader.ResumeLayout(false);
            panelPlayersHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitTeamsPlayers;
        private Panel panelTeamsHeader;
        private Button buttonRemoveTeam;
        private Button buttonAddTeam;
        private Button buttonEditTeam;
        private Label labelTeams;
        private DataGridView dataViewTeams;
        private DataGridViewTextBoxColumn columnTeamName;
        private DataGridViewTextBoxColumn columnSeed;
        private DataGridViewTextBoxColumn columnPlayerCount;
        private Panel panelPlayersHeader;
        private Button buttonEditPlayer;
        private Button buttonAddPlayer;
        private Button buttonRemovePlayer;
        private Label labelPlayers;
        private DataGridView dataGridViewPlayers;
        private DataGridViewTextBoxColumn columnPlayerDisplayName;
        private DataGridViewTextBoxColumn columnPlayerNumber;
    }
}
