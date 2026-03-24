using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;
using System;
using System.Linq;

/// <summary>
/// Group 1 Project - TournamentTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for Tournament construction, division operations, and bracket generation behavior.
    /// </summary>
    [TestClass]
    public class TournamentTests
    {
        /// <summary>
        /// Verifies tournament constructor sets core properties from provided inputs.
        /// </summary>
        [TestMethod]
        public void Tournament_Constructor_SetsPropertiesCorrectly()
        {
            var name = "Test Tournament";
            var startDate = DateTime.Now.AddDays(7);
            var location = "Test Arena";

            var tournament = new Tournament(name, startDate, location);

            Assert.AreEqual(name, tournament.Name);
            Assert.AreEqual(startDate, tournament.StartDate);
            Assert.AreEqual(location, tournament.Location);
            Assert.IsNotNull(tournament.Id);
        }

        /// <summary>
        /// Verifies tournament constructor creates the default main division.
        /// </summary>
        [TestMethod]
        public void Tournament_Constructor_CreatesDefaultDivision()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");

            Assert.AreEqual(1, tournament.Divisions.Count);
            Assert.AreEqual("Main Division", tournament.Divisions[0].Name);
        }

        /// <summary>
        /// Verifies tournament constructor throws when provided an empty name.
        /// </summary>
        [TestMethod]
        public void Tournament_Constructor_ThrowsOnEmptyName()
        {
            bool exceptionThrown = false;
            try
            {
                var tournament = new Tournament("", DateTime.Now, "Location");
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected ArgumentException was not thrown");
        }

        /// <summary>
        /// Verifies creating a division adds it to the tournament division collection.
        /// </summary>
        [TestMethod]
        public void CreateDivision_AddsDivisionToTournament()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");

            var division = tournament.CreateDivision("Division 2");

            Assert.AreEqual(2, tournament.Divisions.Count);
            Assert.AreEqual("Division 2", division.Name);
        }

        /// <summary>
        /// Verifies creating a duplicate division name throws an invalid operation exception.
        /// </summary>
        [TestMethod]
        public void CreateDivision_ThrowsOnDuplicateName()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            tournament.CreateDivision("Division A");

            bool exceptionThrown = false;
            try
            {
                tournament.CreateDivision("Division A");
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected InvalidOperationException was not thrown");
        }

        /// <summary>
        /// Verifies removing a division by identifier removes it from the tournament.
        /// </summary>
        [TestMethod]
        public void RemoveDivision_RemovesDivisionFromTournament()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            var division = tournament.CreateDivision("Division 2");
            var divisionId = division.Id;

            tournament.RemoveDivision(divisionId);

            Assert.AreEqual(1, tournament.Divisions.Count);
        }

        /// <summary>
        /// Verifies bracket generation creates expected first-round match count for eight teams.
        /// </summary>
        [TestMethod]
        public void GenerateBracket_CreatesBracketWithCorrectMatches()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            var division = tournament.Divisions[0];

            for (int i = 0; i < 8; i++)
            {
                var team = new Team($"Team {i + 1}");
                team.Seed = i + 1;
                division.RegisterTeam(team);
            }

            tournament.GenerateBracket();

            Assert.IsNotNull(tournament.Bracket);
            Assert.AreEqual(4, tournament.Bracket.Matches.Count);
        }

        /// <summary>
        /// Verifies bracket generation handles odd team count without failing.
        /// </summary>
        [TestMethod]
        public void GenerateBracket_HandlesOddNumberOfTeams()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            var division = tournament.Divisions[0];

            for (int i = 0; i < 9; i++)
            {
                var team = new Team($"Team {i + 1}");
                team.Seed = i + 1;
                division.RegisterTeam(team);
            }

            tournament.GenerateBracket();

            Assert.IsNotNull(tournament.Bracket);
            Assert.AreEqual(4, tournament.Bracket.Matches.Count);
        }

        /// <summary>
        /// Verifies bracket generation throws when fewer than two teams are registered.
        /// </summary>
        [TestMethod]
        public void GenerateBracket_ThrowsWithLessThanTwoTeams()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            var division = tournament.Divisions[0];
            division.RegisterTeam(new Team("Team 1"));

            bool exceptionThrown = false;
            try
            {
                tournament.GenerateBracket();
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected InvalidOperationException was not thrown");
        }
    }
}