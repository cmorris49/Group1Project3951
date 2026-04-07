namespace Group1Project
{
    /// <summary>
    /// Group 1 Project - AutoScheduleDialog Form
    /// Author: Cameron, Jun, Jonathan
    /// Date: April 7, 2026; Revision: 1.0
    /// Source:
    ///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
    /// </summary>
    public partial class AutoScheduleDialog : Form
    {
        /// <summary>
        /// Gets the selected start date and time for the first match to schedule.
        /// </summary>
        public DateTime FirstMatchStart
        {
            get
            {
                return dateTimePickerFirstMatch.Value;
            }
        }

        /// <summary>
        /// Gets the selected increment converted to a <see cref="TimeSpan"/> based on the chosen unit.
        /// </summary>
        public TimeSpan IncrementSpan
        {
            get
            {
                int value = (int)numericIncrementValue.Value;
                string unit = comboBoxIncrementUnit.SelectedItem?.ToString() ?? "Minutes";

                return unit switch
                {
                    "Hours" => TimeSpan.FromHours(value),
                    "Days" => TimeSpan.FromDays(value),
                    _ => TimeSpan.FromMinutes(value)
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the AutoScheduleDialog form.
        /// </summary>
        public AutoScheduleDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Applies initial values for first match start time and increment settings.
        /// </summary>
        /// <param name="firstMatchStart">The initial date and time shown for the first match.</param>
        /// <param name="incrementValue">The initial numeric increment value.</param>
        /// <param name="incrementUnit">The initial increment unit text (Minutes, Hours, or Days).</param>
        public void SetInitialValues(DateTime firstMatchStart, int incrementValue, string incrementUnit)
        {
            dateTimePickerFirstMatch.Value = firstMatchStart;

            if (incrementValue < numericIncrementValue.Minimum)
            {
                incrementValue = (int)numericIncrementValue.Minimum;
            }

            if (incrementValue > numericIncrementValue.Maximum)
            {
                incrementValue = (int)numericIncrementValue.Maximum;
            }

            numericIncrementValue.Value = incrementValue;

            int index = comboBoxIncrementUnit.Items.IndexOf(incrementUnit);
            comboBoxIncrementUnit.SelectedIndex = index >= 0 ? index : 0;
        }

        /// <summary>
        /// Validates dialog selections and confirms auto-schedule input.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (comboBoxIncrementUnit.SelectedItem == null)
            {
                MessageBox.Show("Please select an increment unit.",
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
        /// Cancels auto-schedule input and closes the dialog.
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