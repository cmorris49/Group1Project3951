using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Match
    {
        public Guid id { get; set; }
        public Team teamA { get; set; }
        public Team teamB { get; set; }
        public DateTime scheduledStart { get; set; }

        /// <summary>
        /// Default Match constructor with all
        /// </summary>
        /// <param name="teamA">frist team</param>
        /// <param name="teamB">second team</param>
        /// <param name="scheduledStart">scheduled start date</param>
        public Match(Team teamA, Team teamB, DateTime scheduledStart)
        {
            this.id = Guid.NewGuid();
            this.teamA = teamA;
            this.teamB = teamB;
            this.scheduledStart = scheduledStart;
        }
        public void ScheduleMatch(Team team1, Team team2, DateTime matchDate)
        {
            // Code to schedule a match between two teams on a specific date
        }
        public Team GetWinner()
        {
            // Code to determine the winner of the match based on the scores or other criteria
            return teamA;
        }
    }
}
