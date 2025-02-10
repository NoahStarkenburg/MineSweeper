using Microsoft.AspNetCore.Mvc;
using RegisterAndLoginAPP.Filters;

namespace MineSweeper.Controllers
{
    public class GameController : Controller
    {
        [SessionCheckFilter]
        public IActionResult StartGame()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
