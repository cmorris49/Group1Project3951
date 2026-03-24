using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

/// <summary>
/// Group 1 Project - MatchTests
/// Author: Cameron, Jun, Jonathan
/// Date: March 24, 2026; Revision: 1.0
/// Source:
///     docment on C# at https://www.w3schools.com/cs/index.php
///     MSTest info https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest
/// </summary>
namespace Group1Project.Tests
{
    /// <summary>
    /// Provides unit tests for Match scheduling, scoring, and winner resolution behavior.
    /// </summary>
    [TestClass]
    public class MatchTests
    {
        /// <summary>
        /// Creates a team instance for test setup.
        /// </summary>
        /// <param name="name">The team name.</param>
        /// <returns>A team instance with the specified name.</returns>
        private Team CreateTeam(string name)
        {
            return new Team(name);
        }

        /// <summary>
        /// Verifies the scheduled constructor initializes identifier, scheduled start, and scheduled status.
        /// </summary>
        [TestMethod]
        public void Constructor_ScheduledMatch_HasIdAndScheduledStatus()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");

            var m = new Match(a, b, DateTime.Today.AddDays(1));

            Assert.AreNotEqual(Guid.Empty, m.Id);
            Assert.AreEqual(MatchStatus.Scheduled, m.Status);
            Assert.IsNotNull(m.ScheduledStart);
        }

        /// <summary>
        /// Verifies the unscheduled constructor initializes unscheduled status and null start time.
        /// </summary>
        [TestMethod]
        public void Constructor_UnscheduledMatch_HasUnscheduledStatus()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");

            var m = new Match(a, b);

            Assert.AreEqual(MatchStatus.Unscheduled, m.Status);
            Assert.IsNull(m.ScheduledStart);
        }

        /// <summary>
        /// Verifies scheduling updates the match start date and sets status to scheduled.
        /// </summary>
        [TestMethod]
        public void Schedule_SetsScheduledStart_AndStatusScheduled()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Match(a, b);

            var date = DateTime.Today.AddDays(2);
            m.Schedule(date);

            Assert.AreEqual(MatchStatus.Scheduled, m.Status);
            Assert.AreEqual(date, m.ScheduledStart);
        }

        /// <summary>
        /// Verifies setting score on an unscheduled match throws an invalid operation exception.
        /// </summary>
        [TestMethod]
        public void SetScore_WhenUnscheduled_Throws()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Group1Project.Match(a, b);

            try
            {
                m.SetScore(1, 0);
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (InvalidOperationException)
            {
                // pass
            }
        }

        /// <summary>
        /// Verifies negative Team A score throws an argument out of range exception.
        /// </summary>
        [TestMethod]
        public void SetScore_NegativeScoreA_Throws()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Group1Project.Match(a, b, DateTime.Today.AddDays(1));

            try
            {
                m.SetScore(-1, 2);
                Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
                // pass
            }
        }

        /// <summary>
        /// Verifies negative Team B score throws an argument out of range exception.
        /// </summary>
        [TestMethod]
        public void SetScore_NegativeScoreB_Throws()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Group1Project.Match(a, b, DateTime.Today.AddDays(1));

            try
            {
                m.SetScore(2, -2);
                Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
                // pass
            }
        }

        /// <summary>
        /// Verifies valid score entry marks the match complete and returns the correct winner.
        /// </summary>
        [TestMethod]
        public void SetScore_Valid_SetsCompleteAndWinner()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Match(a, b, DateTime.Today.AddDays(1));

            m.SetScore(3, 1);

            Assert.AreEqual(MatchStatus.Complete, m.Status);
            Assert.IsNotNull(m.Score);
            Assert.IsTrue(m.IsComplete());
            Assert.AreSame(a, m.GetWinner());
        }

        /// <summary>
        /// Verifies winner resolution returns null when scores are tied.
        /// </summary>
        [TestMethod]
        public void GetWinner_Tie_ReturnsNull()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Match(a, b, DateTime.Today.AddDays(1));

            m.SetScore(2, 2);

            Assert.IsNull(m.GetWinner());
        }

        /// <summary>
        /// Verifies winner resolution returns null when no score has been recorded.
        /// </summary>
        [TestMethod]
        public void GetWinner_WithoutScore_ReturnsNull()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Match(a, b, DateTime.Today.AddDays(1));

            Assert.IsNull(m.GetWinner());
        }
    }
}