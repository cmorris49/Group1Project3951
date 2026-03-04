using System.Windows.Forms;

/// <summary>
/// Group 1 Project - addTeam Form designer
/// Author: Jonathan,Cameron 
/// Date: March 2, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents a Windows Form that allows users to add a new team by entering a team name and confirming or
    /// canceling the operation.
    /// </summary>
    partial class addTeam
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConfirm = new Button();
            btnCancel = new Button();
            teamNameTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            txtPlayerName = new TextBox();
            btnAddPlayer = new Button();
            btnRemovePlayer = new Button();
            numPlayerNumber = new NumericUpDown();
            lblPlayerNumber = new Label();
            lstPlayers = new ListBox();
            ((System.ComponentModel.ISupportInitialize)numPlayerNumber).BeginInit();
            SuspendLayout();
            // 
            // btnConfirm
            // 
            btnConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnConfirm.BackColor = Color.LightGreen;
            btnConfirm.Location = new Point(588, 660);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(138, 55);
            btnConfirm.TabIndex = 0;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.BackColor = Color.Salmon;
            btnCancel.Location = new Point(732, 660);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(144, 55);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // teamNameTextBox
            // 
            teamNameTextBox.Location = new Point(172, 28);
            teamNameTextBox.Name = "teamNameTextBox";
            teamNameTextBox.Size = new Size(311, 39);
            teamNameTextBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 28);
            label1.Name = "label1";
            label1.Size = new Size(143, 32);
            label1.TabIndex = 3;
            label1.Text = "Team name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 127);
            label2.Name = "label2";
            label2.Size = new Size(154, 32);
            label2.TabIndex = 5;
            label2.Text = "Player Name:";
            // 
            // txtPlayerName
            // 
            txtPlayerName.Location = new Point(172, 127);
            txtPlayerName.Name = "txtPlayerName";
            txtPlayerName.Size = new Size(311, 39);
            txtPlayerName.TabIndex = 6;
            // 
            // btnAddPlayer
            // 
            btnAddPlayer.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAddPlayer.BackColor = Color.LightGreen;
            btnAddPlayer.Location = new Point(489, 119);
            btnAddPlayer.Name = "btnAddPlayer";
            btnAddPlayer.Size = new Size(173, 55);
            btnAddPlayer.TabIndex = 7;
            btnAddPlayer.Text = "Add Player";
            btnAddPlayer.UseVisualStyleBackColor = false;
            btnAddPlayer.Click += btnAddPlayer_Click;
            // 
            // btnRemovePlayer
            // 
            btnRemovePlayer.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRemovePlayer.BackColor = Color.Salmon;
            btnRemovePlayer.Location = new Point(668, 119);
            btnRemovePlayer.Name = "btnRemovePlayer";
            btnRemovePlayer.Size = new Size(184, 55);
            btnRemovePlayer.TabIndex = 8;
            btnRemovePlayer.Text = "Remove Player";
            btnRemovePlayer.UseVisualStyleBackColor = false;
            btnRemovePlayer.Click += btnRemovePlayer_Click;
            // 
            // numPlayerNumber
            // 
            numPlayerNumber.Location = new Point(172, 203);
            numPlayerNumber.Name = "numPlayerNumber";
            numPlayerNumber.Size = new Size(240, 39);
            numPlayerNumber.TabIndex = 9;
            // 
            // lblPlayerNumber
            // 
            lblPlayerNumber.AutoSize = true;
            lblPlayerNumber.Location = new Point(12, 205);
            lblPlayerNumber.Name = "lblPlayerNumber";
            lblPlayerNumber.Size = new Size(143, 32);
            lblPlayerNumber.TabIndex = 10;
            lblPlayerNumber.Text = "Player Num:";
            // 
            // lstPlayers
            // 
            lstPlayers.FormattingEnabled = true;
            lstPlayers.Location = new Point(12, 256);
            lstPlayers.Name = "lstPlayers";
            lstPlayers.Size = new Size(864, 388);
            lstPlayers.TabIndex = 11;
            // 
            // addTeam
            // 
            AcceptButton = btnConfirm;
            ClientSize = new Size(888, 727);
            Controls.Add(lstPlayers);
            Controls.Add(lblPlayerNumber);
            Controls.Add(numPlayerNumber);
            Controls.Add(btnRemovePlayer);
            Controls.Add(btnAddPlayer);
            Controls.Add(txtPlayerName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(teamNameTextBox);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "addTeam";
            Text = "Add Team";
            ((System.ComponentModel.ISupportInitialize)numPlayerNumber).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button btnConfirm;
        private Button btnCancel;
        private TextBox teamNameTextBox;
        private Label label1;
        private Label label2;
        private TextBox txtPlayerName;
        private Button btnAddPlayer;
        private Button btnRemovePlayer;
        private NumericUpDown numPlayerNumber;
        private Label lblPlayerNumber;
        private ListBox lstPlayers;
    }
}