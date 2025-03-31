namespace MineSweeper.Models.DAOs.InterfacesDAOs
{
    public interface ISavedGamesDAO
    {
        public Task AddSaveGame(SavedGame savedGame);
        public Task<IEnumerable<SavedGame>> GetAllSavedGamesByUserId(string userId);
        public Task<SavedGame?> GetSavedGameById(int id);
        public Task<int> DeleteSavedGame(int id);
        public Task UpdateSavedGame(SavedGame savedGame);
        public Task<IEnumerable<SavedGame>> GetAllSavedGames();
    }
}
