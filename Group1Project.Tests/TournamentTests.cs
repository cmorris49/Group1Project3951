using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;
using System;
using System.Linq;

namespace Group1Project.Tests
{
    [TestClass]
    public class TournamentTests
    {
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

        [TestMethod]
        public void Tournament_Constructor_CreatesDefaultDivision()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");

            Assert.AreEqual(1, tournament.Divisions.Count);
            Assert.AreEqual("Main Division", tournament.Divisions[0].Name);
        }

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

        [TestMethod]
        public void CreateDivision_AddsDivisionToTournament()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");

            var division = tournament.CreateDivision("Division 2");

            Assert.AreEqual(2, tournament.Divisions.Count);
            Assert.AreEqual("Division 2", division.Name);
        }

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

        [TestMethod]
        public void RemoveDivision_RemovesDivisionFromTournament()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            var division = tournament.CreateDivision("Division 2");
            var divisionId = division.Id;

            tournament.RemoveDivision(divisionId);

            Assert.AreEqual(1, tournament.Divisions.Count);
        }

        [TestMethod]
        public void GenerateBracket_CreatesBracketWithCorrectMatches()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            var division = tournament.Divisions[0];

            for (int i = 0; i < 8; i++)
            {
                var team = new Team($"Team {i + 1}");
                team.seed = i + 1;
                division.RegisterTeam(team);
            }

            tournament.GenerateBracket();

            Assert.IsNotNull(tournament.Bracket);
            Assert.AreEqual(4, tournament.Bracket.Matches.Count);
        }

        [TestMethod]
        public void GenerateBracket_HandlesOddNumberOfTeams()
        {
            var tournament = new Tournament("Test", DateTime.Now, "Location");
            var division = tournament.Divisions[0];

            for (int i = 0; i < 9; i++)
            {
                var team = new Team($"Team {i + 1}");
                team.seed = i + 1;
                division.RegisterTeam(team);
            }

            tournament.GenerateBracket();

            Assert.IsNotNull(tournament.Bracket);
            Assert.AreEqual(4, tournament.Bracket.Matches.Count);
        }

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