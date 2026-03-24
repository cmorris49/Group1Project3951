using System;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - DashboardPageTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for core Dashboard page formatting and reset behavior.
    /// </summary>
    [TestClass]
    public class DashboardPageTests
    {
        /// <summary>
        /// Verifies next-match value label is initialized with default text.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesNextMatchLabel_DefaultText()
        {
            var page = new DashboardPage();
            var nextMatchLabel = GetPrivateField<Label>(page, "_labelNextMatchValue");

            Assert.AreEqual("No upcoming match", nextMatchLabel.Text);
        }

        /// <summary>
        /// Verifies completed-match predicate returns true when status and scores are valid.
        /// </summary>
        [TestMethod]
        public void IsCompletedWithResult_ReturnsTrue_ForCompleteWithScores()
        {
            var match = CreateMatch("Complete", 2, 1);

            var result = (bool)InvokePrivateStatic(typeof(DashboardPage), "IsCompletedWithResult", match)!;

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Verifies completed-match predicate returns false for non-complete status.
        /// </summary>
        [TestMethod]
        public void IsCompletedWithResult_ReturnsFalse_ForUnscheduled()
        {
            var match = CreateMatch("Unscheduled", 2, 1);

            var result = (bool)InvokePrivateStatic(typeof(DashboardPage), "IsCompletedWithResult", match)!;

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Verifies team formatter uses TBD placeholders when team names are null.
        /// </summary>
        [TestMethod]
        public void FormatTeams_UsesTbd_WhenNamesAreNull()
        {
            var match = new ApiClient.MatchReadDto(
                Guid.NewGuid().ToString(),
                null,
                null,
                null,
                null,
                null,
                "Unscheduled",
                null,
                null,
                1,
                1);

            var text = (string)InvokePrivateStatic(typeof(DashboardPage), "FormatTeams", match)!;

            Assert.AreEqual("TBD vs TBD", text);
        }

        /// <summary>
        /// Verifies winner formatter returns Team B when Team B score is higher.
        /// </summary>
        [TestMethod]
        public void GetWinnerName_ReturnsTeamB_WhenScoreBHigher()
        {
            var match = CreateMatch("Complete", 1, 3);

            var winner = (string)InvokePrivateStatic(typeof(DashboardPage), "GetWinnerName", match)!;

            Assert.AreEqual("Team B", winner);
        }

        /// <summary>
        /// Verifies reset routine clears activity sections and restores next-match default text.
        /// </summary>
        [TestMethod]
        public void ResetMatchSections_ClearsRecentAndUpcomingAndResetsNextMatch()
        {
            var page = new DashboardPage();
            var listBox = GetPrivateField<ListBox>(page, "listBox1");
            var flow = GetPrivateField<FlowLayoutPanel>(page, "flowLayoutPanel1");
            var nextMatchLabel = GetPrivateField<Label>(page, "_labelNextMatchValue");

            listBox.Items.Add("recent item");
            flow.Controls.Add(new Label { Text = "upcoming item" });
            nextMatchLabel.Text = "custom text";

            InvokePrivateInstance(page, "ResetMatchSections");

            Assert.AreEqual(0, listBox.Items.Count);
            Assert.AreEqual(0, flow.Controls.Count);
            Assert.AreEqual("No upcoming match", nextMatchLabel.Text);
        }

        /// <summary>
        /// Creates a match DTO used by dashboard formatting tests.
        /// </summary>
        /// <param name="status">Match status value.</param>
        /// <param name="scoreA">Score for Team A.</param>
        /// <param name="scoreB">Score for Team B.</param>
        /// <returns>A match DTO instance for testing.</returns>
        private static ApiClient.MatchReadDto CreateMatch(string status, int? scoreA, int? scoreB)
        {
            return new ApiClient.MatchReadDto(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                "Team A",
                Guid.NewGuid().ToString(),
                "Team B",
                DateTime.Now,
                status,
                scoreA,
                scoreB,
                1,
                1);
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
        /// Invokes a private static method using reflection.
        /// </summary>
        /// <param name="type">Type containing the method.</param>
        /// <param name="methodName">Method name to invoke.</param>
        /// <param name="args">Arguments passed to the method.</param>
        /// <returns>The method return value.</returns>
        private static object? InvokePrivateStatic(Type type, string methodName, params object[] args)
        {
            var method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(method, $"Method '{methodName}' not found.");
            return method!.Invoke(null, args);
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
