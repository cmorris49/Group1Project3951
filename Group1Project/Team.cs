using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }

        public int seed { get; set; }

        public Team(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Players = new List<Player>();
        }
        public void AddPlayer(Player player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
            }
        }
        public void RemovePlayer(Guid playerId)
        {
            foreach (Player player in Players)
            {
                if (player.id == playerId)
                {
                    Players.Remove(player);
                    break;
                }
            }
        }
    }
}
