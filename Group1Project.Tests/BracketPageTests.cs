using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - BracketPageTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for critical Bracket page rendering and view-switch behavior.
    /// </summary>
    [TestClass]
    public class BracketPageTests
    {
        /// <summary>
        /// Verifies constructor populates view selector options.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesViewSelectorItems()
        {
            var page = new BracketPage();
            var combo = GetPrivateField<ComboBox>(page, "comboBoxView");

            Assert.IsTrue(combo.Items.Count >= 2);
        }

        /// <summary>
        /// Verifies matches grid receives expected columns during initialization.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesMatchesGridColumns()
        {
            var page = new BracketPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewStageMatches");

            Assert.AreEqual(4, grid.Columns.Count);
            Assert.AreEqual("Team A", grid.Columns[0].HeaderText);
            Assert.AreEqual("Team B", grid.Columns[1].HeaderText);
            Assert.AreEqual("Status", grid.Columns[2].HeaderText);
            Assert.AreEqual("Score", grid.Columns[3].HeaderText);
        }

        /// <summary>
        /// Verifies ShowView displays matches grid for Matches view.
        /// </summary>
        [TestMethod]
        public void ShowView_Matches_MakesGridVisible()
        {
            var page = new BracketPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewStageMatches");
            var panel = GetPrivateField<Panel>(page, "panelBracketContainer");

            InvokePrivateInstance(page, "ShowView", "Matches");

            Assert.IsTrue(grid.Visible);
            Assert.IsFalse(panel.Visible);
        }

        /// <summary>
        /// Verifies ShowView displays bracket panel for Bracket view.
        /// </summary>
        [TestMethod]
        public void ShowView_Bracket_MakesBracketVisible()
        {
            var page = new BracketPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewStageMatches");
            var panel = GetPrivateField<Panel>(page, "panelBracketContainer");

            InvokePrivateInstance(page, "ShowView", "Bracket");

            Assert.IsFalse(grid.Visible);
            Assert.IsTrue(panel.Visible);
        }

        /// <summary>
        /// Verifies ShowView fallback displays matches grid for unknown view names.
        /// </summary>
        [TestMethod]
        public void ShowView_Default_FallsBackToMatchesGrid()
        {
            var page = new BracketPage();
            var grid = GetPrivateField<DataGridView>(page, "dataGridViewStageMatches");
            var panel = GetPrivateField<Panel>(page, "panelBracketContainer");

            InvokePrivateInstance(page, "ShowView", "UnknownView");

            Assert.IsTrue(grid.Visible);
            Assert.IsFalse(panel.Visible);
        }

        /// <summary>
        /// Verifies bracket renderer shows no-match label when API match list is empty.
        /// </summary>
        [TestMethod]
        public void DrawBracketTree_EmptyMatches_ShowsNoMatchesLabel()
        {
            var page = new BracketPage();
            var panel = GetPrivateField<Panel>(page, "panelBracketContainer");

            SetPrivateField(page, "_apiMatches", new List<ApiClient.MatchReadDto>());

            InvokePrivateInstance(page, "DrawBracketTreeFromApiMatches");

            Assert.AreEqual(1, panel.Controls.Count);
            Assert.IsTrue(panel.Controls[0] is Label);
            Assert.AreEqual("No matches to display", panel.Controls[0].Text);
        }

        /// <summary>
        /// Verifies bracket renderer adds match box controls when match data exists.
        /// </summary>
        [TestMethod]
        public void DrawBracketTree_WithOneMatch_AddsGroupBoxControl()
        {
            var page = new BracketPage();
            var panel = GetPrivateField<Panel>(page, "panelBracketContainer");

            var matches = new List<ApiClient.MatchReadDto>
            {
                new ApiClient.MatchReadDto(
                    Id: Guid.NewGuid().ToString(),
                    TeamAId: Guid.NewGuid().ToString(),
                    TeamAName: "Team A",
                    TeamBId: Guid.NewGuid().ToString(),
                    TeamBName: "Team B",
                    ScheduledStart: null,
                    Status: "Unscheduled",
                    ScoreA: null,
                    ScoreB: null,
                    RoundNumber: 1,
                    MatchNumber: 1)
            };

            SetPrivateField(page, "_apiMatches", matches);

            InvokePrivateInstance(page, "DrawBracketTreeFromApiMatches");

            Assert.IsTrue(panel.Controls.Count > 0);
            Assert.IsTrue(ContainsControlType<GroupBox>(panel));
        }

        /// <summary>
        /// Verifies connector-line draw routine safely exits when no connector pairs exist.
        /// </summary>
        [TestMethod]
        public void DrawConnectingLines_NoPairs_DoesNotThrow()
        {
            var page = new BracketPage();

            using var bitmap = new Bitmap(200, 200);
            using var graphics = Graphics.FromImage(bitmap);

            InvokePrivateInstance(page, "DrawConnectingLinesFromRenderedBoxes", graphics);

            Assert.IsTrue(true);
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

        /// <summary>
        /// Sets a private instance field by name.
        /// </summary>
        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Field '{fieldName}' not found.");
            field!.SetValue(target, value);
        }

        /// <summary>
        /// Invokes a private instance method using reflection.
        /// </summary>
        private static object? InvokePrivateInstance(object instance, string methodName, params object[] args)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, $"Method '{methodName}' not found.");
            return method!.Invoke(instance, args);
        }

        /// <summary>
        /// Checks whether a control hierarchy contains at least one control of the specified type.
        /// </summary>
        private static bool ContainsControlType<T>(Control root) where T : Control
        {
            foreach (Control child in root.Controls)
            {
                if (child is T)
                {
                    return true;
                }

                if (ContainsControlType<T>(child))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
