using MySql.Data.MySqlClient;

namespace MineSweeper.Models.Game_Models
{
    public class GameDAO : IGameManager
    {
        string connectionString = "server=localhost;port=3306;user=root;password=root;database=minesweeper;";
        //string connectionString = "server=localhost;port=8889;user=root;password=root;database=minesweeper;";

        public int AddScore(ScoresModel score)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO `scores`(`NumOfClicks`, `Time`, `Score`, `UserId`) VALUES (@NumOfClicks,@TimeElapsed, @Score, @UserId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", score.NumOfClicks);
                    command.Parameters.AddWithValue("@LastName", score.TimeElapsed);
                    command.Parameters.AddWithValue("@Sex", score.Score);
                    command.Parameters.AddWithValue("@DateOfBirth", score.UserId);

                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public void DeleteScore(ScoresModel score)
        {
            throw new NotImplementedException();
        }

        public List<ScoresModel> GetAllSores()
        {
            throw new NotImplementedException();
        }

        public List<ScoresModel> GetScoreById()
        {
            throw new NotImplementedException();
        }

        public List<ScoresModel> GetScoreByUser()
        {
            throw new NotImplementedException();
        }

        public void UpdateScore(ScoresModel score)
        {
            throw new NotImplementedException();
        }
    }
}
