using MySql.Data.MySqlClient;
using System.Data;

namespace MineSweeper.Models
{
    public class UserDAO : IUserManager
    {
        string connectionString = "server=localhost;port=3306;user=root;password=root;database=minesweeper;";

        public int AddUser(UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO `users`(`FirstName`, `LastName`, `Sex`, `DateOfBirth`, `State`, `Email`, `Username`, `Password`, `Salt`) 
                 VALUES (@FirstName, @LastName, @Sex, @DateOfBirth, @State, @Email, @Username, @PasswordHash, @Salt)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Sex", user.Sex);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth.Date);
                    command.Parameters.AddWithValue("@State", user.State);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);

                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public int CheckCredentials(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE Username = @Username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("Password")),
                        Salt = reader.GetString(reader.GetOrdinal("Salt"))
                    };

                    // Verify the password
                    if (user.VerifyPassword(password))
                        return user.Id;
                }
            }
            return 0;
        }


        public void DeleteUser(UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM users where Id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.ExecuteNonQuery();
            }
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Sex = reader.GetString(reader.GetOrdinal("Sex")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = reader.GetString(reader.GetOrdinal("Salt"))
                    };
                    users.Add(user);
                }
            }
            return users;
        }

        public UserModel getUserById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Sex = reader.GetString(reader.GetOrdinal("Sex")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = reader.GetString(reader.GetOrdinal("Salt"))
                    };
                    return user;
                }
            }
            return null;
        }

        public UserModel getUserByName(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM users WHERE Username = @Username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        Sex = reader.GetString(reader.GetOrdinal("Sex")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("Password")),
                        Salt = reader.GetString(reader.GetOrdinal("Salt"))
                    };
                    return user;
                }
            }
            return null;
        }

        public void UpdateUser(UserModel user)
        {
            if (user != null)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    UPDATE users 
                    SET FirstName = @FirstName, Lastname = @LastName, Sex = @Sex, DateOfBirth = @DateOfBirth, State = @State, Email = @Email, Username = @Username, Password = @PasswordHash, Salt = @Salt
                    WHERE Id = @Id";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Sex", user.Sex);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth.Date);
                    command.Parameters.AddWithValue("@State", user.State);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
