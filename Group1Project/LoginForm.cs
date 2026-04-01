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
    public partial class LoginForm : Form
    {
        public string UserName => txtUserName.Text.Trim();
        public string Password => txtPassword.Text;

        /// <summary>
        /// Initializes a new instance of the LoginForm class.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Validates entered credentials and closes the dialog with an OK result when values are present.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the button click.</param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Enter user name and password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancels the login flow and closes the dialog.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the button click.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Validates entered registration data and closes the dialog with a Retry result to trigger sign-up flow.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event arguments for the button click.</param>
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Enter user name and password.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.Retry;
            Close();
        }
    }
}
