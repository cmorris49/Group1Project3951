using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Group 1 Project - Player Class
/// Author: Cameron
/// Date: March 4, 2026; Revision: 1.0
/// Source: 
///     docment on C# at https://www.w3schools.com/cs/index.php
/// </summary>
namespace Group1Project
{
    internal class RuleSet
    {
        // Unique identifier for the RuleSet, generated automatically upon instantiation.
        public Guid Id { get; } = Guid.NewGuid();

        // get or set the name of the rule set, which may be used to identify or describe the rules being applied. It is initialized to an empty string by default.
        public string Name { get; set; } = "";
    }
}
