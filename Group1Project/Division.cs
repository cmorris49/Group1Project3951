using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Division
    {
        private List<Team> teams = new List<Team>();

        public Division(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; private set; }

        public IReadOnlyList<Team> Teams
        {
            get { return teams; }
        }

        public void RegisterTeam(Team team)
        {
            if (!teams.Contains(team))
            {
                teams.Add(team);
            }
        }

        public void UnregisterTeam(Guid teamId) {
            // Code to unregister a team from the division
        }
    }
}
