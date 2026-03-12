using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - Team Class
/// Author: Jonathan, Cameron, Jun
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     GUID info https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-10.0
/// </summary>
namespace Group1Project
{
    public class Team
    {
        /// <summary>
        /// gets or sets the unique identifier for the team.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of players participating in the team.
        /// </summary>
        public List<Player> Players { get; set; }


        /// <summary>
        /// Gets or sets the seed value used for random number generation.
        /// </summary>
        public int seed { get; set; }

        /// <summary>
        /// Initializes a new instance of the Team class with the specified team name.
        /// </summary>
        /// <param name="name">The name of the team. Cannot be null or empty.</param>
        public Team(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Players = new List<Player>();
        }

        /// <summary>
        /// Adds the specified player to the team if they are not already present.
        /// </summary>
        /// <param name="player">The player to add to the team. Cannot be null.</param>
        public void AddPlayer(Player player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
            }
        }

        /// <summary>
        /// Removes the player with the specified unique identifier from the collection of players.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player to remove from the collection.</param>
        public void RemovePlayer(Guid playerId)
        {
            foreach (Player player in Players)
            {
                if (player.id == playerId)
                {
                    Players.Remove(player);
                    return;
                }
            }
            MessageBox.Show("This Player doesn't exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
