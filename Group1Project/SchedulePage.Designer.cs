namespace Group1Project
{
    partial class SchedulePage
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
            buttonRefresh = new Button();
            buttonSetSchedule = new Button();
            dateTimePickerStart = new DateTimePicker();
            dataGridViewSchedule = new DataGridView();
            columnTeamA = new DataGridViewTextBoxColumn();
            columnTeamB = new DataGridViewTextBoxColumn();
            columnScheduled = new DataGridViewTextBoxColumn();
            columnStatus = new DataGridViewTextBoxColumn();
            columnScore = new DataGridViewTextBoxColumn();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSchedule).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(dataGridViewSchedule, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1320, 1034);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonRefresh);
            panel1.Controls.Add(buttonSetSchedule);
            panel1.Controls.Add(dateTimePickerStart);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1314, 64);
            panel1.TabIndex = 1;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(1149, 15);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(150, 46);
            buttonRefresh.TabIndex = 2;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // buttonSetSchedule
            // 
            buttonSetSchedule.Location = new Point(432, 18);
            buttonSetSchedule.Name = "buttonSetSchedule";
            buttonSetSchedule.Size = new Size(150, 46);
            buttonSetSchedule.TabIndex = 1;
            buttonSetSchedule.Text = "Set";
            buttonSetSchedule.UseVisualStyleBackColor = true;
            buttonSetSchedule.Click += buttonSetSchedule_Click;
            // 
            // dateTimePickerStart
            // 
            dateTimePickerStart.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.Location = new Point(19, 22);
            dateTimePickerStart.Name = "dateTimePickerStart";
            dateTimePickerStart.ShowUpDown = true;
            dateTimePickerStart.Size = new Size(398, 39);
            dateTimePickerStart.TabIndex = 0;
            // 
            // dataGridViewSchedule
            // 
            dataGridViewSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSchedule.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSchedule.Columns.AddRange(new DataGridViewColumn[] { columnTeamA, columnTeamB, columnScheduled, columnStatus, columnScore });
            dataGridViewSchedule.Dock = DockStyle.Fill;
            dataGridViewSchedule.Location = new Point(3, 73);
            dataGridViewSchedule.Name = "dataGridViewSchedule";
            dataGridViewSchedule.ReadOnly = true;
            dataGridViewSchedule.RowHeadersVisible = false;
            dataGridViewSchedule.RowHeadersWidth = 82;
            dataGridViewSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSchedule.Size = new Size(1314, 958);
            dataGridViewSchedule.TabIndex = 2;
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
            // columnStatus
            // 
            columnStatus.HeaderText = "Status";
            columnStatus.MinimumWidth = 10;
            columnStatus.Name = "columnStatus";
            columnStatus.ReadOnly = true;
            // 
            // columnScore
            // 
            columnScore.HeaderText = "Score";
            columnScore.MinimumWidth = 10;
            columnScore.Name = "columnScore";
            columnScore.ReadOnly = true;
            // 
            // SchedulePage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Name = "SchedulePage";
            Size = new Size(1320, 1034);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewSchedule).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private DateTimePicker dateTimePickerStart;
        private Panel panel1;
        private Button buttonRefresh;
        private Button buttonSetSchedule;
        private DataGridView dataGridViewSchedule;
        private DataGridViewTextBoxColumn columnTeamA;
        private DataGridViewTextBoxColumn columnTeamB;
        private DataGridViewTextBoxColumn columnScheduled;
        private DataGridViewTextBoxColumn columnStatus;
        private DataGridViewTextBoxColumn columnScore;
    }
}
