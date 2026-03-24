using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;

/// <summary>
/// Group 1 Project - DivisionTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for Division construction and team registration behavior.
    /// </summary>
    [TestClass]
    public class DivisionTests
    {
        /// <summary>
        /// Verifies division constructor sets the provided name and assigns an identifier.
        /// </summary>
        [TestMethod]
        public void Division_Constructor_SetsNameCorrectly()
        {
            var divisionName = "Main Division";

            var division = new Division(divisionName);

            Assert.AreEqual(divisionName, division.Name);
            Assert.IsNotNull(division.Id);
        }

        /// <summary>
        /// Verifies registering a team adds it to the division team collection.
        /// </summary>
        [TestMethod]
        public void RegisterTeam_AddsTeamToDivision()
        {
            var division = new Division("Main Division");
            var team = new Team("Warriors");

            division.RegisterTeam(team);

            Assert.AreEqual(1, division.Teams.Count);
            Assert.AreEqual(team, division.Teams[0]);
        }

        /// <summary>
        /// Verifies registering the same team instance twice does not create duplicates.
        /// </summary>
        [TestMethod]
        public void RegisterTeam_DoesNotAddDuplicateTeam()
        {
            var division = new Division("Main Division");
            var team = new Team("Warriors");

            division.RegisterTeam(team);
            division.RegisterTeam(team);

            Assert.AreEqual(1, division.Teams.Count);
        }
    }
}
