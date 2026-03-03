using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Represents a form that enables users to add a new team by entering team details and confirming or canceling the
    /// operation.
    /// </summary>
    public partial class addTeam : Form
    {
        /// <summary>
        /// Represents the team associated with this instance, or null if no team is assigned.
        /// </summary>
        private Team? team;

        /// <summary>
        /// Initializes a new instance of the addTeam class and sets up the user interface components.
        /// </summary>
        public addTeam()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Cancel button by clearing the current team selection.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Cancel button that was clicked.</param>
        /// <param name="e">An EventArgs instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetForm();
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the Confirm button to create a new team if one does not already exist.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Confirm button that was clicked.</param>
        /// <param name="e">An EventArgs instance containing the event data.</param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (team == null && !teamNameTextBox.Text.ToString().Equals("") )
            {
                team = new Team(teamNameTextBox.Text.ToString());
                this.Close();
            }
            else
            {
                MessageBox.Show("Error. Please renter team", "Team adding Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
            }
        }

        /// <summary>
        /// Clears the team name input field and resets the team selection to prepare the form for a new entry.
        /// </summary>
        private void ResetForm()
        {
            teamNameTextBox.Text = string.Empty;
            team = null;
        }
    }
}
