using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - Player Class
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
/// </summary>
namespace Group1Project
{
    public class Player
    {
        /// <summary>
        /// gets or sets the unique identifier for the player.
        /// </summary>
        public Guid Id { get; set; }

        public Guid TeamId { get; set; }

        /// <summary>
        /// gets or sets the display name of the player.
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// gets or sets the number associated with the player, which may represent their jersey number or another identifier.
        public int number { get; set; }

        /// <summary>
        /// Initializes a new instance of the Player class with the specified display name and number, assigning a unique identifier to the player.
        /// </summary>
        /// <param name="name">Player's name</param>
        /// <param name="number">Player's number</param>
        public Player(string name, int number) {
            this.Id = Guid.NewGuid();
            this.displayName = name;
            this.number = number;
        }
    }
}
