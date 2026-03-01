using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Bracket
    {
        public Guid id { get; set; }
        public TourmamentFormat format { get; set; }
        public List<Match> matches { get; set; }
        public Bracket()
        {
            matches = new List<Match>();
        }
        public Match FindMatch(Guid matchId)
        {
            // Code to find a match in the bracket by its ID
            return matches.FirstOrDefault(m => m.id == matchId);
        }

    }
}
