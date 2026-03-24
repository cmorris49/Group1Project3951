using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - TeamsPlayersPageTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for critical Teams & Players page behavior.
    /// </summary>
    [TestClass]
    public class TeamsPlayersPageTests
    {
        /// <summary>
        /// Verifies constructor initializes main teams grid.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesTeamsGrid()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");

            Assert.IsNotNull(teamsGrid);
        }

        /// <summary>
        /// Verifies constructor initializes players grid.
        /// </summary>
        [TestMethod]
        public void Constructor_InitializesPlayersGrid()
        {
            var page = new TeamsPlayersPage();
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            Assert.IsNotNull(playersGrid);
        }

        /// <summary>
        /// Verifies teams grid has expected default columns from designer.
        /// </summary>
        [TestMethod]
        public void Constructor_TeamsGrid_HasExpectedColumns()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");

            Assert.AreEqual(3, teamsGrid.Columns.Count);
            Assert.AreEqual("Team", teamsGrid.Columns[0].HeaderText);
            Assert.AreEqual("Seed", teamsGrid.Columns[1].HeaderText);
            Assert.AreEqual("Players", teamsGrid.Columns[2].HeaderText);
        }

        /// <summary>
        /// Verifies players grid has expected default columns from designer.
        /// </summary>
        [TestMethod]
        public void Constructor_PlayersGrid_HasExpectedColumns()
        {
            var page = new TeamsPlayersPage();
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            Assert.AreEqual(2, playersGrid.Columns.Count);
            Assert.AreEqual("Name", playersGrid.Columns[0].HeaderText);
            Assert.AreEqual("#", playersGrid.Columns[1].HeaderText);
        }

        /// <summary>
        /// Verifies selection handler returns without error when no team row is selected.
        /// </summary>
        [TestMethod]
        public void DataViewTeams_SelectionChanged_NoSelection_DoesNotAddPlayers()
        {
            var page = new TeamsPlayersPage();
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            InvokePrivateInstance(page, "DataViewTeams_SelectionChanged", null!, EventArgs.Empty);

            Assert.AreEqual(0, playersGrid.Rows.Count);
        }

        /// <summary>
        /// Verifies selection handler populates player rows for a valid selected team.
        /// </summary>
        [TestMethod]
        public void DataViewTeams_SelectionChanged_ValidSelection_AddsPlayerRows()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            var loadedTeams = new List<ApiClient.TeamReadDto>
            {
                new ApiClient.TeamReadDto(
                    Id: Guid.NewGuid().ToString(),
                    Name: "Warriors",
                    Seed: 1,
                    DivisionId: Guid.NewGuid().ToString(),
                    DivisionName: "Main Division",
                    Players: new List<ApiClient.PlayerReadDto>
                    {
                        new ApiClient.PlayerReadDto(Guid.NewGuid().ToString(), "Alice", 7),
                        new ApiClient.PlayerReadDto(Guid.NewGuid().ToString(), "Bob", 11)
                    })
            };

            SetPrivateField(page, "loadedTeams", loadedTeams);

            teamsGrid.Rows.Add("Warriors", 1, 2);
            teamsGrid.Rows[0].Selected = true;

            InvokePrivateInstance(page, "DataViewTeams_SelectionChanged", teamsGrid, EventArgs.Empty);

            Assert.AreEqual(2, playersGrid.Rows.Count);
        }

        /// <summary>
        /// Verifies selection handler clears stale player rows before rendering selected team players.
        /// </summary>
        [TestMethod]
        public void DataViewTeams_SelectionChanged_ClearsExistingRowsBeforePopulate()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            playersGrid.Rows.Add("Old Player", 99);

            var loadedTeams = new List<ApiClient.TeamReadDto>
            {
                new ApiClient.TeamReadDto(
                    Id: Guid.NewGuid().ToString(),
                    Name: "Knights",
                    Seed: 2,
                    DivisionId: Guid.NewGuid().ToString(),
                    DivisionName: "Main Division",
                    Players: new List<ApiClient.PlayerReadDto>
                    {
                        new ApiClient.PlayerReadDto(Guid.NewGuid().ToString(), "New Player", 3)
                    })
            };

            SetPrivateField(page, "loadedTeams", loadedTeams);

            teamsGrid.Rows.Add("Knights", 2, 1);
            teamsGrid.Rows[0].Selected = true;

            InvokePrivateInstance(page, "DataViewTeams_SelectionChanged", teamsGrid, EventArgs.Empty);

            Assert.AreEqual(1, playersGrid.Rows.Count);
            Assert.AreEqual("New Player", playersGrid.Rows[0].Cells[0].Value);
        }

        /// <summary>
        /// Verifies selection handler safely exits when selected index is outside loaded team range.
        /// </summary>
        [TestMethod]
        public void DataViewTeams_SelectionChanged_SelectedIndexOutOfRange_DoesNothing()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            SetPrivateField(page, "loadedTeams", new List<ApiClient.TeamReadDto>());

            teamsGrid.Rows.Add("Ghost Team", 0, 0);
            teamsGrid.Rows[0].Selected = true;

            InvokePrivateInstance(page, "DataViewTeams_SelectionChanged", teamsGrid, EventArgs.Empty);

            Assert.AreEqual(0, playersGrid.Rows.Count);
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
    }
}