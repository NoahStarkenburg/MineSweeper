using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;

namespace MineSweeper.Models.DAOs
{
    public class SavedGamesDAO
    {
        private readonly string _connectionString;

        public SavedGamesDAO (IConfiguration configuration)
        {
            _connectionString = configuration["SQLConnection:DefaultConnection"];
        }

        public async Task AddSaveGame(SavedGame savedGame)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open ();
                string query = @"INSERT INTO savedgames (UserId, DateSaved, GameData) VALUES (@UserId, @DateSaved, @GameData)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue ("@UserId", savedGame.UserId);
                    command.Parameters.AddWithValue("@DateSaved", savedGame.DateSaved);
                    command.Parameters.AddWithValue("@GameData", savedGame.GameData);

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
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                DateSaved = reader.GetDateTime(2),
                                GameData = reader.GetString(3)
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
                using(MySqlCommand command = new MySqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            SavedGame savedGame = new SavedGame
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                DateSaved = reader.GetDateTime(2),
                                GameData = reader.GetString(3)
                            };
                            return savedGame;
                        }
                    }
                    
                }
            }
            return null;
            
        }


        public async Task DeleteSavedGame(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"DELETE * FROM savedgames WHERE ID = @Id";
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateSavedGame(SavedGame savedGame)
        {
            using(MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string query = @"UPDATE savedgames SET UserId = @UserId, DateSaved = @DateSaved, GameData = @GameData WHERE Id = @Id";
                await connection.OpenAsync();

                using(MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", savedGame.UserId);
                    command.Parameters.AddWithValue("@DateSaved", savedGame.DateSaved);
                    command.Parameters.AddWithValue("GameData", savedGame.GameData);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
