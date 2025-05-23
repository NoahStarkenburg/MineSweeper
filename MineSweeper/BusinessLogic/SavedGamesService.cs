﻿using MineSweeper.BusinessLogic.Game_Logic;
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
            gameEngine.SavedGameId = savedGameId; // Set the SavedGameId

            // Create a new GameViewModel to hold the game state
            GameViewModel viewModel = new GameViewModel(gameEngine);
            
            return viewModel;
        }

        public async Task SaveGame(string userId, GameViewModel gameViewModel)
        {
            // Get all saved games for the user
            var existingGames = await _savedGamesDAO.GetAllSavedGamesByUserId(userId);
            
            // Check if a game with the same board state already exists
            string currentGameJson = JsonConvert.SerializeObject(gameViewModel.Game.GetBoardState(), Formatting.Indented);
            foreach (var existingGame in existingGames)
            {
                if (existingGame.GameData == currentGameJson)
                {
                    // If a duplicate is found, update the existing game instead of creating a new one
                    existingGame.DateSaved = DateTime.Now;
                    existingGame.TimePlayed = (int)gameViewModel.Game.GetElapsedTime().TotalSeconds;
                    await _savedGamesDAO.UpdateSavedGame(existingGame);
                    return;
                }
            }

            // If no duplicate found, create a new saved game
            SavedGame savedGame = new SavedGame();
            savedGame.UserId = Convert.ToInt32(userId);
            savedGame.DateSaved = DateTime.Now;
            savedGame.GameData = currentGameJson;
            savedGame.TimePlayed = (int)gameViewModel.Game.GetElapsedTime().TotalSeconds;

            await _savedGamesDAO.AddSaveGame(savedGame);
        }

        public async Task<IEnumerable<SavedGame>> GetAllGamesById(string userId)
        {
            return await _savedGamesDAO.GetAllSavedGamesByUserId(userId);
        }

        public async Task DeleteSavedGame(int id)
        {
            await _savedGamesDAO.DeleteSavedGame(id);
        }
    }
}
