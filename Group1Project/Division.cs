using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - Division Class
/// Author: Cameron, Jun, Jonathan 
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     Window form info http://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=visualstudio
/// </summary>
namespace Group1Project
{
    internal class Division
    {
        /// <summary>
        /// Store the list of teams associated with the division.
        /// </summary>
        private List<Team> teams = new List<Team>();

        /// <summary>
        /// Initializes a new instance of the Division class with the specified name.
        /// </summary>
        /// <param name="name">The name of the division. Cannot be null or empty.</param>
        public Division(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// get the name of the division and cannot be modified after initialization.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the list of teams associated with the current context.
        /// </summary>
        /// <note>
        /// This property provides read-only access to the collection of teams. The list is
        /// immutable, ensuring that the teams cannot be modified directly through this property.
        /// </note>
        public IReadOnlyList<Team> Teams
        {
            get { return teams; }
        }

        /// <summary>
        /// Registers the specified team if it is not already present in the collection.
        /// </summary>
        /// <param name="team">The team to register. Cannot be null. The team must represent a valid team instance.</param>
        public void RegisterTeam(Team team)
        {
            if (!teams.Contains(team))
            {
                teams.Add(team);
            }
        }

        /// <summary>
        /// Removes the team with the specified identifier from the division.
        /// </summary>
        /// <param name="teamId">The unique identifier of the team to remove. Must be a valid <see cref="System.Guid"/> representing an
        /// existing team in the division.</param>
        public void UnregisterTeam(Guid teamId) {
            // Code to unregister a team from the division
        }
    }
}
