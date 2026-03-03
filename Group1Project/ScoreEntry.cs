using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group1Project
{
    public class ScoreEntry
    {
        public int ScoreA { get; private set; }
        public int ScoreB { get; private set; }
        public DateTime RecordedAt { get; private set; }

        public ScoreEntry(int scoreA, int scoreB)
        {
            if (scoreA < 0) throw new ArgumentOutOfRangeException(nameof(scoreA), "ScoreA cannot be negative.");
            if (scoreB < 0) throw new ArgumentOutOfRangeException(nameof(scoreB), "ScoreB cannot be negative.");

            ScoreA = scoreA;
            ScoreB = scoreB;
            RecordedAt = DateTime.Now;
        }
    }
}