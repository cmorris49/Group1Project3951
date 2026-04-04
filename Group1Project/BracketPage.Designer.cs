/// <summary>
/// Group 1 Project - BracketPage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
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
            buttonResetBracket = new Button();
            buttonGenerateBracket = new Button();
            buttonExportBracket = new Button();
            labelStage = new Label();
            panelViewHost = new Panel();
            dataGridViewStageMatches = new DataGridView();
            panelBracketContainer = new Panel();
            tableMain.SuspendLayout();
            panelHeader.SuspendLayout();
            panelViewHost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewStageMatches).BeginInit();
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
            tableMain.Margin = new Padding(2);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 1;
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 11F));
            tableMain.Size = new Size(773, 524);
            tableMain.TabIndex = 0;
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(comboBoxView);
            panelHeader.Controls.Add(buttonResetBracket);
            panelHeader.Controls.Add(buttonGenerateBracket);
            panelHeader.Controls.Add(buttonExportBracket);
            panelHeader.Controls.Add(labelStage);
            panelHeader.Dock = DockStyle.Fill;
            panelHeader.Location = new Point(2, 2);
            panelHeader.Margin = new Padding(2);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(769, 38);
            panelHeader.TabIndex = 0;
            // 
            // comboBoxView
            // 
            comboBoxView.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxView.FormattingEnabled = true;
            comboBoxView.Items.AddRange(new object[] { "Bracket", "Matches" });
            comboBoxView.Location = new Point(152, 10);
            comboBoxView.Name = "comboBoxView";
            comboBoxView.Size = new Size(132, 25);
            comboBoxView.TabIndex = 5;
            // 
            // buttonResetBracket
            // 
            buttonResetBracket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonResetBracket.Location = new Point(628, 8);
            buttonResetBracket.Margin = new Padding(2);
            buttonResetBracket.Name = "buttonResetBracket";
            buttonResetBracket.Size = new Size(65, 24);
            buttonResetBracket.TabIndex = 3;
            buttonResetBracket.Text = "Reset";
            buttonResetBracket.UseVisualStyleBackColor = true;
            // 
            // buttonGenerateBracket
            // 
            buttonGenerateBracket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonGenerateBracket.Location = new Point(561, 8);
            buttonGenerateBracket.Margin = new Padding(2);
            buttonGenerateBracket.Name = "buttonGenerateBracket";
            buttonGenerateBracket.Size = new Size(65, 24);
            buttonGenerateBracket.TabIndex = 2;
            buttonGenerateBracket.Text = "Generate";
            buttonGenerateBracket.UseVisualStyleBackColor = true;
            // 
            // buttonExportBracket
            // 
            buttonExportBracket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonExportBracket.Location = new Point(696, 8);
            buttonExportBracket.Margin = new Padding(2);
            buttonExportBracket.Name = "buttonExportBracket";
            buttonExportBracket.Size = new Size(65, 24);
            buttonExportBracket.TabIndex = 1;
            buttonExportBracket.Text = "Export";
            buttonExportBracket.UseVisualStyleBackColor = true;
            buttonExportBracket.Click += buttonExportBracket_Click;
            // 
            // labelStage
            // 
            labelStage.AutoSize = true;
            labelStage.Font = new Font("Segoe UI", 16F);
            labelStage.Location = new Point(6, 5);
            labelStage.Margin = new Padding(2, 0, 2, 0);
            labelStage.Name = "labelStage";
            labelStage.Size = new Size(155, 32);
            labelStage.TabIndex = 0;
            labelStage.Text = "Bracket Type:";
            // 
            // panelViewHost
            // 
            panelViewHost.Controls.Add(dataGridViewStageMatches);
            panelViewHost.Controls.Add(panelBracketContainer);
            panelViewHost.Dock = DockStyle.Fill;
            panelViewHost.Location = new Point(2, 44);
            panelViewHost.Margin = new Padding(2);
            panelViewHost.Name = "panelViewHost";
            panelViewHost.Size = new Size(769, 478);
            panelViewHost.TabIndex = 1;
            // 
            // dataGridViewStageMatches
            // 
            dataGridViewStageMatches.AllowUserToAddRows = false;
            dataGridViewStageMatches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewStageMatches.BackgroundColor = SystemColors.Control;
            dataGridViewStageMatches.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewStageMatches.Dock = DockStyle.Fill;
            dataGridViewStageMatches.Location = new Point(0, 0);
            dataGridViewStageMatches.Margin = new Padding(2);
            dataGridViewStageMatches.MultiSelect = false;
            dataGridViewStageMatches.Name = "dataGridViewStageMatches";
            dataGridViewStageMatches.ReadOnly = true;
            dataGridViewStageMatches.RowHeadersVisible = false;
            dataGridViewStageMatches.RowHeadersWidth = 82;
            dataGridViewStageMatches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewStageMatches.Size = new Size(769, 478);
            dataGridViewStageMatches.TabIndex = 1;
            dataGridViewStageMatches.Visible = false;
            // 
            // panelBracketContainer
            // 
            panelBracketContainer.AutoScroll = true;
            panelBracketContainer.Dock = DockStyle.Fill;
            panelBracketContainer.Location = new Point(0, 0);
            panelBracketContainer.Margin = new Padding(2);
            panelBracketContainer.Name = "panelBracketContainer";
            panelBracketContainer.Size = new Size(769, 478);
            panelBracketContainer.TabIndex = 1;
            panelBracketContainer.Visible = false;
            // 
            // BracketPage
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableMain);
            Margin = new Padding(2);
            Name = "BracketPage";
            Size = new Size(773, 524);
            tableMain.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelViewHost.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewStageMatches).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableMain;
        private Panel panelHeader;
        private Label labelStage;
        private Button buttonResetBracket;
        private Button buttonGenerateBracket;
        private Button buttonExportBracket;
        private ComboBox comboBoxView;
        private Panel panelViewHost;
        private DataGridView dataGridViewStageMatches;
        private Panel panelBracketContainer;
    }
}
