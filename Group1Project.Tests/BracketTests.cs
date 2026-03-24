using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group1Project;

/// <summary>
/// Group 1 Project - BracketTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for Bracket initialization and core mutation behavior.
    /// </summary>
    [TestClass]
    public class BracketTests
    {
        /// <summary>
        /// Verifies bracket constructor initializes bracket type, empty matches, and zero rounds.
        /// </summary>
        [TestMethod]
        public void Bracket_Constructor_SetsBracketTypeCorrectly()
        {
            var bracket = new Bracket(BracketType.SingleElimination);

            Assert.AreEqual(BracketType.SingleElimination, bracket.BracketType);
            Assert.IsEmpty(bracket.Matches);
            Assert.AreEqual(0, bracket.TotalRounds);
        }

        /// <summary>
        /// Verifies adding a match stores the match in the bracket collection.
        /// </summary>
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

        /// <summary>
        /// Verifies setting total rounds updates the total round count.
        /// </summary>
        [TestMethod]
        public void SetTotalRounds_UpdatesTotalRounds()
        {
            var bracket = new Bracket(BracketType.SingleElimination);

            bracket.SetTotalRounds(3);

            Assert.AreEqual(3, bracket.TotalRounds);
        }
    }
}
