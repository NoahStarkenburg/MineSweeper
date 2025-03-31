using MySql.Data.MySqlClient;

namespace MineSweeper.Models.DAOs
{
    public class SavedGamesDAO
    {
        private readonly string _connectionString;

        public SavedGamesDAO(IConfiguration configuration)
        {
            _connectionString = configuration["SQLConnection:DefaultConnection"];
        }

        public async Task AddSaveGame(SavedGame savedGame)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO savedgames (UserId, DateSaved, GameData, TimePlayed) 
                                 VALUES (@UserId, @DateSaved, @GameData, @TimePlayed)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", savedGame.UserId);
                    command.Parameters.AddWithValue("@DateSaved", savedGame.DateSaved);
                    command.Parameters.AddWithValue("@GameData", savedGame.GameData);
                    command.Parameters.AddWithValue("@TimePlayed", savedGame.TimePlayed);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<SavedGame>> GetAllSavedGames()
        {
            List<SavedGame> savedGames = new List<SavedGame>();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM savedgames";

                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            SavedGame savedGame = new SavedGame
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                UserId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                DateSaved = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                GameData = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                TimePlayed = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                            };
                            savedGames.Add(savedGame);
                        }
                    }
                }
            }
            return savedGames;
        }

        public async Task<SavedGame?> GetSavedGameById(int id)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM savedgames WHERE ID = @Id";
                await con.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            SavedGame savedGame = new SavedGame
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                UserId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                DateSaved = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                GameData = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                TimePlayed = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                            };
                            return savedGame;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<int> DeleteSavedGame(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"DELETE FROM savedgames WHERE ID = @Id";
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateSavedGame(SavedGame savedGame)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"UPDATE savedgames 
                                 SET UserId = @UserId, DateSaved = @DateSaved, GameData = @GameData, TimePlayed = @TimePlayed 
                                 WHERE Id = @Id";
                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", savedGame.UserId);
                    command.Parameters.AddWithValue("@DateSaved", savedGame.DateSaved);
                    command.Parameters.AddWithValue("@GameData", savedGame.GameData);
                    command.Parameters.AddWithValue("@TimePlayed", savedGame.TimePlayed);
                    command.Parameters.AddWithValue("@Id", savedGame.Id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
