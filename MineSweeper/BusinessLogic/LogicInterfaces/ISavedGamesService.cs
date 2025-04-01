using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using MineSweeper.BusinessLogic.Game_Logic;

namespace MineSweeper.BusinessLogic.LogicInterfaces
{
    public interface ISavedGamesService
    {
        public Task SaveGame(string userId, GameViewModel gameViewModel);
        public Task<GameViewModel?> LoadGame(string userId, int savedGameId);
        public Task<IEnumerable<SavedGame>> GetAllGamesById(string userId);
        public Task DeleteSavedGame(int id);
    }
}
