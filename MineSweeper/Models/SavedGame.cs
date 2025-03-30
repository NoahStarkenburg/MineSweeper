using Org.BouncyCastle.Asn1.Cms;

namespace MineSweeper.Models
{
    public class SavedGame
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateSaved { get; set; } = DateTime.Now;
        public string GameData { get; set; }
        public int TimePlayed { get; set; }
    }
}
