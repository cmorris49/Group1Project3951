using Group1Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Group1Project.Tests")]

/// <summary>
/// Group 1 Project - Tournament Class
/// Author: Cameron, Jonathan 
/// Date: March 4, 2026; Revision: 1.1
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
/// </summary>
namespace Group1Project
{
    /// <summary>
    /// Represents a tournament aggregate, including divisions, bracket configuration, and bracket generation behavior.
    /// </summary>
    internal class Tournament
    {
        /// Gets the list of divisions associated with this instance.
        /// </summary>
        /// <note>
        /// This property is read-only and is initialized during the construction of the
        /// containing class. Modifications to the list should be done through appropriate methods to maintain the
        /// integrity of the data.
        /// </note>
        private readonly List<Division> divisions;

        /// <summary>
        /// Provides access to the bracket associated with the tournament. 
        /// The bracket is generated based on the teams registered in the divisions and the selected bracket type.
        /// </summary>
        private Bracket? bracket;

        /// <summary>
        /// Initializes a new instance of the Tournament class with the specified name, start date, and location.
        /// </summary>
        /// <param name="name">Tournament's name</param>
        /// <param name="startDate">Start for Tournament</param>
        /// <param name="location">Tournament location</param>
        /// <exception cref="ArgumentException">thrown when the tournament name is null, empty, or consists only of white-space characters.</exception>
        public Tournament(string name, DateTime startDate, string location)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Tournament name is required.", nameof(name));
            }

            this.Id = Guid.NewGuid();
            this.Name = name.Trim();
            this.StartDate = startDate;
            this.Location = location?.Trim() ?? "";
            this.divisions = new List<Division>();

            // NOTE: This is a temporary division. This will go when we can add/remove devisions. 
            var defaultDivision = new Division("Main Division");
            defaultDivision.TournamentId = this.Id;
            divisions.Add(defaultDivision);
        }

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the tournament. This value cannot be null or consist only of white-space characters.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the location of the tournament. If null, an empty string is used and cannot be modified after initialization.
        /// </summary>
        public string Location { get; private set; }


        /// <summary>
        /// Provides read-only access to the collection of divisions associated with the tournament. 
        /// The list is initialized during construction and cannot be modified directly through this property. 
        /// To add or remove divisions, use the appropriate methods provided by the class.
        /// </summary>
        /// <note>
        /// The property returns an IReadOnlyList, ensuring that the collection of divisions cannot be modified directly.
        /// </note>
        public IReadOnlyList<Division> Divisions => divisions;

        /// <summary>
        /// Gets or sets the date and time when the tournament is scheduled to start. This value is set during initialization and can be modified if necessary.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the type of bracket to be used for the tournament. The default value is BracketType.SingleElimination.
        /// </summary>
        public BracketType BracketType { get; set; } = BracketType.SingleElimination;

        /// <summary>
        /// Gets the bracket associated with the tournament. The bracket is generated based on the teams registered in the divisions and the selected bracket type.
        /// </summary>
        public Bracket? Bracket
        {
            get { return bracket; }
        }

        /// <summary>
        /// Creates a new division with the specified name and adds it to the tournament. 
        /// The division name must be unique and cannot be null or consist only of white-space characters.
        /// </summary>
        /// <param name="name">Division's name</param>
        /// <returns>The newly created Division object.</returns>
        /// <exception cref="ArgumentException">thrown when the division name is null, empty, or consists only of white-space characters.</exception>
        /// <exception cref="InvalidOperationException">thrown when a division with the same name already exists in the tournament.</exception>
        public Division CreateDivision(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Division name is required.", nameof(name));
            }
                
            var trimmed = name.Trim();

            if (divisions.Any(d => string.Equals(d.Name, trimmed, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("A division with that name already exists.");
            }

            var division = new Division(trimmed);
            division.TournamentId = this.Id;
            divisions.Add(division);
            return division;
        }

        /// <summary>
        /// Removes the division with the specified identifier from the tournament. If the division is not found, an exception is thrown.
        /// </summary>
        /// <param name="divisionId"> The unique identifier of the division to remove. Must be a valid <see cref="System.Guid"/> representing an existing division in the tournament.</param>
        /// <exception cref="InvalidOperationException">thrown when a division with the specified identifier is not found in the tournament.</exception>
        public void RemoveDivision(Guid divisionId)
        {
            var index = divisions.FindIndex(d => d.Id == divisionId);
            if (index < 0)
            {
                throw new InvalidOperationException("Division not found.");
            }

            divisions.RemoveAt(index);
        }

        /// <summary>
        /// Generates the tournament bracket based on the teams registered in the divisions and the selected bracket type.
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown when there are fewer than 2 teams registered across all divisions, as a bracket cannot be generated.</exception>
        /// <exception cref="NotImplementedException">thrown when the selected bracket type is not yet implemented in the method.</exception>
        public void GenerateBracket()
        {
            var allTeams = new List<Team>();
            foreach (var division in divisions)
            {
                foreach (var team in division.Teams)
                {
                    allTeams.Add(team);
                }
            }

            if (allTeams.Count < 2)
            {
                throw new InvalidOperationException("Need at least 2 teams to generate a bracket.");
            }

            switch (BracketType)
            {
                case BracketType.SingleElimination:
                    GenerateSingleEliminationBracket(allTeams);
                    break;
                default:
                    throw new NotImplementedException($"{BracketType} bracket generation not yet implemented.");
            }
        }

        /// <summary>
        /// Generates a single-elimination bracket from the provided team list.
        /// </summary>
        /// <param name="teams">The teams used to generate first-round matchups.</param>
        private void GenerateSingleEliminationBracket(List<Team> teams)
        {
            bracket = new Bracket(BracketType.SingleElimination);

            int bracketSize = 1;
            while (bracketSize < teams.Count)
            {
                bracketSize *= 2;
            }

            int rounds = (int)Math.Log2(bracketSize);
            bracket.SetTotalRounds(rounds);

            // Sort teams by seed (if provided) and then alphabetically by name
            var seededTeams = teams
                .OrderBy(t => t.Seed == 0 ? int.MaxValue : t.Seed)
                .ThenBy(t => t.Name)
                .ToList();

            // Create matches by pairing teams sequentially
            for (int i = 0; i < seededTeams.Count - 1; i += 2)
            {
                Team teamA = seededTeams[i];
                Team teamB = seededTeams[i + 1];

                Match match = new Match(teamA, teamB);
                bracket.AddMatch(match);
            }
        }
    }
}




