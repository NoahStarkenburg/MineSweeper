using MineSweeper.BusinessLogic.Game_Logic;
using MineSweeper.BusinessLogic.LogicInterfaces;
using MineSweeper.Models;
using MineSweeper.Models.DAOs;
using MineSweeper.Models.DAOs.InterfacesDAOs;
using MineSweeper.Models.Game_Models;
using Newtonsoft.Json;

namespace MineSweeper.BusinessLogic
{
    public class SavedGamesService : ISavedGamesService
    {
        public ISavedGamesDAO _savedGamesDAO;
        
        public SavedGamesService(ISavedGamesDAO savedGamesDAO)
        {
            _savedGamesDAO = savedGamesDAO;
        }

        public async Task<GameViewModel?> LoadGame(string userId, int savedGameId)
        {
            // Retrieve the saved game for the current user by the saved game's ID
            SavedGame savedGame = await _savedGamesDAO.GetSavedGameById(savedGameId);

            if (savedGame == null || savedGame.UserId.ToString() != userId)
            {
                return null;
            }
            // Deserialize the saved game data (Board state)
            Board board = JsonConvert.DeserializeObject<Board>(savedGame.GameData);

            // Initialize the game engine with the deserialized board
            GameEngine gameEngine = new GameEngine(board, savedGame.TimePlayed); // Pass the deserialized board

            // Create a new GameViewModel to hold the game state
            GameViewModel viewModel = new GameViewModel(gameEngine);
            
            return viewModel;
        }

        public async Task SaveGame(string userId, GameViewModel gameViewModel)
        {
            SavedGame savedGame = new SavedGame();
  
            string json = JsonConvert.SerializeObject(gameViewModel.Game.GetBoardState(), Formatting.Indented);

            savedGame.UserId = Convert.ToInt32(userId);
            savedGame.DateSaved = DateTime.Now;
            savedGame.GameData = json;
            savedGame.TimePlayed = (int)gameViewModel.Game.GetElapsedTime().TotalSeconds;

            await _savedGamesDAO.AddSaveGame(savedGame);
            
        }
    }
}
