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
            panelTeamsHeader = new Panel();
            labelTeams = new Label();
            buttonEditTeam = new Button();
            buttonAddTeam = new Button();
            buttonRemoveTeam = new Button();
            dataViewTeams = new DataGridView();
            columnTeamName = new DataGridViewTextBoxColumn();
            columnSeed = new DataGridViewTextBoxColumn();
            columnPlayerCount = new DataGridViewTextBoxColumn();
            panelPlayersHeader = new Panel();
            labelPlayers = new Label();
            buttonRemovePlayer = new Button();
            buttonAddPlayer = new Button();
            buttonEditPlayer = new Button();
            dataGridViewPlayers = new DataGridView();
            columnPlayerDisplayName = new DataGridViewTextBoxColumn();
            columnPlayerNumber = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)splitTeamsPlayers).BeginInit();
            splitTeamsPlayers.Panel1.SuspendLayout();
            splitTeamsPlayers.Panel2.SuspendLayout();
            splitTeamsPlayers.SuspendLayout();
            panelTeamsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataViewTeams).BeginInit();
            panelPlayersHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayers).BeginInit();
            SuspendLayout();
            // 
            // splitTeamsPlayers
            // 
            splitTeamsPlayers.Dock = DockStyle.Fill;
            splitTeamsPlayers.Location = new Point(0, 0);
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
            splitTeamsPlayers.Size = new Size(759, 594);
            splitTeamsPlayers.SplitterDistance = 260;
            splitTeamsPlayers.TabIndex = 0;
            // 
            // panelTeamsHeader
            // 
            panelTeamsHeader.Controls.Add(buttonRemoveTeam);
            panelTeamsHeader.Controls.Add(buttonAddTeam);
            panelTeamsHeader.Controls.Add(buttonEditTeam);
            panelTeamsHeader.Controls.Add(labelTeams);
            panelTeamsHeader.Dock = DockStyle.Top;
            panelTeamsHeader.Location = new Point(0, 0);
            panelTeamsHeader.Name = "panelTeamsHeader";
            panelTeamsHeader.Size = new Size(759, 56);
            panelTeamsHeader.TabIndex = 0;
            // 
            // labelTeams
            // 
            labelTeams.AutoSize = true;
            labelTeams.Font = new Font("Segoe UI", 12F);
            labelTeams.Location = new Point(10, 5);
            labelTeams.Name = "labelTeams";
            labelTeams.Size = new Size(109, 45);
            labelTeams.TabIndex = 0;
            labelTeams.Text = "Teams";
            // 
            // buttonEditTeam
            // 
            buttonEditTeam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonEditTeam.Location = new Point(530, 5);
            buttonEditTeam.Name = "buttonEditTeam";
            buttonEditTeam.Size = new Size(110, 45);
            buttonEditTeam.TabIndex = 1;
            buttonEditTeam.Text = "Edit";
            buttonEditTeam.UseVisualStyleBackColor = true;
            // 
            // buttonAddTeam
            // 
            buttonAddTeam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAddTeam.Location = new Point(414, 5);
            buttonAddTeam.Name = "buttonAddTeam";
            buttonAddTeam.Size = new Size(110, 45);
            buttonAddTeam.TabIndex = 2;
            buttonAddTeam.Text = "Add";
            buttonAddTeam.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveTeam
            // 
            buttonRemoveTeam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRemoveTeam.Location = new Point(646, 5);
            buttonRemoveTeam.Name = "buttonRemoveTeam";
            buttonRemoveTeam.Size = new Size(110, 45);
            buttonRemoveTeam.TabIndex = 3;
            buttonRemoveTeam.Text = "Remove";
            buttonRemoveTeam.UseVisualStyleBackColor = true;
            // 
            // dataViewTeams
            // 
            dataViewTeams.AllowUserToAddRows = false;
            dataViewTeams.AllowUserToDeleteRows = false;
            dataViewTeams.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataViewTeams.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataViewTeams.Columns.AddRange(new DataGridViewColumn[] { columnTeamName, columnSeed, columnPlayerCount });
            dataViewTeams.Dock = DockStyle.Fill;
            dataViewTeams.Location = new Point(0, 56);
            dataViewTeams.Name = "dataViewTeams";
            dataViewTeams.ReadOnly = true;
            dataViewTeams.RowHeadersVisible = false;
            dataViewTeams.RowHeadersWidth = 82;
            dataViewTeams.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataViewTeams.Size = new Size(759, 204);
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
            // panelPlayersHeader
            // 
            panelPlayersHeader.Controls.Add(buttonEditPlayer);
            panelPlayersHeader.Controls.Add(buttonAddPlayer);
            panelPlayersHeader.Controls.Add(buttonRemovePlayer);
            panelPlayersHeader.Controls.Add(labelPlayers);
            panelPlayersHeader.Dock = DockStyle.Top;
            panelPlayersHeader.Location = new Point(0, 0);
            panelPlayersHeader.Name = "panelPlayersHeader";
            panelPlayersHeader.Size = new Size(759, 56);
            panelPlayersHeader.TabIndex = 0;
            // 
            // labelPlayers
            // 
            labelPlayers.AutoSize = true;
            labelPlayers.Font = new Font("Segoe UI", 12F);
            labelPlayers.Location = new Point(10, 5);
            labelPlayers.Name = "labelPlayers";
            labelPlayers.Size = new Size(353, 45);
            labelPlayers.TabIndex = 0;
            labelPlayers.Text = "Players (Selected Team)";
            // 
            // buttonRemovePlayer
            // 
            buttonRemovePlayer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRemovePlayer.Location = new Point(646, 3);
            buttonRemovePlayer.Name = "buttonRemovePlayer";
            buttonRemovePlayer.Size = new Size(110, 45);
            buttonRemovePlayer.TabIndex = 1;
            buttonRemovePlayer.Text = "Remove";
            buttonRemovePlayer.UseVisualStyleBackColor = true;
            // 
            // buttonAddPlayer
            // 
            buttonAddPlayer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonAddPlayer.Location = new Point(414, 3);
            buttonAddPlayer.Name = "buttonAddPlayer";
            buttonAddPlayer.Size = new Size(110, 45);
            buttonAddPlayer.TabIndex = 2;
            buttonAddPlayer.Text = "Add";
            buttonAddPlayer.UseVisualStyleBackColor = true;
            // 
            // buttonEditPlayer
            // 
            buttonEditPlayer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonEditPlayer.Location = new Point(530, 3);
            buttonEditPlayer.Name = "buttonEditPlayer";
            buttonEditPlayer.Size = new Size(110, 45);
            buttonEditPlayer.TabIndex = 3;
            buttonEditPlayer.Text = "Edit";
            buttonEditPlayer.UseVisualStyleBackColor = true;
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
            dataGridViewPlayers.Location = new Point(0, 56);
            dataGridViewPlayers.MultiSelect = false;
            dataGridViewPlayers.Name = "dataGridViewPlayers";
            dataGridViewPlayers.ReadOnly = true;
            dataGridViewPlayers.RowHeadersVisible = false;
            dataGridViewPlayers.RowHeadersWidth = 82;
            dataGridViewPlayers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlayers.Size = new Size(759, 274);
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
            // TeamsPlayersPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitTeamsPlayers);
            Name = "TeamsPlayersPage";
            Size = new Size(759, 594);
            splitTeamsPlayers.Panel1.ResumeLayout(false);
            splitTeamsPlayers.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitTeamsPlayers).EndInit();
            splitTeamsPlayers.ResumeLayout(false);
            panelTeamsHeader.ResumeLayout(false);
            panelTeamsHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataViewTeams).EndInit();
            panelPlayersHeader.ResumeLayout(false);
            panelPlayersHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayers).EndInit();
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
