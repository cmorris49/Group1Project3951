using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Group1Project
{
    public partial class BracketPage : UserControl
    {
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
