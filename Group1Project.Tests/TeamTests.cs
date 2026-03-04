using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;

namespace Group1Project.Tests
{
    [TestClass]
    public class TeamTests
    {
        [TestMethod]
        public void Team_Constructor_SetsNameCorrectly()
        {
            var teamName = "Warriors";

            var team = new Team(teamName);

            Assert.AreEqual(teamName, team.Name);
            Assert.IsNotNull(team.Id);
            Assert.AreEqual(0, team.Players.Count);
        }

        [TestMethod]
        public void AddPlayer_AddsPlayerToTeam()
        {
            var team = new Team("Warriors");
            var player = new Player("John Doe", 10);

            team.AddPlayer(player);

            Assert.AreEqual(1, team.Players.Count);
            Assert.AreEqual(player, team.Players[0]);
        }

        [TestMethod]
        public void AddPlayer_DoesNotAddDuplicatePlayer()
        {
            var team = new Team("Warriors");
            var player = new Player("John Doe", 10);

            team.AddPlayer(player);
            team.AddPlayer(player);

            Assert.AreEqual(1, team.Players.Count);
        }

        [TestMethod]
        public void RemovePlayer_RemovesPlayerFromTeam()
        {
            var team = new Team("Warriors");
            var player = new Player("John Doe", 10);
            team.AddPlayer(player);

            team.RemovePlayer(player.id);

            Assert.AreEqual(0, team.Players.Count);
        }
    }
}
