using MineSweeper.BusinessLogic.Game_Logic;

namespace MineSweeper.Models
{
    public class GameViewModel
    {
        public GameEngine Game { get; set; }

        public GameViewModel() { }

        public GameViewModel(GameEngine game)
        {
            Game = game;
        }

        
    }
}
