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
/// Group 1 Project - BracketPage UserControl
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    
    public partial class BracketPage : UserControl
    {
        /// <summary>
        /// Represents a user control that displays tournament progression, allowing users to toggle 
        /// between a tabular view of matches and a graphical bracket view.
        /// </summary>
        public BracketPage()
        {
            InitializeComponent();

            comboBoxView.SelectedIndexChanged += (s, e) =>
            {
                var view = comboBoxView.SelectedItem?.ToString() ?? "Matches";
                ShowView(view);
            };

            if (comboBoxView.Items.Count > 0)
                comboBoxView.SelectedIndex = 0;

            ShowView("Matches");
        }

        /// <summary>
        /// Toggles the visibility of the user control's containers to display the requested view.
        /// </summary>
        /// <param name="viewName">The name of the view to display. Expected values are "Matches" to show the DataGridView, or "Bracket" to show the visual panel.</param>
        private void ShowView(string viewName)
        {
            dataGridViewStageMatches.Visible = false;
            panelBracketContainer.Visible = false;

            switch (viewName)
            {
                case "Matches":
                    dataGridViewStageMatches.Visible = true;
                    dataGridViewStageMatches.BringToFront();
                    break;

                case "Bracket":
                    panelBracketContainer.Visible = true;
                    panelBracketContainer.BringToFront();
                    break;

                default:
                    dataGridViewStageMatches.Visible = true;
                    dataGridViewStageMatches.BringToFront();
                    break;
            }
        }
    }
}
