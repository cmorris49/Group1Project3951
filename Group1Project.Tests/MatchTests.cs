using Group1Project;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Group1Project.Tests
{
    [TestClass]
    public class MatchTests
    {
        // Helper to create teams
        private Team CreateTeam(string name)
        {
            return new Team(name);
        }

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

        [TestMethod]
        public void Constructor_UnscheduledMatch_HasUnscheduledStatus()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");

            var m = new Match(a, b);

            Assert.AreEqual(MatchStatus.Unscheduled, m.Status);
            Assert.IsNull(m.ScheduledStart);
        }

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

        [TestMethod]
        public void GetWinner_Tie_ReturnsNull()
        {
            var a = CreateTeam("A");
            var b = CreateTeam("B");
            var m = new Match(a, b, DateTime.Today.AddDays(1));

            m.SetScore(2, 2);

            Assert.IsNull(m.GetWinner());
        }

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