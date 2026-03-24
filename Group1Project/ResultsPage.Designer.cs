namespace Group1Project
{
    partial class ResultsPage
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
            tableLayoutMain = new TableLayoutPanel();
            panel1 = new Panel();
            labelSelectedMatch = new Label();
            numericScoreA = new NumericUpDown();
            numericScoreB = new NumericUpDown();
            buttonSetResult = new Button();
            buttonRefresh = new Button();
            dataGridViewResults = new DataGridView();
            columnTeamA = new DataGridViewTextBoxColumn();
            columnTeamB = new DataGridViewTextBoxColumn();
            columnScheduled = new DataGridViewTextBoxColumn();
            columnScore = new DataGridViewTextBoxColumn();
            columnWinner = new DataGridViewTextBoxColumn();
            columnStatus = new DataGridViewTextBoxColumn();
            tableLayoutMain.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericScoreA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericScoreB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutMain
            // 
            tableLayoutMain.ColumnCount = 1;
            tableLayoutMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutMain.Controls.Add(panel1, 0, 0);
            tableLayoutMain.Controls.Add(dataGridViewResults, 0, 1);
            tableLayoutMain.Dock = DockStyle.Fill;
            tableLayoutMain.Location = new Point(0, 0);
            tableLayoutMain.Name = "tableLayoutMain";
            tableLayoutMain.RowCount = 2;
            tableLayoutMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            tableLayoutMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutMain.Size = new Size(1241, 1021);
            tableLayoutMain.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonRefresh);
            panel1.Controls.Add(buttonSetResult);
            panel1.Controls.Add(numericScoreB);
            panel1.Controls.Add(numericScoreA);
            panel1.Controls.Add(labelSelectedMatch);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1235, 104);
            panel1.TabIndex = 0;
            // 
            // labelSelectedMatch
            // 
            labelSelectedMatch.AutoSize = true;
            labelSelectedMatch.Location = new Point(15, 35);
            labelSelectedMatch.Name = "labelSelectedMatch";
            labelSelectedMatch.Size = new Size(186, 32);
            labelSelectedMatch.TabIndex = 0;
            labelSelectedMatch.Text = "Selected: (none)";
            // 
            // numericScoreA
            // 
            numericScoreA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericScoreA.Location = new Point(635, 35);
            numericScoreA.Name = "numericScoreA";
            numericScoreA.Size = new Size(130, 39);
            numericScoreA.TabIndex = 1;
            // 
            // numericScoreB
            // 
            numericScoreB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            numericScoreB.Location = new Point(771, 35);
            numericScoreB.Name = "numericScoreB";
            numericScoreB.Size = new Size(120, 39);
            numericScoreB.TabIndex = 2;
            // 
            // buttonSetResult
            // 
            buttonSetResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSetResult.Location = new Point(917, 35);
            buttonSetResult.Name = "buttonSetResult";
            buttonSetResult.Size = new Size(150, 48);
            buttonSetResult.TabIndex = 3;
            buttonSetResult.Text = "Set Result";
            buttonSetResult.UseVisualStyleBackColor = true;
            buttonSetResult.Click += buttonSetResult_Click;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonRefresh.Location = new Point(1073, 35);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(150, 46);
            buttonRefresh.TabIndex = 4;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // dataGridViewResults
            // 
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewResults.Columns.AddRange(new DataGridViewColumn[] { columnTeamA, columnTeamB, columnScheduled, columnScore, columnWinner, columnStatus });
            dataGridViewResults.Dock = DockStyle.Fill;
            dataGridViewResults.Location = new Point(3, 113);
            dataGridViewResults.MultiSelect = false;
            dataGridViewResults.Name = "dataGridViewResults";
            dataGridViewResults.ReadOnly = true;
            dataGridViewResults.RowHeadersVisible = false;
            dataGridViewResults.RowHeadersWidth = 82;
            dataGridViewResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewResults.Size = new Size(1235, 905);
            dataGridViewResults.TabIndex = 1;
            dataGridViewResults.SelectionChanged += dataGridViewResults_SelectionChanged;
            // 
            // columnTeamA
            // 
            columnTeamA.HeaderText = "Team A";
            columnTeamA.MinimumWidth = 10;
            columnTeamA.Name = "columnTeamA";
            columnTeamA.ReadOnly = true;
            // 
            // columnTeamB
            // 
            columnTeamB.HeaderText = "Team B";
            columnTeamB.MinimumWidth = 10;
            columnTeamB.Name = "columnTeamB";
            columnTeamB.ReadOnly = true;
            // 
            // columnScheduled
            // 
            columnScheduled.HeaderText = "Scheduled";
            columnScheduled.MinimumWidth = 10;
            columnScheduled.Name = "columnScheduled";
            columnScheduled.ReadOnly = true;
            // 
            // columnScore
            // 
            columnScore.HeaderText = "Score";
            columnScore.MinimumWidth = 10;
            columnScore.Name = "columnScore";
            columnScore.ReadOnly = true;
            // 
            // columnWinner
            // 
            columnWinner.HeaderText = "Winner";
            columnWinner.MinimumWidth = 10;
            columnWinner.Name = "columnWinner";
            columnWinner.ReadOnly = true;
            // 
            // columnStatus
            // 
            columnStatus.HeaderText = "Status";
            columnStatus.MinimumWidth = 10;
            columnStatus.Name = "columnStatus";
            columnStatus.ReadOnly = true;
            // 
            // ResultsPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutMain);
            Name = "ResultsPage";
            Size = new Size(1241, 1021);
            tableLayoutMain.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericScoreA).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericScoreB).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewResults).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutMain;
        private Panel panel1;
        private Button buttonRefresh;
        private Button buttonSetResult;
        private NumericUpDown numericScoreB;
        private NumericUpDown numericScoreA;
        private Label labelSelectedMatch;
        private DataGridView dataGridViewResults;
        private DataGridViewTextBoxColumn columnTeamA;
        private DataGridViewTextBoxColumn columnTeamB;
        private DataGridViewTextBoxColumn columnScheduled;
        private DataGridViewTextBoxColumn columnScore;
        private DataGridViewTextBoxColumn columnWinner;
        private DataGridViewTextBoxColumn columnStatus;
    }
}
