using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;

namespace Group1Project.Tests
{
    [TestClass]
    public class BracketTests
    {
        [TestMethod]
        public void Bracket_Constructor_SetsBracketTypeCorrectly()
        {
            var bracket = new Bracket(BracketType.SingleElimination);

            Assert.AreEqual(BracketType.SingleElimination, bracket.BracketType);
            Assert.IsEmpty(bracket.Matches);
            Assert.AreEqual(0, bracket.TotalRounds);
        }

        [TestMethod]
        public void AddMatch_AddsMatchToBracket()
        {
            var bracket = new Bracket(BracketType.SingleElimination);
            var team1 = new Team("Team 1");
            var team2 = new Team("Team 2");
            var match = new Match(team1, team2);

            bracket.AddMatch(match);

            Assert.HasCount(1, bracket.Matches);
            Assert.AreEqual(match, bracket.Matches[0]);
        }

        [TestMethod]
        public void SetTotalRounds_UpdatesTotalRounds()
        {
            var bracket = new Bracket(BracketType.SingleElimination);

            bracket.SetTotalRounds(3);

            Assert.AreEqual(3, bracket.TotalRounds);
        }
    }
}
