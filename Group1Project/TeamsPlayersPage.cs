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
/// Group 1 Project - TeamsPlayersPage UserControl
/// Author: Cameron
///         Jun
///         Jonathan 
/// Date: March 2, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents a user control for managing teams and their players within the application.
    /// </summary>
    public partial class TeamsPlayersPage : UserControl
    {
        public TeamsPlayersPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Add Team button by displaying a popup for adding a new team.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Add Team button.</param>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        private void buttonAddTeam_Click(object sender, EventArgs e)
        {
            addTeam addTeamForm = new addTeam();
            addTeamForm.ShowDialog();

            addTeamForm.Dispose();
        }
    }
}
