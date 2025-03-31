namespace MineSweeper.Models.DAOs.InterfacesDAOs
{
    public interface ISavedGamesDAO
    {
        public Task AddSaveGame(SavedGame savedGame);
        public Task<IEnumerable<SavedGame>> GetAllSavedGames();
        public Task<SavedGame?> GetSavedGameById(int id);
        public Task<int> DeleteSavedGame(int id);
        public Task UpdateSavedGame(SavedGame savedGame);
    }
}
