using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - ScoreEntry class
/// Author: Cameron, Jun, Jonathan 
/// Date: March 3, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
/// </summary>
namespace Group1Project
{
    public class ScoreEntry
    {
        // Score for Team A. Cannot be negative and cannot be modified after initialization.
        public int ScoreA { get; private set; }

        // Score for Team B. Cannot be negative and cannot be modified after initialization.
        public int ScoreB { get; private set; }

        /// <summary>
        /// Gets the date and time when the record was created.
        /// </summary>
        /// <note>
        /// This property is set to the current date and time when the record is created and
        /// cannot be modified afterward.
        /// </note>
        public DateTime RecordedAt { get; private set; }

        /// <summary>
        /// ScoreEntry constructor that initializes the scores for Team A and Team B, and sets the RecordedAt property to the current date and time.
        /// </summary>
        /// <param name="scoreA">Team A score</param>
        /// <param name="scoreB">Team B score</param>
        /// <exception cref="ArgumentOutOfRangeException"> thrown when a negative score is given</exception>
        public ScoreEntry(int scoreA, int scoreB)
        {
            if (scoreA < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scoreA), "ScoreA cannot be negative.");
            }
            if (scoreB < 0) 
            { 
                throw new ArgumentOutOfRangeException(nameof(scoreB), "ScoreB cannot be negative.");
            }

            ScoreA = scoreA;
            ScoreB = scoreB;
            RecordedAt = DateTime.Now;
        }
    }
}