namespace Group1Project
{
    partial class EntityInputDialog
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
            labelName = new Label();
            labelNumber = new Label();
            textBoxName = new TextBox();
            numericNumber = new NumericUpDown();
            buttonOk = new Button();
            buttonCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numericNumber).BeginInit();
            SuspendLayout();
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(42, 98);
            labelName.Name = "labelName";
            labelName.Size = new Size(83, 32);
            labelName.TabIndex = 0;
            labelName.Text = "Name:";
            // 
            // labelNumber
            // 
            labelNumber.AutoSize = true;
            labelNumber.Location = new Point(42, 177);
            labelNumber.Name = "labelNumber";
            labelNumber.Size = new Size(107, 32);
            labelNumber.TabIndex = 1;
            labelNumber.Text = "Number:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(253, 91);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(294, 39);
            textBoxName.TabIndex = 2;
            // 
            // numericNumber
            // 
            numericNumber.Location = new Point(253, 170);
            numericNumber.Name = "numericNumber";
            numericNumber.Size = new Size(240, 39);
            numericNumber.TabIndex = 3;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(438, 294);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(150, 46);
            buttonOk.TabIndex = 4;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(595, 294);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(150, 46);
            buttonCancel.TabIndex = 5;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // EntityInputDialog
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(757, 353);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(numericNumber);
            Controls.Add(textBoxName);
            Controls.Add(labelNumber);
            Controls.Add(labelName);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EntityInputDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit";
            ((System.ComponentModel.ISupportInitialize)numericNumber).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelName;
        private Label labelNumber;
        private TextBox textBoxName;
        private NumericUpDown numericNumber;
        private Button buttonOk;
        private Button buttonCancel;
    }
}