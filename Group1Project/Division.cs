using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    internal class Division
    {
        public Division(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; private set; }

        public void RegisterTeam(Team team)
        {
            // Code to register a team in the division
        }

        public void UnregisterTeam(Guid teamId) {
            // Code to unregister a team from the division
        }
    }
}
