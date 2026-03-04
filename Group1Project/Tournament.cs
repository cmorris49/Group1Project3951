using Group1Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Group1Project.Tests")]

namespace Group1Project
{
    internal class Tournament
    {
        private readonly List<Division> divisions;

        private Bracket? bracket;

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

        public BracketType BracketType { get; set; } = BracketType.SingleElimination;

        public Bracket? Bracket
        {
            get { return bracket; }
        }

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
                throw new InvalidOperationException("Need at least 2 teams to generate a bracket.");

            switch (BracketType)
            {
                case BracketType.SingleElimination:
                    GenerateSingleEliminationBracket(allTeams);
                    break;
                default:
                    throw new NotImplementedException($"{BracketType} bracket generation not yet implemented.");
            }
        }

        private void GenerateSingleEliminationBracket(List<Team> teams)
        {
            bracket = new Bracket(BracketType.SingleElimination);

            int bracketSize = 1;
            while (bracketSize < teams.Count)
                bracketSize *= 2;

            int rounds = (int)Math.Log2(bracketSize);
            bracket.SetTotalRounds(rounds);

            var seededTeams = teams
                .OrderBy(t => t.seed == 0 ? int.MaxValue : t.seed)
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

            // If odd number of teams
        }

        public static Tournament CreateTestTournament()
        {
            var tournament = new Tournament("Test Tournament 2026", DateTime.Now.AddDays(7), "Test Arena");

            var division = tournament.Divisions[0];

            string[] teamNames = { "Warriors", "Knights", "Dragons", "Phoenix", "Tigers", "Eagles", "Sharks", "Lions" };

            for (int teamIndex = 0; teamIndex < 8; teamIndex++)
            {
                var team = new Team(teamNames[teamIndex]);
                team.seed = teamIndex + 1; 

                for (int playerIndex = 1; playerIndex <= 5; playerIndex++)
                {
                    var player = new Player($"Player {playerIndex}", playerIndex + (teamIndex * 10));
                    team.AddPlayer(player);
                }

                division.RegisterTeam(team);
            }

            return tournament;
        }
    }
}





