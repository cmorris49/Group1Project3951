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

namespace Group1Project
{
    public partial class NewTournamentForm : Form
    {
        internal Tournament? CreatedTournament { get; private set; }

        public NewTournamentForm()
        {
            InitializeComponent();
            dateTimePickerStartDate.Value = DateTime.Today;
            buttonCreate.Click += BtnOk_Click;
        }

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
