using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;

namespace Group1Project.Tests
{
    [TestClass]
    public class DivisionTests
    {
        [TestMethod]
        public void Division_Constructor_SetsNameCorrectly()
        {
            var divisionName = "Main Division";

            var division = new Division(divisionName);

            Assert.AreEqual(divisionName, division.Name);
            Assert.IsNotNull(division.Id);
        }

        [TestMethod]
        public void RegisterTeam_AddsTeamToDivision()
        {
            var division = new Division("Main Division");
            var team = new Team("Warriors");

            division.RegisterTeam(team);

            Assert.AreEqual(1, division.Teams.Count);
            Assert.AreEqual(team, division.Teams[0]);
        }

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
