using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Schedule
    {
        public List<Match> matches { get; set; }
        public void AddMatch(Match match)
        {
            // Code to add a match to the schedule
            matches.Add(match);
        }
    }
}
