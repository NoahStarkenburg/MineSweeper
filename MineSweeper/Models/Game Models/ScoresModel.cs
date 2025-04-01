using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Models.Game_Models
{
    public class ScoresModel : IComparable<ScoresModel>
    {
        // Class Fields
        public int Id { get; private set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public TimeSpan TimeElapsed { get; set; }
        public int NumOfClicks { get; set; }
        public int UserId { get; set; }

        /// <summary>
        /// Constructor for adding a GameScore object
        /// </summary>
        /// <param name="name">Name of the player</param>
        /// <param name="score">Players score</param>
        public ScoresModel(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public int CompareTo(ScoresModel? other)
        {
            if (other == null) return 1;
            return this.Score.CompareTo(other.Score);
        }
    }
}
