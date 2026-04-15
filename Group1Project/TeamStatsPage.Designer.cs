namespace Group1Project
{
    partial class TeamStatsPage
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
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            buttonRefreshStats = new Button();
            labelHint = new Label();
            labelTitle = new Label();
            dataGridViewTeamStats = new DataGridView();
            columnTeam = new DataGridViewTextBoxColumn();
            columnGP = new DataGridViewTextBoxColumn();
            columnW = new DataGridViewTextBoxColumn();
            columnL = new DataGridViewTextBoxColumn();
            columnD = new DataGridViewTextBoxColumn();
            columnPF = new DataGridViewTextBoxColumn();
            columnPA = new DataGridViewTextBoxColumn();
            columnPD = new DataGridViewTextBoxColumn();
            columnWinPct = new DataGridViewTextBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTeamStats).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(dataGridViewTeamStats, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1278, 986);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonRefreshStats);
            panel1.Controls.Add(labelHint);
            panel1.Controls.Add(labelTitle);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1272, 64);
            panel1.TabIndex = 0;
            // 
            // buttonRefreshStats
            // 
            buttonRefreshStats.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRefreshStats.Location = new Point(1107, 10);
            buttonRefreshStats.Name = "buttonRefreshStats";
            buttonRefreshStats.Size = new Size(150, 46);
            buttonRefreshStats.TabIndex = 2;
            buttonRefreshStats.Text = "Refresh";
            buttonRefreshStats.UseVisualStyleBackColor = true;
            // 
            // labelHint
            // 
            labelHint.AutoSize = true;
            labelHint.Location = new Point(213, 24);
            labelHint.Name = "labelHint";
            labelHint.Size = new Size(329, 32);
            labelHint.TabIndex = 1;
            labelHint.Text = "Based on completed matches";
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 14F);
            labelTitle.Location = new Point(19, 10);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(200, 51);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Team Stats";
            // 
            // dataGridViewTeamStats
            // 
            dataGridViewTeamStats.AllowUserToAddRows = false;
            dataGridViewTeamStats.AllowUserToDeleteRows = false;
            dataGridViewTeamStats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewTeamStats.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTeamStats.Columns.AddRange(new DataGridViewColumn[] { columnTeam, columnGP, columnW, columnL, columnD, columnPF, columnPA, columnPD, columnWinPct });
            dataGridViewTeamStats.Dock = DockStyle.Fill;
            dataGridViewTeamStats.Location = new Point(3, 73);
            dataGridViewTeamStats.MultiSelect = false;
            dataGridViewTeamStats.Name = "dataGridViewTeamStats";
            dataGridViewTeamStats.ReadOnly = true;
            dataGridViewTeamStats.RowHeadersVisible = false;
            dataGridViewTeamStats.RowHeadersWidth = 82;
            dataGridViewTeamStats.Size = new Size(1272, 910);
            dataGridViewTeamStats.TabIndex = 1;
            // 
            // columnTeam
            // 
            columnTeam.HeaderText = "Team";
            columnTeam.MinimumWidth = 10;
            columnTeam.Name = "columnTeam";
            columnTeam.ReadOnly = true;
            // 
            // columnGP
            // 
            columnGP.HeaderText = "GP";
            columnGP.MinimumWidth = 10;
            columnGP.Name = "columnGP";
            columnGP.ReadOnly = true;
            // 
            // columnW
            // 
            columnW.HeaderText = "Wins";
            columnW.MinimumWidth = 10;
            columnW.Name = "columnW";
            columnW.ReadOnly = true;
            // 
            // columnL
            // 
            columnL.HeaderText = "Losses";
            columnL.MinimumWidth = 10;
            columnL.Name = "columnL";
            columnL.ReadOnly = true;
            // 
            // columnD
            // 
            columnD.HeaderText = "Draws";
            columnD.MinimumWidth = 10;
            columnD.Name = "columnD";
            columnD.ReadOnly = true;
            // 
            // columnPF
            // 
            columnPF.HeaderText = "Points For";
            columnPF.MinimumWidth = 10;
            columnPF.Name = "columnPF";
            columnPF.ReadOnly = true;
            // 
            // columnPA
            // 
            columnPA.HeaderText = "Points Against";
            columnPA.MinimumWidth = 10;
            columnPA.Name = "columnPA";
            columnPA.ReadOnly = true;
            // 
            // columnPD
            // 
            columnPD.HeaderText = "Poingts Difference";
            columnPD.MinimumWidth = 10;
            columnPD.Name = "columnPD";
            columnPD.ReadOnly = true;
            // 
            // columnWinPct
            // 
            columnWinPct.HeaderText = "Win %";
            columnWinPct.MinimumWidth = 10;
            columnWinPct.Name = "columnWinPct";
            columnWinPct.ReadOnly = true;
            // 
            // TeamStatsPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "TeamStatsPage";
            Size = new Size(1278, 986);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTeamStats).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label labelTitle;
        private Button buttonRefreshStats;
        private Label labelHint;
        private DataGridView dataGridViewTeamStats;
        private DataGridViewTextBoxColumn columnTeam;
        private DataGridViewTextBoxColumn columnGP;
        private DataGridViewTextBoxColumn columnW;
        private DataGridViewTextBoxColumn columnL;
        private DataGridViewTextBoxColumn columnD;
        private DataGridViewTextBoxColumn columnPF;
        private DataGridViewTextBoxColumn columnPA;
        private DataGridViewTextBoxColumn columnPD;
        private DataGridViewTextBoxColumn columnWinPct;
    }
}
