using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - Schedule Class
/// Author: Cameron, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents a collection of tournament matches used for scheduling operations.
    /// </summary>
    internal class Schedule
    {
        // Store the list of matches in the schedule.
        public List<Match> matches { get; set; }

        /// <summary>
        /// Initializes a new instance of the Schedule class with an empty list of matches.
        /// </summary>
        /// <param name="match"> The list of matches to initialize the schedule with.</param>
        public void AddMatch(Match match)
        {
            // Code to add a match to the schedule
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match Doesn't exist.");
            }
            matches.Add(match);
        }
    }
}
