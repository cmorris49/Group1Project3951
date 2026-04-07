namespace Group1Project
{
    partial class AutoScheduleDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelFirstMatch = new Label();
            dateTimePickerFirstMatch = new DateTimePicker();
            labelIncrementValue = new Label();
            buttonOk = new Button();
            buttonCancel = new Button();
            numericIncrementValue = new NumericUpDown();
            comboBoxIncrementUnit = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)numericIncrementValue).BeginInit();
            SuspendLayout();
            // 
            // labelFirstMatch
            // 
            labelFirstMatch.AutoSize = true;
            labelFirstMatch.Location = new Point(38, 83);
            labelFirstMatch.Name = "labelFirstMatch";
            labelFirstMatch.Size = new Size(191, 32);
            labelFirstMatch.TabIndex = 0;
            labelFirstMatch.Text = "First match time:";
            // 
            // dateTimePickerFirstMatch
            // 
            dateTimePickerFirstMatch.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dateTimePickerFirstMatch.Format = DateTimePickerFormat.Custom;
            dateTimePickerFirstMatch.Location = new Point(235, 83);
            dateTimePickerFirstMatch.Name = "dateTimePickerFirstMatch";
            dateTimePickerFirstMatch.ShowUpDown = true;
            dateTimePickerFirstMatch.Size = new Size(400, 39);
            dateTimePickerFirstMatch.TabIndex = 1;
            // 
            // labelIncrementValue
            // 
            labelIncrementValue.AutoSize = true;
            labelIncrementValue.Location = new Point(38, 181);
            labelIncrementValue.Name = "labelIncrementValue";
            labelIncrementValue.Size = new Size(306, 32);
            labelIncrementValue.TabIndex = 2;
            labelIncrementValue.Text = "Schedule new match every:";
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(521, 357);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(150, 46);
            buttonOk.TabIndex = 3;
            buttonOk.Text = "Schedule";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(677, 357);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(150, 46);
            buttonCancel.TabIndex = 4;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // numericIncrementValue
            // 
            numericIncrementValue.Location = new Point(350, 181);
            numericIncrementValue.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericIncrementValue.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericIncrementValue.Name = "numericIncrementValue";
            numericIncrementValue.Size = new Size(112, 39);
            numericIncrementValue.TabIndex = 5;
            numericIncrementValue.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // comboBoxIncrementUnit
            // 
            comboBoxIncrementUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxIncrementUnit.FormattingEnabled = true;
            comboBoxIncrementUnit.Items.AddRange(new object[] { "Minutes", "Hours", "Days" });
            comboBoxIncrementUnit.Location = new Point(468, 181);
            comboBoxIncrementUnit.Name = "comboBoxIncrementUnit";
            comboBoxIncrementUnit.Size = new Size(242, 40);
            comboBoxIncrementUnit.TabIndex = 6;
            // 
            // AutoScheduleDialog
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(839, 415);
            Controls.Add(comboBoxIncrementUnit);
            Controls.Add(numericIncrementValue);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(labelIncrementValue);
            Controls.Add(dateTimePickerFirstMatch);
            Controls.Add(labelFirstMatch);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AutoScheduleDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Auto Schedule";
            ((System.ComponentModel.ISupportInitialize)numericIncrementValue).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelFirstMatch;
        private DateTimePicker dateTimePickerFirstMatch;
        private Label labelIncrementValue;
        private Button buttonOk;
        private Button buttonCancel;
        private NumericUpDown numericIncrementValue;
        private ComboBox comboBoxIncrementUnit;
    }
}