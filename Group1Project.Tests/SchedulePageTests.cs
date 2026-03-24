using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - SchedulePageTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for critical Schedule page setup behavior.
    /// </summary>
    [TestClass]
    public class SchedulePageTests
    {
        /// <summary>
        /// Verifies schedule grid disallows manual row insertion.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_DisallowAddRows()
        {
            var page = new SchedulePage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewSchedule");

            Assert.IsFalse(grid.AllowUserToAddRows);
        }

        /// <summary>
        /// Verifies schedule grid is read-only.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_ReadOnly()
        {
            var page = new SchedulePage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewSchedule");

            Assert.IsTrue(grid.ReadOnly);
        }

        /// <summary>
        /// Verifies schedule grid row headers are hidden.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_RowHeadersHidden()
        {
            var page = new SchedulePage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewSchedule");

            Assert.IsFalse(grid.RowHeadersVisible);
        }

        /// <summary>
        /// Verifies schedule grid selection mode is full-row select.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_SelectionModeFullRowSelect()
        {
            var page = new SchedulePage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewSchedule");

            Assert.AreEqual(DataGridViewSelectionMode.FullRowSelect, grid.SelectionMode);
        }

        /// <summary>
        /// Verifies schedule grid multi-select is disabled.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_MultiSelectFalse()
        {
            var page = new SchedulePage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewSchedule");

            Assert.IsFalse(grid.MultiSelect);
        }

        /// <summary>
        /// Verifies schedule grid uses fill-mode column sizing.
        /// </summary>
        [TestMethod]
        public void Constructor_ConfiguresGrid_AutoSizeColumnsFill()
        {
            var page = new SchedulePage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewSchedule");

            Assert.AreEqual(DataGridViewAutoSizeColumnsMode.Fill, grid.AutoSizeColumnsMode);
        }

        /// <summary>
        /// Verifies schedule grid columns exist with expected headers.
        /// </summary>
        [TestMethod]
        public void Constructor_CreatesExpectedColumns()
        {
            var page = new SchedulePage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewSchedule");

            Assert.AreEqual(5, grid.Columns.Count);
            Assert.AreEqual("Team A", grid.Columns[0].HeaderText);
            Assert.AreEqual("Team B", grid.Columns[1].HeaderText);
            Assert.AreEqual("Scheduled", grid.Columns[2].HeaderText);
            Assert.AreEqual("Status", grid.Columns[3].HeaderText);
            Assert.AreEqual("Score", grid.Columns[4].HeaderText);
        }

        /// <summary>
        /// Verifies schedule date-time picker exists.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesDateTimePicker()
        {
            var page = new SchedulePage();
            var picker = GetPrivateField<DateTimePicker>(page, "dateTimePickerStart");

            Assert.IsNotNull(picker);
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
