using System.Windows.Forms;

/// <summary>
/// Group 1 Project - addTeam Form
/// Author: Jonathan 
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
            SuspendLayout();
            // 
            // btnConfirm
            // 
            btnConfirm.BackColor = Color.LightGreen;
            btnConfirm.Location = new Point(310, 208);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(85, 28);
            btnConfirm.TabIndex = 0;
            btnConfirm.Text = "Confirm";
            btnConfirm.UseVisualStyleBackColor = false;
            btnConfirm.Click += btnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Salmon;
            btnCancel.Location = new Point(73, 208);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(85, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // teamNameTextBox
            // 
            teamNameTextBox.Location = new Point(128, 59);
            teamNameTextBox.Name = "teamNameTextBox";
            teamNameTextBox.Size = new Size(311, 25);
            teamNameTextBox.Text = "";
            teamNameTextBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 67);
            label1.Name = "label1";
            label1.Size = new Size(78, 17);
            label1.TabIndex = 3;
            label1.Text = "Team name:";
            // 
            // addTeam
            // 
            this.AcceptButton = btnConfirm;
            ClientSize = new Size(493, 271);
            Controls.Add(label1);
            Controls.Add(teamNameTextBox);
            Controls.Add(btnCancel);
            Controls.Add(btnConfirm);
            Name = "addTeam";
            Text = "Add Team";
            ResumeLayout(false);
            PerformLayout();
        }

        private Button btnConfirm;
        private Button btnCancel;
        private TextBox teamNameTextBox;
        private Label label1;
    }
}