using System;
using System.Windows.Forms;

/// <summary>
/// Group 1 Project - EntityInputDialog Form
/// Author: Cameron, Jun, Jonathan
/// Date: April 6, 2026; Revision: 1.0
/// Source:
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents a reusable modal dialog used to collect or edit an entity name,
    /// and optionally a numeric value, for team/player workflows.
    /// </summary>
    public partial class EntityInputDialog : Form
    {
        /// <summary>
        /// Gets the trimmed name value entered by the user.
        /// </summary>
        public string EntityName
        {
            get
            {
                return textBoxName.Text.Trim();
            }
        }

        /// <summary>
        /// Gets the numeric value entered by the user.
        /// </summary>
        public int EntityNumber
        {
            get
            {
                return (int)numericNumber.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityInputDialog"/> class with configurable labels,
        /// initial values, and optional number input visibility.
        /// </summary>
        /// <param name="title">Dialog title text.</param>
        /// <param name="nameLabel">Label text shown for the name input.</param>
        /// <param name="initialName">Initial value shown in the name textbox.</param>
        /// <param name="showNumber">Indicates whether the number label/input should be visible.</param>
        /// <param name="initialNumber">Initial numeric value to display when number input is shown.</param>
        /// <param name="numberLabel">Label text shown for the numeric input.</param>
        public EntityInputDialog(
            string title,
            string nameLabel,
            string initialName,
            bool showNumber,
            int initialNumber = 0,
            string numberLabel = "Number:")
        {
            InitializeComponent();

            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            labelName.Text = nameLabel;
            textBoxName.Text = initialName;

            labelNumber.Text = numberLabel;
            labelNumber.Visible = showNumber;
            numericNumber.Visible = showNumber;
            numericNumber.Minimum = 0;
            numericNumber.Maximum = 999;
            numericNumber.Value = Math.Max(0, Math.Min(999, initialNumber));

            if (!showNumber)
            {
                int buttonTop = textBoxName.Bottom + 24;
                buttonOk.Top = buttonTop;
                buttonCancel.Top = buttonTop;

                ClientSize = new System.Drawing.Size(ClientSize.Width, buttonCancel.Bottom + 16);
            }
        }

        /// <summary>
        /// Validates required input and confirms the dialog when values are valid.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Name is required.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancels the dialog and closes without applying changes.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}