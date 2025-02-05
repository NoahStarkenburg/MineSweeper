namespace MineSweeper.Models
{
    public interface IUserManager
    {
        public List<UserModel> GetAllUsers();
        public UserModel getUserById(int id);
        public UserModel getUserByName(string username);
        public int AddUser(UserModel user);
        public void UpdateUser(UserModel user);
        public void DeleteUser(UserModel user);
        public int CheckCredentials(string username, string password);
    }
}
