using MineSweeper.BusinessLogic.Game_Logic;

namespace MineSweeper.Models
{
    // GameViewModel.cs
    public class GameViewModel
    {
        public GameEngine Game { get; set; }
        public DateTime StartTime { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public GameViewModel(GameEngine game)
        {
            Game = game;
            StartTime = DateTime.UtcNow;
        }

        public TimeSpan GetElapsedTime()
        {
            return DateTime.UtcNow - StartTime;
        }
    }

}
