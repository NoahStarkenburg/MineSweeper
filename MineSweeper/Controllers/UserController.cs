using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using RegisterAndLoginAPP.Filters;
using RegisterAndLoginAPP.Models;

namespace MineSweeper.Controllers
{
    public class UserController : Controller
    {
        UserDAO users = new UserDAO();
        UserModel newUser;

        public UserController(UserDAO userManager) 
        {
            users = userManager;
        }

        public IActionResult ProccessLogin(LoginViewModel loginViewModel)
        {
            UserModel userData = new UserModel();

            if (users.CheckCredentials(loginViewModel.Username, loginViewModel.Password) != 0)
            {
                newUser = users.getUserByName(loginViewModel.Username);

                // Save the user data in the session
                string userJson = ServiceStack.Text.JsonSerializer.SerializeToString(userData);
                HttpContext.Session.SetString("User", userJson);
                return View("LoginSuccess", newUser);
            }
            else
            {
                return View("LoginFailure", userData);
            }
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        public IActionResult ProcessRegister(RegisterViewModel registerViewModel)
        {
            UserModel user = new UserModel();
            user.FirstName = registerViewModel.FirstName;
            user.LastName = registerViewModel.LastName;
            user.Sex = registerViewModel.Sex;
            user.DateOfBirth = registerViewModel.DateOfBirth;
            user.State = registerViewModel.State;
            user.Username = registerViewModel.Username;
            user.SetPassword(registerViewModel.Password);
            users.AddUser(user);

            return View("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        [SessionCheckFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return View("Index");
        }
    }
}
