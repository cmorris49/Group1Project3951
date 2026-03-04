using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/// <summary>
/// Group 1 Project - Tournament Management System
/// Author: Jonathan, Cameron, Jun
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
///     GUID info https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-10.0
/// </summary>
namespace Group1Project
{
    internal class Tournament
    {
        /// <summary>
        /// Gets the list of divisions associated with this instance.
        /// </summary>
        /// <note>
        /// This property is read-only and is initialized during the construction of the
        /// containing class. Modifications to the list should be done through appropriate methods to maintain the
        /// integrity of the data.
        /// </note>
        private readonly List<Division> divisions;

        /// <summary>
        /// Initializes a new instance of the Tournament class with the specified name, start date, and location.
        /// </summary>
        /// <param name="name">The name of the tournament. This value cannot be null or consist only of white-space characters.</param>
        /// <param name="startDate">The date and time when the tournament is scheduled to start.</param>
        /// <param name="location">The location where the tournament will take place. If null, an empty string is used.</param>
        /// <exception cref="ArgumentException">Thrown if the name parameter is null or consists only of white-space characters.</exception>
        public Tournament(string name, DateTime startDate, string location)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tournament name is required.", nameof(name));

            this.Id = Guid.NewGuid();
            this.Name = name.Trim();
            this.StartDate = startDate;
            this.Location = location?.Trim() ?? "";
            this.divisions = new List<Division>();

            // NOTE: This is a temporary division. This will go when we can add/remove devisions. 
            var defaultDivision = new Division("Main Division");
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
        /// Creates a new division with the specified name.
        /// </summary>
        /// <param name="name">The name of the division to create. Cannot be null, empty, or consist only of white-space characters.</param>
        /// <returns>A Division object representing the newly created division.</returns>
        /// <exception cref="ArgumentException">Thrown if the name parameter is null, empty, or consists only of white-space characters.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a division with the specified name already exists.</exception>
        public Division CreateDivision(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Division name is required.", nameof(name));

            var trimmed = name.Trim();

            if (divisions.Any(d => string.Equals(d.Name, trimmed, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("A division with that name already exists.");

            var division = new Division(trimmed);
            divisions.Add(division);
            return division;
        }

        /// <summary>
        /// Removes the division with the specified identifier from the collection of divisions.
        /// </summary>
        /// <param name="divisionId">The unique identifier of the division to be removed.</param>
        /// <exception cref="InvalidOperationException">Thrown if no division with the specified identifier exists in the collection.</exception>
        public void RemoveDivision(Guid divisionId)
        {
            var index = divisions.FindIndex(d => d.Id == divisionId);
            if (index < 0)
                throw new InvalidOperationException("Division not found.");

            divisions.RemoveAt(index);
        }

        /*
        public void SetRuleSet(RuleSet rules)
        {
            ruleSet = rules ?? throw new ArgumentNullException(nameof(rules));
        }
        */
    }
}
