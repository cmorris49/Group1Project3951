using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;

/// <summary>
/// Group 1 Project - TeamTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for Team construction and roster management behavior.
    /// </summary>
    [TestClass]
    public class TeamTests
    {
        /// <summary>
        /// Verifies team constructor sets name, identifier, and initializes empty player collection.
        /// </summary>
        [TestMethod]
        public void Team_Constructor_SetsNameCorrectly()
        {
            var teamName = "Warriors";

            var team = new Team(teamName);

            Assert.AreEqual(teamName, team.Name);
            Assert.IsNotNull(team.Id);
            Assert.AreEqual(0, team.Players.Count);
        }

        /// <summary>
        /// Verifies adding a player stores the player in the team roster.
        /// </summary>
        [TestMethod]
        public void AddPlayer_AddsPlayerToTeam()
        {
            var team = new Team("Warriors");
            var player = new Player("John Doe", 10);

            team.AddPlayer(player);

            Assert.AreEqual(1, team.Players.Count);
            Assert.AreEqual(player, team.Players[0]);
        }

        /// <summary>
        /// Verifies adding the same player instance twice does not create duplicates.
        /// </summary>
        [TestMethod]
        public void AddPlayer_DoesNotAddDuplicatePlayer()
        {
            var team = new Team("Warriors");
            var player = new Player("John Doe", 10);

            team.AddPlayer(player);
            team.AddPlayer(player);

            Assert.AreEqual(1, team.Players.Count);
        }

        /// <summary>
        /// Verifies removing a player by identifier removes that player from the roster.
        /// </summary>
        [TestMethod]
        public void RemovePlayer_RemovesPlayerFromTeam()
        {
            var team = new Team("Warriors");
            var player = new Player("John Doe", 10);
            team.AddPlayer(player);

            team.RemovePlayer(player.Id);

            Assert.AreEqual(0, team.Players.Count);
        }
    }
}
