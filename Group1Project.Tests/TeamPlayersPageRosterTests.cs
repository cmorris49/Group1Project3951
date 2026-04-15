using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Group 1 Project - TeamsPlayersPageRosterTests
/// Author: Cameron, Jun, Jonathan
/// Date: April 9, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for team/player selection logic used by add/remove/edit actions.
    /// </summary>
    [TestClass]
    public class TeamsPlayersPageRosterTests
    {
        /// <summary>
        /// Verifies selected-team resolver returns false when no team row is selected.
        /// </summary>
        [TestMethod]
        public void TryGetSelectedTeam_ReturnsFalse_WhenNoSelection()
        {
            var page = new TeamsPlayersPage();
            SetPrivateField(page, "loadedTeams", BuildLoadedTeams());

            var args = new object?[] { null, -1 };
            var result = (bool)InvokePrivateInstance(page, "TryGetSelectedTeam", args)!;

            Assert.IsFalse(result);
            Assert.IsNull(args[0]);
            Assert.AreEqual(-1, (int)args[1]!);
        }

        /// <summary>
        /// Verifies selected-team resolver returns the expected team and index for a valid selection.
        /// </summary>
        [TestMethod]
        public void TryGetSelectedTeam_ReturnsTrue_WhenValidSelection()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");

            var loadedTeams = BuildLoadedTeams();
            SetPrivateField(page, "loadedTeams", loadedTeams);

            teamsGrid.Rows.Add("Warriors", 1, 2);
            teamsGrid.Rows[0].Selected = true;

            var args = new object?[] { null, -1 };
            var result = (bool)InvokePrivateInstance(page, "TryGetSelectedTeam", args)!;

            Assert.IsTrue(result);
            Assert.IsNotNull(args[0]);
            Assert.AreEqual(0, (int)args[1]!);

            var selectedTeam = (ApiClient.TeamReadDto)args[0]!;
            Assert.AreEqual("Warriors", selectedTeam.Name);
        }

        /// <summary>
        /// Verifies selected-player resolver returns false when no player row is selected.
        /// </summary>
        [TestMethod]
        public void TryGetSelectedPlayer_ReturnsFalse_WhenNoPlayerSelection()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            SetPrivateField(page, "loadedTeams", BuildLoadedTeams());

            teamsGrid.Rows.Add("Warriors", 1, 2);
            teamsGrid.Rows[0].Selected = true;

            playersGrid.Rows.Add("Alice", 7);
            playersGrid.Rows.Add("Bob", 11);

            var args = new object?[] { null, null, -1, -1 };
            var result = (bool)InvokePrivateInstance(page, "TryGetSelectedPlayer", args)!;

            Assert.IsFalse(result);
            Assert.IsNull(args[1]);
        }

        /// <summary>
        /// Verifies selected-player resolver returns team/player values for valid team and player selection.
        /// </summary>
        [TestMethod]
        public void TryGetSelectedPlayer_ReturnsTrue_WhenValidSelection()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            SetPrivateField(page, "loadedTeams", BuildLoadedTeams());

            teamsGrid.Rows.Add("Warriors", 1, 2);
            teamsGrid.Rows[0].Selected = true;

            playersGrid.Rows.Add("Alice", 7);
            playersGrid.Rows.Add("Bob", 11);
            playersGrid.Rows[1].Selected = true;

            var args = new object?[] { null, null, -1, -1 };
            var result = (bool)InvokePrivateInstance(page, "TryGetSelectedPlayer", args)!;

            Assert.IsTrue(result);
            Assert.AreEqual(0, (int)args[2]!);
            Assert.AreEqual(1, (int)args[3]!);

            var selectedPlayer = (ApiClient.PlayerReadDto)args[1]!;
            Assert.AreEqual("Bob", selectedPlayer.DisplayName);
            Assert.AreEqual(11, selectedPlayer.Number);
        }

        /// <summary>
        /// Verifies selected-player resolver returns false when selected player index is outside player list bounds.
        /// </summary>
        [TestMethod]
        public void TryGetSelectedPlayer_ReturnsFalse_WhenPlayerIndexOutOfRange()
        {
            var page = new TeamsPlayersPage();
            var teamsGrid = GetPrivateField<DataGridView>(page, "dataViewTeams");
            var playersGrid = GetPrivateField<DataGridView>(page, "dataGridViewPlayers");

            SetPrivateField(page, "loadedTeams", BuildLoadedTeams());

            teamsGrid.Rows.Add("Warriors", 1, 2);
            teamsGrid.Rows[0].Selected = true;

            playersGrid.Rows.Add("Placeholder", 99);
            playersGrid.Rows.Add("Placeholder", 100);
            playersGrid.Rows.Add("Placeholder", 101);
            playersGrid.Rows[2].Selected = true;

            var args = new object?[] { null, null, -1, -1 };
            var result = (bool)InvokePrivateInstance(page, "TryGetSelectedPlayer", args)!;

            Assert.IsFalse(result);
            Assert.IsNull(args[1]);
        }

        /// <summary>
        /// Builds deterministic loaded-team test data with two players.
        /// </summary>
        private static List<ApiClient.TeamReadDto> BuildLoadedTeams()
        {
            return new List<ApiClient.TeamReadDto>
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
        /// Invokes a private instance method with explicit argument array support.
        /// </summary>
        private static object? InvokePrivateInstance(object instance, string methodName, object?[] args)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, $"Method '{methodName}' not found.");
            return method!.Invoke(instance, args);
        }
    }
}