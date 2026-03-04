using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Group1Project
{
    internal class Tournament
    {
        private readonly List<Division> divisions;
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
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; private set; }

        public IReadOnlyList<Division> Divisions => divisions;

        public DateTime StartDate { get; set; }

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
