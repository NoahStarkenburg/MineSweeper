using Microsoft.AspNetCore.Mvc;
using MineSweeper.BusinessLogic.Game_Logic;
using MineSweeper.Models;
using MineSweeper.Models.Game_Models;
using RegisterAndLoginAPP.Filters;
using ServiceStack.Text;

namespace MineSweeper.Controllers
{
    public class GameController : Controller
    {
        // Dictionary that holds all games accross the site
        private static Dictionary<string, GameViewModel> CurrentGames = new Dictionary<string, GameViewModel>();

        /// <summary>
        /// Starts game with the input from rows, cols, and the difficulty
        /// </summary>
        /// <param name="rows">3-10</param>
        /// <param name="columns">3-10</param>
        /// <param name="difficulty">1-3</param>
        /// <returns></returns>
        [HttpPost]
        [SessionCheckFilter]
        public IActionResult StartGame(int rows, int columns, int difficulty)
        {
            // Get user id
            string userId = GetUserById();
            if (!CurrentGames.TryGetValue(userId, out GameViewModel viewModel))
            {
                // Create new GameEngine and assign that to the view model
                GameEngine gameEngine = new GameEngine(rows, columns, difficulty);
                viewModel = new GameViewModel(gameEngine);

                // Add gameViewModel while assigning it to the user
                CurrentGames[userId] = viewModel;
            }

            // Return view of game
            return View("MinesweeperGame", viewModel);
        }

        /// <summary>
        /// Brings user to the page of the sliders to input choice of difficulty, rows, and columns
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SessionCheckFilter]
        public IActionResult StartGame()
        {

            return View();
        }

        /// <summary>
        /// Main method that will react to every click the user has made. Checks if game is also still running and if not will return back to the beginning
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PlayMove(int row, int col)
        {
            string userId = GetUserById();

            if (CurrentGames.TryGetValue(userId, out GameViewModel viewModel))
            {
                bool continueGame = viewModel.Game.UpdateGame(row, col, 1); // "1" represents clicking action

                if (viewModel.Game.IsGameWin())
                {
                    int finalScore = viewModel.Game.GenerateScore();
                    CurrentGames.Remove(userId); // End the game
                    return RedirectToAction("WinPage", new { score = finalScore });
                }

                if (continueGame)
                {
                    // 🔹 Keep existing GameEngine instance
                    CurrentGames[userId] = viewModel;
                    return View("MinesweeperGame", viewModel);
                }

                if (!continueGame && !viewModel.Game.IsGameWin())
                {
                    CurrentGames.Remove(userId);
                    return View("LosePage");
                }
            }

            return RedirectToAction("StartGame");
        }

        [HttpPost]
        public IActionResult PlayMovePartial(int row, int col)
        {
            string userId = GetUserById();
            if (CurrentGames.TryGetValue(userId, out GameViewModel viewModel))
            {
                bool continueGame = viewModel.Game.UpdateGame(row, col, 1); // 1 = left-click action

                if (viewModel.Game.IsGameWin())
                {
                    int finalScore = viewModel.Game.GenerateScore();
                    CurrentGames.Remove(userId);
                    return RedirectToAction("WinPage", new { score = finalScore });
                }
                if (!continueGame && !viewModel.Game.IsGameWin())
                {
                    // When a bomb is clicked, the game ends (loss)
                    CurrentGames.Remove(userId);
                    return View("LosePage");
                }

                // Update the board state in the current game
                CurrentGames[userId] = viewModel;
                // Return the entire board partial view to capture flood fill changes
                return PartialView("_GameBoard", viewModel);
            }
            return BadRequest();
        }





        [HttpPost]
        public IActionResult PartialCellUpdate(int row, int col)
        {
            // Get user id
            string userId = GetUserById();

            // Gets value (which is a bool) if the game is still running
            if (CurrentGames.TryGetValue(userId, out GameViewModel viewModel))
            {
                // Update the game state
                bool continueGame = viewModel.Game.UpdateGame(row, col, 1); // "1" represents clicking action

                if (viewModel.Game.IsGameWin())
                {
                    int finalScore = viewModel.Game.GenerateScore();
                    CurrentGames.Remove(userId); // End the game
                    return RedirectToAction("WinPage", new { score = finalScore });
                }

                if (continueGame)
                {
                    CurrentGames[userId] = viewModel; // Update dictionary
                    return View("MinesweeperGame", viewModel);
                }

                if (!continueGame && !viewModel.Game.IsGameWin())
                {
                    CurrentGames.Remove(userId);
                    return View("LosePage");
                }
            }

            // Returns back to start page if game is invalid
            return PartialView("_Cell");
        }



        public IActionResult WinPage(int score)
        {
            ViewBag.Score = score; // Store the score in ViewBag
            return View();
        }


        // Should be moved to DAO service class
        private string GetUserById()
        {
            string userJson = HttpContext.Session.GetString("User");
            UserModel user = JsonSerializer.DeserializeFromString<UserModel>(userJson);
            return user.Id.ToString();
        }

        [HttpPost]
        public IActionResult FlagCell(int Row, int Col)
        {
            string userId = GetUserById();

            if (CurrentGames.TryGetValue(userId, out GameViewModel viewModel))
            {
                viewModel.Game.UpdateGame(Row, Col, 2); // 2 = flag action

                if (viewModel.Game.AllBombsFlagged()) // Check if game is won
                {
                    int finalScore = viewModel.Game.GenerateScore();
                    CurrentGames.Remove(userId); // End game
                    return Json(new { gameWon = true, redirectUrl = Url.Action("WinPage", new { score = finalScore }) });
                }

                ViewData["Row"] = Row;
                ViewData["Col"] = Col;

                return PartialView("_Cell", viewModel); //  Only update flagged cell
            }

            return BadRequest();
        }




        [HttpGet]
        public IActionResult GetElapsedTime()
        {
            string userId = GetUserById();
            if (CurrentGames.TryGetValue(userId, out GameViewModel viewModel))
            {
                int elapsedSeconds = (int)viewModel.Game.GetElapsedTime().TotalSeconds;
                return Json(elapsedSeconds);
            }

            return Json(0);
        }


    }
}
