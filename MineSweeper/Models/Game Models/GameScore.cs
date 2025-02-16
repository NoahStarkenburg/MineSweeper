using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MineSweeper.Models.Game_Models
{
    public class GameScore : IComparable<GameScore>
    {
        // Class Fields
        public static int _idCounter = 1;
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

        /// <summary>
        /// Constructor for adding a GameScore object
        /// </summary>
        /// <param name="name">Name of the player</param>
        /// <param name="score">Players score</param>
        public GameScore(string name, int score)
        {
            Id = _idCounter++; 
            Name = name;
            Score = score;
            Date = DateTime.Now;
        }

        /// <summary>
        /// Constructor for reading from a file
        /// </summary>
        /// <param name="line">Each line represents a score.</param>
        public GameScore(string[] line)
        {
            Id = Convert.ToInt32(line[0]);
            Name = line[1];
            Score = Convert.ToInt32(line[2]);
            Date = Convert.ToDateTime(line[3]);
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>A String that represents the GameScore object</returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}\n", Id, Name, Score, Date);
        }

        /// <summary>
        /// Compares itself to other GameScore models to tell which one is the highest
        /// </summary>
        /// <param name="other">The other GameScore object being compaired agaainst</param>
        /// <returns>zero is the objects are the same. positive if this instance is greater. Negative is this object is less than.</returns>
        public int CompareTo(GameScore? other)
        {
            return other.Score.CompareTo(Score);
        }

        /// <summary>
        /// Checks what is the highest Id in the file and will start from that point
        /// </summary>
        /// <param name="filePath">The filepath for counter</param>
        public static void InitializeIdCounter(string filePath)
        {
            int maxId = 0;

            if (!File.Exists(filePath))
            {
                _idCounter = 1; 
                return;
            }

            // Read all lines and parse the id's
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                _idCounter = 1; 
                return;
            }

            // Parse the id's and find the maximum
            foreach (var line in lines)
            {
                // Split the line and parse the id first value
                var parts = line.Split(", ");
                if (int.TryParse(parts[0], out int id))
                {
                    if (id > maxId)
                    {
                        maxId = id;
                    }
                }
            }
            
            _idCounter = maxId + 1; 
        }
    }
}
