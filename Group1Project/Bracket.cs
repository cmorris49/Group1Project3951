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
        /// Unique identifier for the bracket, generated as a new GUID when a Bracket instance is created. 
        /// This property  cannot be modified after initialization, ensuring that each bracket has a distinct and immutable identifier throughout its lifecycle.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the type of the bracket, which determines the structure of the tournament (e.g., single elimination, double elimination, round robin).
        /// </summary>
        public BracketType BracketType { get; private set; }

        /// <summary>
        /// Gets the list of matches that are part of the bracket. This collection represents the individual matchups between teams or players in the tournament.
        /// </summary>
        public List<Match> Matches { get; private set; }

        /// <summary>
        /// Gets the total number of rounds in the bracket, which is determined based on the number of matches and the structure of the tournament. 
        /// This value can be set using the SetTotalRounds method, which ensures that the total rounds are calculated correctly according to the tournament format.
        /// </summary>
        public int TotalRounds { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Bracket class with the specified bracket type. 
        /// The constructor generates a unique identifier for the bracket, sets the bracket type, initializes the list of matches, and sets the total rounds to zero.
        /// </summary>
        /// <param name="bracketType">BracketType enum value representing the type of bracket to be used for the tournament. This parameter is required to determine the structure of the tournament.</param>
        public Bracket(BracketType bracketType)
        {
            Id = Guid.NewGuid();
            BracketType = bracketType;
            Matches = new List<Match>();
            TotalRounds = 0;
        }

        /// <summary>
        /// Sets the total number of rounds in the bracket based on the number of matches and the structure of the tournament.
        /// </summary>
        /// <param name="rounds"> The total number of rounds in the bracket. Must be a non-negative integer representing the number of rounds in the tournament structure.</param>
        /// <exception cref="ArgumentOutOfRangeException">thrown when the provided rounds value is negative, as total rounds must be a non-negative integer.</exception>
        public void SetTotalRounds(int rounds)
        {
            if (rounds < 0)
                throw new ArgumentOutOfRangeException(nameof(rounds), "Total rounds must be a non-negative integer.");
            TotalRounds = rounds;
        }

        /// <summary>
        /// Adds a match to the bracket's collection of matches. The match must not be null, and it will be added to the list of matches that make up the tournament structure.
        /// </summary>
        /// <param name="match">The match to be added to the bracket. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided match is null, as a valid match must be provided to add to the bracket.</exception>
        public void AddMatch(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            Matches.Add(match);
        }

        /// <summary>
        /// Calculate which matches belong to which round
        /// For single elimination: Round 1 = first half, Round 2 = next quarter, etc.
        /// </summary>
        /// <param name="roundNumber"></param>
        /// <returns></returns>
        public List<Match> GetMatchesForRound(int roundNumber)
        {
            
            int matchesInRound = Matches.Count / (int)Math.Pow(2, roundNumber - 1);
            int startIndex = Matches.Count - matchesInRound * (int)Math.Pow(2, roundNumber - 1);

            return Matches.Skip(startIndex).Take(matchesInRound).ToList();
        }
    }
}
