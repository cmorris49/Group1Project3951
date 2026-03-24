using System;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - TeamStatsPageTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for critical Team Stats page setup and computed row behavior.
    /// </summary>
    [TestClass]
    public class TeamStatsPageTests
    {
        /// <summary>
        /// Verifies team stats grid disallows manual row insertion.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_DisallowAddRows()
        {
            var page = new TeamStatsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewTeamStats");

            Assert.IsFalse(grid.AllowUserToAddRows);
        }

        /// <summary>
        /// Verifies team stats grid is read-only.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_ReadOnly()
        {
            var page = new TeamStatsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewTeamStats");

            Assert.IsTrue(grid.ReadOnly);
        }

        /// <summary>
        /// Verifies team stats grid row headers are hidden.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_RowHeadersHidden()
        {
            var page = new TeamStatsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewTeamStats");

            Assert.IsFalse(grid.RowHeadersVisible);
        }

        /// <summary>
        /// Verifies team stats grid selection mode is full-row select.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_SelectionModeFullRowSelect()
        {
            var page = new TeamStatsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewTeamStats");

            Assert.AreEqual(DataGridViewSelectionMode.FullRowSelect, grid.SelectionMode);
        }

        /// <summary>
        /// Verifies team stats grid multi-select is disabled.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_MultiSelectFalse()
        {
            var page = new TeamStatsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewTeamStats");

            Assert.IsFalse(grid.MultiSelect);
        }

        /// <summary>
        /// Verifies team stats grid uses fill-mode column sizing.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_AutoSizeColumnsFill()
        {
            var page = new TeamStatsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewTeamStats");

            Assert.AreEqual(DataGridViewAutoSizeColumnsMode.Fill, grid.AutoSizeColumnsMode);
        }

        /// <summary>
        /// Verifies team stats grid columns exist with expected headers.
        /// </summary>
        [TestMethod]
        public void Constructor_CreatesExpectedColumns()
        {
            var page = new TeamStatsPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewTeamStats");

            Assert.AreEqual(9, grid.Columns.Count);
            Assert.AreEqual("Team", grid.Columns[0].HeaderText);
            Assert.AreEqual("GP", grid.Columns[1].HeaderText);
            Assert.AreEqual("W", grid.Columns[2].HeaderText);
            Assert.AreEqual("L", grid.Columns[3].HeaderText);
            Assert.AreEqual("D", grid.Columns[4].HeaderText);
            Assert.AreEqual("PF", grid.Columns[5].HeaderText);
            Assert.AreEqual("PA", grid.Columns[6].HeaderText);
            Assert.AreEqual("PD", grid.Columns[7].HeaderText);
            Assert.AreEqual("Win %", grid.Columns[8].HeaderText);
        }

        /// <summary>
        /// Verifies the TeamStatRow PD computed property returns PF minus PA.
        /// </summary>
        [TestMethod]
        public void TeamStatRow_PD_ReturnsPointsForMinusPointsAgainst()
        {
            var rowType = typeof(TeamStatsPage).GetNestedType("TeamStatRow", BindingFlags.NonPublic);
            Assert.IsNotNull(rowType, "Nested TeamStatRow type not found.");

            var row = Activator.CreateInstance(rowType!, nonPublic: true);
            Assert.IsNotNull(row);

            rowType!.GetProperty("PF")!.SetValue(row, 42);
            rowType.GetProperty("PA")!.SetValue(row, 30);

            var pd = (int)rowType.GetProperty("PD")!.GetValue(row)!;
            Assert.AreEqual(12, pd);
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
    }
}
