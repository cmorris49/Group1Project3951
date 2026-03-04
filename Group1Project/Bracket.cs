using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - Bracket Class
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Bracket class represents the structure of a tournament bracket, containing a collection of matches and providing
    /// </summary>
    internal class Bracket
    {
        /// <summary>
        /// get or sets the unique identifier for the bracket.
        /// </summary>
        public Guid id { get; set; }

        // public TourmamentFormat format { get; set; }

        /// <summary>
        /// get or sets the collection of matches associated with the bracket.
        /// This property provides access to the list of matches that are part of the tournament bracket, allowing for retrieval and
        /// manipulation of match information as needed.
        /// </summary>
        public List<Match> matches { get; set; }

        /// <summary>
        /// Initializes a new instance of the Bracket class, setting up the necessary data structures to manage the tournament matches.
        /// </summary>
        public Bracket()
        {
            matches = new List<Match>();
        }

        /// <summary>
        /// find the match in the bracket by its unique identifier, allowing for retrieval of specific match details based on the provided match ID.
        /// </summary>
        /// <param name="matchId">the match Id of the match</param>
        /// <returns>match that has the matchId </returns>
        public Match FindMatch(Guid matchId)
        {
            // Code to find a match in the bracket by its ID
            return matches.FirstOrDefault(m => m.Id == matchId);
        }

    }
}
