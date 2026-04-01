using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - LoginFormAuthTests
/// Author: Cameron, Jun, Jonathan
/// Date: April 1, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for LoginForm login, sign-up, cancel actions, and credential properties.
    /// </summary>
    [TestClass]
    public class LoginFormAuthTests
    {
        /// <summary>
        /// Verifies UserName property returns trimmed user name text.
        /// </summary>
        [TestMethod]
        public void UserName_ReturnsTrimmedText()
        {
            using var form = new LoginForm();
            var userNameTextBox = GetPrivateField<TextBox>(form, "txtUserName");

            userNameTextBox.Text = "  sampleUser  ";

            Assert.AreEqual("sampleUser", form.UserName);
        }

        /// <summary>
        /// Verifies Password property returns the password textbox value as entered.
        /// </summary>
        [TestMethod]
        public void Password_ReturnsEnteredText()
        {
            using var form = new LoginForm();
            var passwordTextBox = GetPrivateField<TextBox>(form, "txtPassword");

            passwordTextBox.Text = "secret123";

            Assert.AreEqual("secret123", form.Password);
        }

        /// <summary>
        /// Verifies login click with valid inputs sets dialog result to OK.
        /// </summary>
        [TestMethod]
        public void BtnLoginClick_WithValidInput_SetsDialogResultOk()
        {
            using var form = new LoginForm();
            SetCredentials(form, "user1", "pass1");

            InvokePrivateInstance(form, "btnLogin_Click", form, System.EventArgs.Empty);

            Assert.AreEqual(DialogResult.OK, form.DialogResult);
        }

        /// <summary>
        /// Verifies sign-up click with valid inputs sets dialog result to Retry.
        /// </summary>
        [TestMethod]
        public void BtnSignUpClick_WithValidInput_SetsDialogResultRetry()
        {
            using var form = new LoginForm();
            SetCredentials(form, "newuser", "newpass");

            InvokePrivateInstance(form, "btnSignUp_Click", form, System.EventArgs.Empty);

            Assert.AreEqual(DialogResult.Retry, form.DialogResult);
        }

        /// <summary>
        /// Verifies cancel click sets dialog result to Cancel.
        /// </summary>
        [TestMethod]
        public void BtnCancelClick_SetsDialogResultCancel()
        {
            using var form = new LoginForm();

            InvokePrivateInstance(form, "btnCancel_Click", form, System.EventArgs.Empty);

            Assert.AreEqual(DialogResult.Cancel, form.DialogResult);
        }

        /// <summary>
        /// Sets login credentials in private textbox controls.
        /// </summary>
        /// <param name="form">Login form instance.</param>
        /// <param name="userName">User name text value.</param>
        /// <param name="password">Password text value.</param>
        private static void SetCredentials(LoginForm form, string userName, string password)
        {
            var userNameTextBox = GetPrivateField<TextBox>(form, "txtUserName");
            var passwordTextBox = GetPrivateField<TextBox>(form, "txtPassword");

            userNameTextBox.Text = userName;
            passwordTextBox.Text = password;
        }

        /// <summary>
        /// Reads a private instance field value by name for test inspection.
        /// </summary>
        /// <typeparam name="T">Expected field type.</typeparam>
        /// <param name="target">Target object containing the field.</param>
        /// <param name="fieldName">Private field name.</param>
        /// <returns>The field value cast to the requested type.</returns>
        private static T GetPrivateField<T>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' not found.");
            return (T)field!.GetValue(target)!;
        }

        /// <summary>
        /// Invokes a private instance method using reflection.
        /// </summary>
        /// <param name="instance">Object instance containing the method.</param>
        /// <param name="methodName">Method name to invoke.</param>
        /// <param name="args">Arguments passed to the method.</param>
        /// <returns>The method return value.</returns>
        private static object? InvokePrivateInstance(object instance, string methodName, params object[] args)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, $"Method '{methodName}' not found.");
            return method!.Invoke(instance, args);
        }
    }
}