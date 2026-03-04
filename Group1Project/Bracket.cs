using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Bracket
    {
        public Guid Id { get; private set; }
        public BracketType BracketType { get; private set; }
        public List<Match> Matches { get; private set; }
        public int TotalRounds { get; private set; }

        public Bracket(BracketType bracketType)
        {
            Id = Guid.NewGuid();
            BracketType = bracketType;
            Matches = new List<Match>();
            TotalRounds = 0;
        }

        public void AddMatch(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            Matches.Add(match);
        }

        public void SetTotalRounds(int rounds)
        {
            TotalRounds = rounds;
        }

        /// <summary>
        /// Calculate which matches belong to which round
        /// For single elimination: Round 1 = first half, Round 2 = next quarter, etc.
        /// </summary>
        /// <param name="roundNumber"></param>
        /// <returns></returns>
        public List<Match> GetMatchesForRound(int roundNumber)
        {
            
            int matchesInRound = Matches.Count / (int)Math.Pow(2, roundNumber - 1);
            int startIndex = Matches.Count - matchesInRound * (int)Math.Pow(2, roundNumber - 1);

            return Matches.Skip(startIndex).Take(matchesInRound).ToList();
        }
    }
}
