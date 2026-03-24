using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - Match Class
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents a matchup between two teams, including schedule, status, and score information.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Match constructor (initially scheduled).
        /// </summary>
        public Match(Team teamA, Team teamB, DateTime scheduledStart)
        {
            Id = Guid.NewGuid();
            TeamA = teamA ?? throw new ArgumentNullException(nameof(teamA));
            TeamB = teamB ?? throw new ArgumentNullException(nameof(teamB));
            ScheduledStart = scheduledStart;
            Status = MatchStatus.Scheduled;
        }

        /// <summary>
        /// Match constructor (unscheduled).
        /// </summary>
        public Match(Team teamA, Team teamB)
        {
            Id = Guid.NewGuid();
            TeamA = teamA ?? throw new ArgumentNullException(nameof(teamA));
            TeamB = teamB ?? throw new ArgumentNullException(nameof(teamB));
            ScheduledStart = null;
            Status = MatchStatus.Unscheduled;
        }

        // Unique identifier for the match.
        public Guid Id { get; private set; }

        // Team A involved in the match. Cannot be null.
        public Team TeamA { get; private set; }

        // Team B involved in the match. Cannot be null.
        public Team TeamB { get; private set; }

        // If you don't schedule, keep it nullable.
        public DateTime? ScheduledStart { get; private set; }

        // Match status: Unscheduled, Scheduled, Complete
        public MatchStatus Status { get; private set; }

        // Score is null until set. Once set, it cannot be modified.
        public ScoreEntry? Score { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchDate"></param>
        public void Schedule(DateTime matchDate)
        {
            ScheduledStart = matchDate;
            Status = MatchStatus.Scheduled;
        }

        /// <summary>
        /// Sets the score for the match.
        /// - Scores cannot be negative.
        /// - Match must be scheduled first.
        /// - After setting score, Status becomes Complete.
        /// </summary>
        /// <param name="scoreA">Score for Team A. Must be non-negative.</param>
        /// <param name="scoreB">Score for Team B. Must be non-negative.</param>
        /// <exception cref="InvalidOperationException">Thrown if match is unscheduled.</exception>
        public void SetScore(int scoreA, int scoreB)
        {
            if (Status == MatchStatus.Unscheduled)
            {
                throw new InvalidOperationException("Cannot set score for an unscheduled match.");
            }

            Score = new ScoreEntry(scoreA, scoreB);
            Status = MatchStatus.Complete;
        }

        /// <summary>
        /// Returns the winner team.
        /// - If score not set => null
        /// - If tie => null
        /// </summary>
        /// <returns>The winning team, or null if there is no winner.</returns>
        public Team? GetWinner()
        {
            if (Score is null)
            {
                return null;
            }

            if (Score.ScoreA > Score.ScoreB)
            {
                return TeamA;
            }
            if (Score.ScoreB > Score.ScoreA)
            {
                return TeamB;
            }

            return null; // tie
        }

        /// <summary>
        /// checks if the match status is complete.
        /// </summary>
        /// <returns>True if the match is complete</returns>
        public bool IsComplete() => Status == MatchStatus.Complete;
    }
}
