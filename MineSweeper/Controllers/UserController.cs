using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using RegisterAndLoginAPP.Filters;
using RegisterAndLoginAPP.Models;

namespace MineSweeper.Controllers
{
    public class UserController : Controller
    {
        private IUserManager users;
        UserModel newUser;

        public IActionResult Index()
        {
            return View();
        }

        public UserController(IUserManager userManager) 
        {
            users = userManager;
        }

        public IActionResult ProccessLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return View("LoginFailure", new UserModel { Username = username });
            }

            int userId = users.CheckCredentials(username, password);

            if (userId != 0)
            {
                newUser = users.getUserByName(username);
                string userJson = ServiceStack.Text.JsonSerializer.SerializeToString(newUser);
                HttpContext.Session.SetString("User", userJson);
                return View("LoginSuccess", newUser);
            }
            else
            {
                return View("LoginFailure", new UserModel { Username = username });
            }
        }


        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        public IActionResult ProcessRegister(RegisterViewModel registerViewModel)
        {
            UserModel user = new UserModel
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Sex = registerViewModel.Sex,
                DateOfBirth = registerViewModel.DateOfBirth,
                Email = registerViewModel.Email,
                State = registerViewModel.State,
                Username = registerViewModel.Username
            };
            user.SetPassword(registerViewModel.Password);
            users.AddUser(user);

            return View("Index");
        }


        [SessionCheckFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return View("Index");
        }
    }
}
