using System;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - ResultsPageTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for critical Results page behavior, including grid setup,
    /// score input setup, and winner text logic.
    /// </summary>
    [TestClass]
    public class ResultsPageTests
    {
        /// <summary>
        /// Verifies Score A numeric input minimum and maximum values are configured as expected.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesScoreA_MinAndMax()
        {
            var page = new ResultsPage();
            var numericScoreA = GetPrivateField<NumericUpDown>(page, "numericScoreA");

            Assert.AreEqual(0m, numericScoreA.Minimum);
            Assert.AreEqual(999m, numericScoreA.Maximum);
        }

        /// <summary>
        /// Verifies Score B numeric input minimum and maximum values are configured as expected.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesScoreB_MinAndMax()
        {
            var page = new ResultsPage();
            var numericScoreB = GetPrivateField<NumericUpDown>(page, "numericScoreB");

            Assert.AreEqual(0m, numericScoreB.Minimum);
            Assert.AreEqual(999m, numericScoreB.Maximum);
        }

        /// <summary>
        /// Verifies the results grid is read-only and disallows adding rows.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_ReadOnlyAndNoAddRows()
        {
            var page = new ResultsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewResults");

            Assert.IsTrue(grid.ReadOnly);
            Assert.IsFalse(grid.AllowUserToAddRows);
        }

        /// <summary>
        /// Verifies expected selection behavior is configured for the results grid.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_SelectionProperties()
        {
            var page = new ResultsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewResults");

            Assert.AreEqual(DataGridViewSelectionMode.FullRowSelect, grid.SelectionMode);
            Assert.IsFalse(grid.MultiSelect);
            Assert.IsFalse(grid.RowHeadersVisible);
        }

        /// <summary>
        /// Verifies all expected results grid columns are created with correct headers.
        /// </summary>
        [TestMethod]
        public void Constructor_CreatesExpectedColumns()
        {
            var page = new ResultsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewResults");

            Assert.AreEqual(6, grid.Columns.Count);
            Assert.AreEqual("Team A", grid.Columns[0].HeaderText);
            Assert.AreEqual("Team B", grid.Columns[1].HeaderText);
            Assert.AreEqual("Scheduled", grid.Columns[2].HeaderText);
            Assert.AreEqual("Score", grid.Columns[3].HeaderText);
            Assert.AreEqual("Winner", grid.Columns[4].HeaderText);
            Assert.AreEqual("Status", grid.Columns[5].HeaderText);
        }

        /// <summary>
        /// Verifies winner text returns placeholder when score values are not present.
        /// </summary>
        [TestMethod]
        public void GetWinnerText_ReturnsDash_WhenScoresMissing()
        {
            var match = CreateMatch(scoreA: null, scoreB: null);

            var result = (string)InvokePrivateStatic(typeof(ResultsPage), "GetWinnerText", match)!;

            Assert.AreEqual("-", result);
        }

        /// <summary>
        /// Verifies winner text returns Team A name when Team A score is higher.
        /// </summary>
        [TestMethod]
        public void GetWinnerText_ReturnsTeamA_WhenScoreAHigher()
        {
            var match = CreateMatch(scoreA: 3, scoreB: 1);

            var result = (string)InvokePrivateStatic(typeof(ResultsPage), "GetWinnerText", match)!;

            Assert.AreEqual("Team A", result);
        }

        /// <summary>
        /// Verifies winner text returns Draw when both scores are equal.
        /// </summary>
        [TestMethod]
        public void GetWinnerText_ReturnsDraw_WhenTie()
        {
            var match = CreateMatch(scoreA: 2, scoreB: 2);

            var result = (string)InvokePrivateStatic(typeof(ResultsPage), "GetWinnerText", match)!;

            Assert.AreEqual("Draw", result);
        }

        /// <summary>
        /// Creates a match DTO used by winner-text tests.
        /// </summary>
        /// <param name="scoreA">Score for Team A.</param>
        /// <param name="scoreB">Score for Team B.</param>
        /// <returns>A match DTO instance for testing.</returns>
        private static ApiClient.MatchReadDto CreateMatch(int? scoreA, int? scoreB)
        {
            return new ApiClient.MatchReadDto(
                Id: Guid.NewGuid().ToString(),
                TeamAId: Guid.NewGuid().ToString(),
                TeamAName: "Team A",
                TeamBId: Guid.NewGuid().ToString(),
                TeamBName: "Team B",
                ScheduledStart: DateTime.Now,
                Status: "Complete",
                ScoreA: scoreA,
                ScoreB: scoreB,
                RoundNumber: 1,
                MatchNumber: 1);
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
    }
}
