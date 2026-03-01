using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Division
    {
        public Guid id { get; set; }
        public string name { get; set; }

        public void RegisterTeam(Team team)
        {
            // Code to register a team in the division
        }

        public void UnregisterTeam(Guid teamId) {
            // Code to unregister a team from the division
        }
    }
}
