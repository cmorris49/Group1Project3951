using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

/// <summary>
/// Group 1 Project - NewTournamentForm From
/// Author: Cameron, Jun
/// Date: March 3, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    public partial class NewTournamentForm : Form
    {
        /// <summary>
        /// Gets the tournament created by this form, or null if the form was cancelled. Cannont be modified after the form is closed.
        /// </summary>
        internal Tournament? CreatedTournament { get; private set; }

        /// <summary>
        /// Initializes a new instance of the NewTournamentForm class and sets up the user interface components, including event handlers for form controls.
        /// </summary>
        public NewTournamentForm()
        {
            InitializeComponent();
            dateTimePickerStartDate.Value = DateTime.Today;
            buttonCreate.Click += BtnOk_Click;
        }

        /// <summary>
        /// the event handler for the "Create" button click event. It validates the user input for the tournament name, location, and start date, and if valid,
        /// creates a new Tournament object and assigns it to the CreatedTournament property. 
        /// If the tournament name is invalid (empty or whitespace), it shows an error message and prevents the form from closing.
        /// </summary>
        /// <param name="sender">The source of the event, typically when the create button</param>
        /// <param name="e"> An EventArgs instance containing the event data.</param>
        private void BtnOk_Click(object? sender, EventArgs e)
        {
            var name = textTournamentName.Text.Trim();
            var location = textLocation.Text.Trim();
            var startDate = dateTimePickerStartDate.Value.Date;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a tournament name.");
                DialogResult = DialogResult.None;
                return;
            }

            CreatedTournament = new Tournament(name, startDate, location);
        }
    }
}
