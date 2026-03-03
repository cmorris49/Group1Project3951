using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    public class Match
    {
        public Guid Id { get; private set; }
        public Team TeamA { get; private set; }
        public Team TeamB { get; private set; }

        // If you don't schedule, keep it nullable.
        public DateTime? ScheduledStart { get; private set; }

        public MatchStatus Status { get; private set; }

        public ScoreEntry? Score { get; private set; }

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
        public void SetScore(int scoreA, int scoreB)
        {
            if (Status == MatchStatus.Unscheduled)
                throw new InvalidOperationException("Cannot set score for an unscheduled match.");

            Score = new ScoreEntry(scoreA, scoreB);
            Status = MatchStatus.Complete;
        }

        /// <summary>
        /// Returns the winner team.
        /// - If score not set => null
        /// - If tie => null
        /// </summary>
        public Team? GetWinner()
        {
            if (Score is null) return null;

            if (Score.ScoreA > Score.ScoreB) return TeamA;
            if (Score.ScoreB > Score.ScoreA) return TeamB;

            return null; // tie
        }

        public bool IsComplete() => Status == MatchStatus.Complete;
    }
}
