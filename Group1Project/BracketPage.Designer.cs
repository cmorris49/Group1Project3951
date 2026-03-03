namespace Group1Project
{
    partial class BracketPage
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
            tableMain = new TableLayoutPanel();
            panelHeader = new Panel();
            comboBoxView = new ComboBox();
            comboBoxStage = new ComboBox();
            buttonResetBracket = new Button();
            buttonGenerateBracket = new Button();
            buttonExportBracket = new Button();
            labelStage = new Label();
            panelViewHost = new Panel();
            dataGridViewStageMatches = new DataGridView();
            panelBracketContainer = new Panel();
            flowRounds = new FlowLayoutPanel();
            panelRound1 = new Panel();
            panel6 = new Panel();
            label9 = new Label();
            label10 = new Label();
            panel5 = new Panel();
            label7 = new Label();
            label8 = new Label();
            panel4 = new Panel();
            label5 = new Label();
            label6 = new Label();
            panel3 = new Panel();
            label3 = new Label();
            label4 = new Label();
            panel2 = new Panel();
            label1 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            labelTeamB = new Label();
            labelTeamA = new Label();
            labelRound = new Label();
            panelRound2 = new Panel();
            panel8 = new Panel();
            label11 = new Label();
            label12 = new Label();
            panel9 = new Panel();
            label13 = new Label();
            label14 = new Label();
            panel10 = new Panel();
            label15 = new Label();
            label16 = new Label();
            panel11 = new Panel();
            label17 = new Label();
            label18 = new Label();
            panel12 = new Panel();
            label19 = new Label();
            label20 = new Label();
            panel13 = new Panel();
            label21 = new Label();
            label22 = new Label();
            label23 = new Label();
            panelRound3 = new Panel();
            panel15 = new Panel();
            label24 = new Label();
            label25 = new Label();
            panel16 = new Panel();
            label26 = new Label();
            label27 = new Label();
            panel17 = new Panel();
            label28 = new Label();
            label29 = new Label();
            panel18 = new Panel();
            label30 = new Label();
            label31 = new Label();
            panel19 = new Panel();
            label32 = new Label();
            label33 = new Label();
            panel20 = new Panel();
            label34 = new Label();
            label35 = new Label();
            label36 = new Label();
            tableMain.SuspendLayout();
            panelHeader.SuspendLayout();
            panelViewHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewStageMatches).BeginInit();
            panelBracketContainer.SuspendLayout();
            flowRounds.SuspendLayout();
            panelRound1.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panelRound2.SuspendLayout();
            panel8.SuspendLayout();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
            panel11.SuspendLayout();
            panel12.SuspendLayout();
            panel13.SuspendLayout();
            panelRound3.SuspendLayout();
            panel15.SuspendLayout();
            panel16.SuspendLayout();
            panel17.SuspendLayout();
            panel18.SuspendLayout();
            panel19.SuspendLayout();
            panel20.SuspendLayout();
            SuspendLayout();
            // 
            // tableMain
            // 
            tableMain.ColumnCount = 1;
            tableMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableMain.Controls.Add(panelHeader, 0, 0);
            tableMain.Controls.Add(panelViewHost, 0, 1);
            tableMain.Dock = DockStyle.Fill;
            tableMain.Location = new Point(0, 0);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 1;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableMain.Size = new Size(1436, 986);
            tableMain.TabIndex = 0;
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(comboBoxView);
            panelHeader.Controls.Add(comboBoxStage);
            panelHeader.Controls.Add(buttonResetBracket);
            panelHeader.Controls.Add(buttonGenerateBracket);
            panelHeader.Controls.Add(buttonExportBracket);
            panelHeader.Controls.Add(labelStage);
            panelHeader.Dock = DockStyle.Fill;
            panelHeader.Location = new Point(3, 3);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1430, 74);
            panelHeader.TabIndex = 0;
            // 
            // comboBoxView
            // 
            comboBoxView.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxView.FormattingEnabled = true;
            comboBoxView.Items.AddRange(new object[] { "Bracket", "Matches" });
            comboBoxView.Location = new Point(406, 24);
            comboBoxView.Name = "comboBoxView";
            comboBoxView.Size = new Size(242, 40);
            comboBoxView.TabIndex = 5;
            // 
            // comboBoxStage
            // 
            comboBoxStage.FormattingEnabled = true;
            comboBoxStage.Location = new Point(160, 24);
            comboBoxStage.Name = "comboBoxStage";
            comboBoxStage.Size = new Size(240, 40);
            comboBoxStage.TabIndex = 4;
            // 
            // buttonResetBracket
            // 
            buttonResetBracket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonResetBracket.Location = new Point(1169, 15);
            buttonResetBracket.Name = "buttonResetBracket";
            buttonResetBracket.Size = new Size(120, 45);
            buttonResetBracket.TabIndex = 3;
            buttonResetBracket.Text = "Reset";
            buttonResetBracket.UseVisualStyleBackColor = true;
            // 
            // buttonGenerateBracket
            // 
            buttonGenerateBracket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonGenerateBracket.Location = new Point(1043, 15);
            buttonGenerateBracket.Name = "buttonGenerateBracket";
            buttonGenerateBracket.Size = new Size(120, 45);
            buttonGenerateBracket.TabIndex = 2;
            buttonGenerateBracket.Text = "Generate";
            buttonGenerateBracket.UseVisualStyleBackColor = true;
            // 
            // buttonExportBracket
            // 
            buttonExportBracket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonExportBracket.Location = new Point(1295, 15);
            buttonExportBracket.Name = "buttonExportBracket";
            buttonExportBracket.Size = new Size(120, 45);
            buttonExportBracket.TabIndex = 1;
            buttonExportBracket.Text = "Export";
            buttonExportBracket.UseVisualStyleBackColor = true;
            // 
            // labelStage
            // 
            labelStage.AutoSize = true;
            labelStage.Font = new Font("Segoe UI", 16F);
            labelStage.Location = new Point(12, 10);
            labelStage.Name = "labelStage";
            labelStage.Size = new Size(140, 59);
            labelStage.TabIndex = 0;
            labelStage.Text = "Stage:";
            // 
            // panelViewHost
            // 
            panelViewHost.Controls.Add(dataGridViewStageMatches);
            panelViewHost.Controls.Add(panelBracketContainer);
            panelViewHost.Dock = DockStyle.Fill;
            panelViewHost.Location = new Point(3, 83);
            panelViewHost.Name = "panelViewHost";
            panelViewHost.Size = new Size(1430, 900);
            panelViewHost.TabIndex = 1;
            // 
            // dataGridViewStageMatches
            // 
            dataGridViewStageMatches.AllowUserToAddRows = false;
            dataGridViewStageMatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewStageMatches.BackgroundColor = SystemColors.ActiveCaption;
            dataGridViewStageMatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewStageMatches.Dock = DockStyle.Fill;
            dataGridViewStageMatches.Location = new Point(0, 0);
            dataGridViewStageMatches.MultiSelect = false;
            dataGridViewStageMatches.Name = "dataGridViewStageMatches";
            dataGridViewStageMatches.ReadOnly = true;
            dataGridViewStageMatches.RowHeadersVisible = false;
            dataGridViewStageMatches.RowHeadersWidth = 82;
            dataGridViewStageMatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewStageMatches.Size = new Size(1430, 900);
            dataGridViewStageMatches.TabIndex = 1;
            dataGridViewStageMatches.Visible = false;
            // 
            // panelBracketContainer
            // 
            panelBracketContainer.AutoScroll = true;
            panelBracketContainer.Controls.Add(flowRounds);
            panelBracketContainer.Dock = DockStyle.Fill;
            panelBracketContainer.Location = new Point(0, 0);
            panelBracketContainer.Name = "panelBracketContainer";
            panelBracketContainer.Size = new Size(1430, 900);
            panelBracketContainer.TabIndex = 1;
            panelBracketContainer.Visible = false;
            // 
            // flowRounds
            // 
            flowRounds.Controls.Add(panelRound1);
            flowRounds.Controls.Add(panelRound2);
            flowRounds.Controls.Add(panelRound3);
            flowRounds.Dock = DockStyle.Fill;
            flowRounds.Location = new Point(0, 0);
            flowRounds.Name = "flowRounds";
            flowRounds.Size = new Size(1430, 900);
            flowRounds.TabIndex = 0;
            flowRounds.WrapContents = false;
            // 
            // panelRound1
            // 
            panelRound1.Controls.Add(panel6);
            panelRound1.Controls.Add(panel5);
            panelRound1.Controls.Add(panel4);
            panelRound1.Controls.Add(panel3);
            panelRound1.Controls.Add(panel2);
            panelRound1.Controls.Add(panel1);
            panelRound1.Controls.Add(labelRound);
            panelRound1.Location = new Point(20, 20);
            panelRound1.Margin = new Padding(20);
            panelRound1.Name = "panelRound1";
            panelRound1.Size = new Size(200, 600);
            panelRound1.TabIndex = 1;
            // 
            // panel6
            // 
            panel6.Controls.Add(label9);
            panel6.Controls.Add(label10);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 482);
            panel6.Name = "panel6";
            panel6.Size = new Size(200, 90);
            panel6.TabIndex = 6;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(102, 26);
            label9.Name = "label9";
            label9.Size = new Size(92, 32);
            label9.TabIndex = 1;
            label9.Text = "Team B";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(3, 26);
            label10.Name = "label10";
            label10.Size = new Size(93, 32);
            label10.TabIndex = 0;
            label10.Text = "Team A";
            // 
            // panel5
            // 
            panel5.Controls.Add(label7);
            panel5.Controls.Add(label8);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 392);
            panel5.Name = "panel5";
            panel5.Size = new Size(200, 90);
            panel5.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(102, 26);
            label7.Name = "label7";
            label7.Size = new Size(92, 32);
            label7.TabIndex = 1;
            label7.Text = "Team B";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(3, 26);
            label8.Name = "label8";
            label8.Size = new Size(93, 32);
            label8.TabIndex = 0;
            label8.Text = "Team A";
            // 
            // panel4
            // 
            panel4.Controls.Add(label5);
            panel4.Controls.Add(label6);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 302);
            panel4.Name = "panel4";
            panel4.Size = new Size(200, 90);
            panel4.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(102, 26);
            label5.Name = "label5";
            label5.Size = new Size(92, 32);
            label5.TabIndex = 1;
            label5.Text = "Team B";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(3, 26);
            label6.Name = "label6";
            label6.Size = new Size(93, 32);
            label6.TabIndex = 0;
            label6.Text = "Team A";
            // 
            // panel3
            // 
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label4);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 212);
            panel3.Name = "panel3";
            panel3.Size = new Size(200, 90);
            panel3.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(102, 26);
            label3.Name = "label3";
            label3.Size = new Size(92, 32);
            label3.TabIndex = 1;
            label3.Text = "Team B";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 26);
            label4.Name = "label4";
            label4.Size = new Size(93, 32);
            label4.TabIndex = 0;
            label4.Text = "Team A";
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 122);
            panel2.Name = "panel2";
            panel2.Size = new Size(200, 90);
            panel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(102, 26);
            label1.Name = "label1";
            label1.Size = new Size(92, 32);
            label1.TabIndex = 1;
            label1.Text = "Team B";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 26);
            label2.Name = "label2";
            label2.Size = new Size(93, 32);
            label2.TabIndex = 0;
            label2.Text = "Team A";
            // 
            // panel1
            // 
            panel1.Controls.Add(labelTeamB);
            panel1.Controls.Add(labelTeamA);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 32);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 90);
            panel1.TabIndex = 1;
            // 
            // labelTeamB
            // 
            labelTeamB.AutoSize = true;
            labelTeamB.Location = new Point(102, 26);
            labelTeamB.Name = "labelTeamB";
            labelTeamB.Size = new Size(92, 32);
            labelTeamB.TabIndex = 1;
            labelTeamB.Text = "Team B";
            // 
            // labelTeamA
            // 
            labelTeamA.AutoSize = true;
            labelTeamA.Location = new Point(3, 26);
            labelTeamA.Name = "labelTeamA";
            labelTeamA.Size = new Size(93, 32);
            labelTeamA.TabIndex = 0;
            labelTeamA.Text = "Team A";
            // 
            // labelRound
            // 
            labelRound.AutoSize = true;
            labelRound.Dock = DockStyle.Top;
            labelRound.Location = new Point(0, 0);
            labelRound.Name = "labelRound";
            labelRound.Size = new Size(103, 32);
            labelRound.TabIndex = 0;
            labelRound.Text = "Round 1";
            // 
            // panelRound2
            // 
            panelRound2.Controls.Add(panel8);
            panelRound2.Controls.Add(panel9);
            panelRound2.Controls.Add(panel10);
            panelRound2.Controls.Add(panel11);
            panelRound2.Controls.Add(panel12);
            panelRound2.Controls.Add(panel13);
            panelRound2.Controls.Add(label23);
            panelRound2.Location = new Point(260, 20);
            panelRound2.Margin = new Padding(20);
            panelRound2.Name = "panelRound2";
            panelRound2.Size = new Size(200, 600);
            panelRound2.TabIndex = 2;
            // 
            // panel8
            // 
            panel8.Controls.Add(label11);
            panel8.Controls.Add(label12);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 482);
            panel8.Name = "panel8";
            panel8.Size = new Size(200, 90);
            panel8.TabIndex = 6;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(102, 26);
            label11.Name = "label11";
            label11.Size = new Size(92, 32);
            label11.TabIndex = 1;
            label11.Text = "Team B";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 26);
            label12.Name = "label12";
            label12.Size = new Size(93, 32);
            label12.TabIndex = 0;
            label12.Text = "Team A";
            // 
            // panel9
            // 
            panel9.Controls.Add(label13);
            panel9.Controls.Add(label14);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(0, 392);
            panel9.Name = "panel9";
            panel9.Size = new Size(200, 90);
            panel9.TabIndex = 5;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(102, 26);
            label13.Name = "label13";
            label13.Size = new Size(92, 32);
            label13.TabIndex = 1;
            label13.Text = "Team B";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(3, 26);
            label14.Name = "label14";
            label14.Size = new Size(93, 32);
            label14.TabIndex = 0;
            label14.Text = "Team A";
            // 
            // panel10
            // 
            panel10.Controls.Add(label15);
            panel10.Controls.Add(label16);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(0, 302);
            panel10.Name = "panel10";
            panel10.Size = new Size(200, 90);
            panel10.TabIndex = 4;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(102, 26);
            label15.Name = "label15";
            label15.Size = new Size(92, 32);
            label15.TabIndex = 1;
            label15.Text = "Team B";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(3, 26);
            label16.Name = "label16";
            label16.Size = new Size(93, 32);
            label16.TabIndex = 0;
            label16.Text = "Team A";
            // 
            // panel11
            // 
            panel11.Controls.Add(label17);
            panel11.Controls.Add(label18);
            panel11.Dock = DockStyle.Top;
            panel11.Location = new Point(0, 212);
            panel11.Name = "panel11";
            panel11.Size = new Size(200, 90);
            panel11.TabIndex = 3;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(102, 26);
            label17.Name = "label17";
            label17.Size = new Size(92, 32);
            label17.TabIndex = 1;
            label17.Text = "Team B";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(3, 26);
            label18.Name = "label18";
            label18.Size = new Size(93, 32);
            label18.TabIndex = 0;
            label18.Text = "Team A";
            // 
            // panel12
            // 
            panel12.Controls.Add(label19);
            panel12.Controls.Add(label20);
            panel12.Dock = DockStyle.Top;
            panel12.Location = new Point(0, 122);
            panel12.Name = "panel12";
            panel12.Size = new Size(200, 90);
            panel12.TabIndex = 2;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(102, 26);
            label19.Name = "label19";
            label19.Size = new Size(92, 32);
            label19.TabIndex = 1;
            label19.Text = "Team B";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(3, 26);
            label20.Name = "label20";
            label20.Size = new Size(93, 32);
            label20.TabIndex = 0;
            label20.Text = "Team A";
            // 
            // panel13
            // 
            panel13.Controls.Add(label21);
            panel13.Controls.Add(label22);
            panel13.Dock = DockStyle.Top;
            panel13.Location = new Point(0, 32);
            panel13.Name = "panel13";
            panel13.Size = new Size(200, 90);
            panel13.TabIndex = 1;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(102, 26);
            label21.Name = "label21";
            label21.Size = new Size(92, 32);
            label21.TabIndex = 1;
            label21.Text = "Team B";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(3, 26);
            label22.Name = "label22";
            label22.Size = new Size(93, 32);
            label22.TabIndex = 0;
            label22.Text = "Team A";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Dock = DockStyle.Top;
            label23.Location = new Point(0, 0);
            label23.Name = "label23";
            label23.Size = new Size(103, 32);
            label23.TabIndex = 0;
            label23.Text = "Round 2";
            // 
            // panelRound3
            // 
            panelRound3.Controls.Add(panel15);
            panelRound3.Controls.Add(panel16);
            panelRound3.Controls.Add(panel17);
            panelRound3.Controls.Add(panel18);
            panelRound3.Controls.Add(panel19);
            panelRound3.Controls.Add(panel20);
            panelRound3.Controls.Add(label36);
            panelRound3.Location = new Point(500, 20);
            panelRound3.Margin = new Padding(20);
            panelRound3.Name = "panelRound3";
            panelRound3.Size = new Size(200, 600);
            panelRound3.TabIndex = 3;
            // 
            // panel15
            // 
            panel15.Controls.Add(label24);
            panel15.Controls.Add(label25);
            panel15.Dock = DockStyle.Top;
            panel15.Location = new Point(0, 482);
            panel15.Name = "panel15";
            panel15.Size = new Size(200, 90);
            panel15.TabIndex = 6;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(102, 26);
            label24.Name = "label24";
            label24.Size = new Size(92, 32);
            label24.TabIndex = 1;
            label24.Text = "Team B";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(3, 26);
            label25.Name = "label25";
            label25.Size = new Size(93, 32);
            label25.TabIndex = 0;
            label25.Text = "Team A";
            // 
            // panel16
            // 
            panel16.Controls.Add(label26);
            panel16.Controls.Add(label27);
            panel16.Dock = DockStyle.Top;
            panel16.Location = new Point(0, 392);
            panel16.Name = "panel16";
            panel16.Size = new Size(200, 90);
            panel16.TabIndex = 5;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(102, 26);
            label26.Name = "label26";
            label26.Size = new Size(92, 32);
            label26.TabIndex = 1;
            label26.Text = "Team B";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(3, 26);
            label27.Name = "label27";
            label27.Size = new Size(93, 32);
            label27.TabIndex = 0;
            label27.Text = "Team A";
            // 
            // panel17
            // 
            panel17.Controls.Add(label28);
            panel17.Controls.Add(label29);
            panel17.Dock = DockStyle.Top;
            panel17.Location = new Point(0, 302);
            panel17.Name = "panel17";
            panel17.Size = new Size(200, 90);
            panel17.TabIndex = 4;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(102, 26);
            label28.Name = "label28";
            label28.Size = new Size(92, 32);
            label28.TabIndex = 1;
            label28.Text = "Team B";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(3, 26);
            label29.Name = "label29";
            label29.Size = new Size(93, 32);
            label29.TabIndex = 0;
            label29.Text = "Team A";
            // 
            // panel18
            // 
            panel18.Controls.Add(label30);
            panel18.Controls.Add(label31);
            panel18.Dock = DockStyle.Top;
            panel18.Location = new Point(0, 212);
            panel18.Name = "panel18";
            panel18.Size = new Size(200, 90);
            panel18.TabIndex = 3;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(102, 26);
            label30.Name = "label30";
            label30.Size = new Size(92, 32);
            label30.TabIndex = 1;
            label30.Text = "Team B";
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(3, 26);
            label31.Name = "label31";
            label31.Size = new Size(93, 32);
            label31.TabIndex = 0;
            label31.Text = "Team A";
            // 
            // panel19
            // 
            panel19.Controls.Add(label32);
            panel19.Controls.Add(label33);
            panel19.Dock = DockStyle.Top;
            panel19.Location = new Point(0, 122);
            panel19.Name = "panel19";
            panel19.Size = new Size(200, 90);
            panel19.TabIndex = 2;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(102, 26);
            label32.Name = "label32";
            label32.Size = new Size(92, 32);
            label32.TabIndex = 1;
            label32.Text = "Team B";
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(3, 26);
            label33.Name = "label33";
            label33.Size = new Size(93, 32);
            label33.TabIndex = 0;
            label33.Text = "Team A";
            // 
            // panel20
            // 
            panel20.Controls.Add(label34);
            panel20.Controls.Add(label35);
            panel20.Dock = DockStyle.Top;
            panel20.Location = new Point(0, 32);
            panel20.Name = "panel20";
            panel20.Size = new Size(200, 90);
            panel20.TabIndex = 1;
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(102, 26);
            label34.Name = "label34";
            label34.Size = new Size(92, 32);
            label34.TabIndex = 1;
            label34.Text = "Team B";
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new Point(3, 26);
            label35.Name = "label35";
            label35.Size = new Size(93, 32);
            label35.TabIndex = 0;
            label35.Text = "Team A";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Dock = DockStyle.Top;
            label36.Location = new Point(0, 0);
            label36.Name = "label36";
            label36.Size = new Size(103, 32);
            label36.TabIndex = 0;
            label36.Text = "Round 3";
            // 
            // BracketPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableMain);
            Name = "BracketPage";
            Size = new Size(1436, 986);
            tableMain.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelViewHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewStageMatches).EndInit();
            panelBracketContainer.ResumeLayout(false);
            flowRounds.ResumeLayout(false);
            panelRound1.ResumeLayout(false);
            panelRound1.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelRound2.ResumeLayout(false);
            panelRound2.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panelRound3.ResumeLayout(false);
            panelRound3.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            panel17.ResumeLayout(false);
            panel17.PerformLayout();
            panel18.ResumeLayout(false);
            panel18.PerformLayout();
            panel19.ResumeLayout(false);
            panel19.PerformLayout();
            panel20.ResumeLayout(false);
            panel20.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableMain;
        private Panel panelHeader;
        private Label labelStage;
        private Button buttonResetBracket;
        private Button buttonGenerateBracket;
        private Button buttonExportBracket;
        private ComboBox comboBoxStage;
        private ComboBox comboBoxView;
        private Panel panelViewHost;
        private DataGridView dataGridViewStageMatches;
        private Panel panelBracketContainer;
        private FlowLayoutPanel flowRounds;
        private Panel panelRound1;
        private Panel panel6;
        private Label label9;
        private Label label10;
        private Panel panel5;
        private Label label7;
        private Label label8;
        private Panel panel4;
        private Label label5;
        private Label label6;
        private Panel panel3;
        private Label label3;
        private Label label4;
        private Panel panel2;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private Label labelTeamB;
        private Label labelTeamA;
        private Label labelRound;
        private Panel panelRound2;
        private Panel panel8;
        private Label label11;
        private Label label12;
        private Panel panel9;
        private Label label13;
        private Label label14;
        private Panel panel10;
        private Label label15;
        private Label label16;
        private Panel panel11;
        private Label label17;
        private Label label18;
        private Panel panel12;
        private Label label19;
        private Label label20;
        private Panel panel13;
        private Label label21;
        private Label label22;
        private Label label23;
        private Panel panelRound3;
        private Panel panel15;
        private Label label24;
        private Label label25;
        private Panel panel16;
        private Label label26;
        private Label label27;
        private Panel panel17;
        private Label label28;
        private Label label29;
        private Panel panel18;
        private Label label30;
        private Label label31;
        private Panel panel19;
        private Label label32;
        private Label label33;
        private Panel panel20;
        private Label label34;
        private Label label35;
        private Label label36;
    }
}
