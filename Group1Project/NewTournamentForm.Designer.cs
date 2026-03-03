namespace Group1Project
{
    partial class NewTournamentForm
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
            labelTournamentName = new Label();
            textTournamentName = new TextBox();
            labelStartDate = new Label();
            dateTimePickerStartDate = new DateTimePicker();
            labelLocation = new Label();
            textLocation = new TextBox();
            buttonCreate = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // labelTournamentName
            // 
            labelTournamentName.AutoSize = true;
            labelTournamentName.Location = new Point(12, 41);
            labelTournamentName.Name = "labelTournamentName";
            labelTournamentName.Size = new Size(214, 32);
            labelTournamentName.TabIndex = 0;
            labelTournamentName.Text = "Tournament Name";
            // 
            // textTournamentName
            // 
            textTournamentName.Location = new Point(232, 34);
            textTournamentName.Name = "textTournamentName";
            textTournamentName.Size = new Size(200, 39);
            textTournamentName.TabIndex = 1;
            // 
            // labelStartDate
            // 
            labelStartDate.AutoSize = true;
            labelStartDate.Location = new Point(12, 100);
            labelStartDate.Name = "labelStartDate";
            labelStartDate.Size = new Size(119, 32);
            labelStartDate.TabIndex = 2;
            labelStartDate.Text = "Start Date";
            // 
            // dateTimePickerStartDate
            // 
            dateTimePickerStartDate.Format = DateTimePickerFormat.Short;
            dateTimePickerStartDate.Location = new Point(232, 95);
            dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            dateTimePickerStartDate.Size = new Size(400, 39);
            dateTimePickerStartDate.TabIndex = 3;
            // 
            // labelLocation
            // 
            labelLocation.AutoSize = true;
            labelLocation.Location = new Point(12, 160);
            labelLocation.Name = "labelLocation";
            labelLocation.Size = new Size(104, 32);
            labelLocation.TabIndex = 4;
            labelLocation.Text = "Location";
            // 
            // textLocation
            // 
            textLocation.Location = new Point(232, 165);
            textLocation.Name = "textLocation";
            textLocation.Size = new Size(200, 39);
            textLocation.TabIndex = 5;
            // 
            // buttonCreate
            // 
            buttonCreate.DialogResult = DialogResult.OK;
            buttonCreate.Location = new Point(482, 392);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new Size(150, 46);
            buttonCreate.TabIndex = 6;
            buttonCreate.Text = "Create";
            buttonCreate.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(638, 392);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(150, 46);
            buttonCancel.TabIndex = 7;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // NewTournamentForm
            // 
            AcceptButton = buttonCreate;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonCancel);
            Controls.Add(buttonCreate);
            Controls.Add(textLocation);
            Controls.Add(labelLocation);
            Controls.Add(dateTimePickerStartDate);
            Controls.Add(labelStartDate);
            Controls.Add(textTournamentName);
            Controls.Add(labelTournamentName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewTournamentForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "NewTournamentForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTournamentName;
        private TextBox textTournamentName;
        private Label labelStartDate;
        private DateTimePicker dateTimePickerStartDate;
        private Label labelLocation;
        private TextBox textLocation;
        private Button buttonCreate;
        private Button buttonCancel;
    }
}