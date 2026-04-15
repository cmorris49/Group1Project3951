using System;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - AutoScheduleDialogTests
/// Author: Cameron, Jun, Jonathan
/// Date: April 9, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for auto-schedule dialog increment and initialization behavior.
    /// </summary>
    [TestClass]
    public class AutoScheduleDialogTests
    {
        /// <summary>
        /// Verifies default increment span uses minutes when no increment unit is selected.
        /// </summary>
        [TestMethod]
        public void IncrementSpan_DefaultsToMinutes_WhenNoUnitSelected()
        {
            using var dialog = new AutoScheduleDialog();

            Assert.AreEqual(TimeSpan.FromMinutes(30), dialog.IncrementSpan);
        }

        /// <summary>
        /// Verifies increment span converts hours correctly.
        /// </summary>
        [TestMethod]
        public void IncrementSpan_UsesHours_WhenHoursUnitSelected()
        {
            using var dialog = new AutoScheduleDialog();
            dialog.SetInitialValues(DateTime.Now, 2, "Hours");

            Assert.AreEqual(TimeSpan.FromHours(2), dialog.IncrementSpan);
        }

        /// <summary>
        /// Verifies increment span converts days correctly.
        /// </summary>
        [TestMethod]
        public void IncrementSpan_UsesDays_WhenDaysUnitSelected()
        {
            using var dialog = new AutoScheduleDialog();
            dialog.SetInitialValues(DateTime.Now, 3, "Days");

            Assert.AreEqual(TimeSpan.FromDays(3), dialog.IncrementSpan);
        }

        /// <summary>
        /// Verifies initial increment value is clamped to numeric minimum when lower than allowed.
        /// </summary>
        [TestMethod]
        public void SetInitialValues_ClampsIncrementToMinimum()
        {
            using var dialog = new AutoScheduleDialog();
            var numeric = GetPrivateField<NumericUpDown>(dialog, "numericIncrementValue");

            dialog.SetInitialValues(DateTime.Now, 0, "Minutes");

            Assert.AreEqual(numeric.Minimum, numeric.Value);
        }

        /// <summary>
        /// Verifies initial increment value is clamped to numeric maximum when higher than allowed.
        /// </summary>
        [TestMethod]
        public void SetInitialValues_ClampsIncrementToMaximum()
        {
            using var dialog = new AutoScheduleDialog();
            var numeric = GetPrivateField<NumericUpDown>(dialog, "numericIncrementValue");

            dialog.SetInitialValues(DateTime.Now, 20001, "Minutes");

            Assert.AreEqual(numeric.Maximum, numeric.Value);
        }

        /// <summary>
        /// Verifies unknown increment unit falls back to first combo option.
        /// </summary>
        [TestMethod]
        public void SetInitialValues_UnknownUnit_SelectsDefaultUnit()
        {
            using var dialog = new AutoScheduleDialog();
            var combo = GetPrivateField<ComboBox>(dialog, "comboBoxIncrementUnit");

            dialog.SetInitialValues(DateTime.Now, 30, "UnknownUnit");

            Assert.AreEqual(0, combo.SelectedIndex);
            Assert.AreEqual("Minutes", combo.SelectedItem?.ToString());
        }

        /// <summary>
        /// Reads private instance field value by name.
        /// </summary>
        private static T GetPrivateField<T>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' not found.");
            return (T)field!.GetValue(target)!;
        }
    }
}