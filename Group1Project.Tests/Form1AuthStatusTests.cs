using System;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - Form1AuthStatusTests
/// Author: Cameron, Jun, Jonathan
/// Date: April 1, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for Form1 authentication status text behavior in the status bar.
    /// </summary>
    [TestClass]
    public class Form1AuthStatusTests
    {
        /// <summary>
        /// Verifies status bar shows not-logged-in text when no user is authenticated and no tournament is selected.
        /// </summary>
        [TestMethod]
        public void UpdateStatusBar_ShowsNotLoggedIn_WhenNoUserAndNoTournament()
        {
            using var form = new Form1();
            SetApiClientAuthState(form, null, null);
            SetPrivateField(form, "currentTournament", null);

            InvokePrivateInstance(form, "UpdateStatusBar");

            var sslHint = GetPrivateField<ToolStripStatusLabel>(form, "sslHint");
            StringAssert.Contains(sslHint.Text, "Not logged in");
            StringAssert.Contains(sslHint.Text, "No tournament selected");
        }

        /// <summary>
        /// Verifies status bar shows logged-in user text when authentication state is present and no tournament is selected.
        /// </summary>
        [TestMethod]
        public void UpdateStatusBar_ShowsLoggedInUser_WhenUserAuthenticatedAndNoTournament()
        {
            using var form = new Form1();
            SetApiClientAuthState(form, Guid.NewGuid().ToString(), "testuser");
            SetPrivateField(form, "currentTournament", null);

            InvokePrivateInstance(form, "UpdateStatusBar");

            var sslHint = GetPrivateField<ToolStripStatusLabel>(form, "sslHint");
            StringAssert.Contains(sslHint.Text, "Logged in: testuser");
            StringAssert.Contains(sslHint.Text, "No tournament selected");
        }

        /// <summary>
        /// Verifies status bar includes logged-in user and selected tournament information.
        /// </summary>
        [TestMethod]
        public void UpdateStatusBar_IncludesTournament_WhenUserAuthenticatedAndTournamentSelected()
        {
            using var form = new Form1();
            SetApiClientAuthState(form, Guid.NewGuid().ToString(), "testuser");

            var tournament = new Tournament("Auth Test Tournament", DateTime.Now.AddDays(2), "Arena");
            SetPrivateField(form, "currentTournament", tournament);

            InvokePrivateInstance(form, "UpdateStatusBar");

            var sslHint = GetPrivateField<ToolStripStatusLabel>(form, "sslHint");
            StringAssert.Contains(sslHint.Text, "Logged in: testuser");
            StringAssert.Contains(sslHint.Text, "Current Tournament: Auth Test Tournament");
        }

        /// <summary>
        /// Sets private ApiClient authentication fields used by Form1.
        /// </summary>
        /// <param name="form">Target main form instance.</param>
        /// <param name="userId">User identifier value.</param>
        /// <param name="userName">User display name value.</param>
        private static void SetApiClientAuthState(Form1 form, string? userId, string? userName)
        {
            var apiClient = GetPrivateField<ApiClient>(form, "_apiClient");
            SetPrivateField(apiClient, "_currentUserId", userId);
            SetPrivateField(apiClient, "_currentUserName", userName);
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
        /// Sets a private instance field value by name for test setup.
        /// </summary>
        /// <param name="target">Target object containing the field.</param>
        /// <param name="fieldName">Private field name.</param>
        /// <param name="value">Value to assign to the field.</param>
        private static void SetPrivateField(object target, string fieldName, object? value)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' not found.");
            field!.SetValue(target, value);
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