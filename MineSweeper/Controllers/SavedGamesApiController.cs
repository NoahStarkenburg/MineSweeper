using Microsoft.AspNetCore.Mvc;
using MineSweeper.Models;
using MineSweeper.Models.DAOs;

namespace MineSweeper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavedGamesApiController : ControllerBase
    {
        private readonly SavedGamesDAO _savedGamesDAO;

        public SavedGamesApiController(SavedGamesDAO savedGamesDAO)
        {
            _savedGamesDAO = savedGamesDAO;
        }

        // GET: localhost/api/showSavedGames
        [HttpGet("/api/showSavedGamesByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<SavedGame>>> ShowAllSavedGames(string userId)
        {
            var savedGames = await _savedGamesDAO.GetAllSavedGamesByUserId(userId);
            return Ok(savedGames);
        }

        // GET: localhost/api/showSavedGames/5
        [HttpGet("/api/showSavedGames/{id}")]
        public async Task<ActionResult<SavedGame>> ShowSavedGameById(int id)
        {
            var savedGame = await _savedGamesDAO.GetSavedGameById(id);
            if (savedGame == null)
            {
                return NotFound(new { message = $"Game with ID {id} not found." });
            }
            return Ok(savedGame);
        }

        // DELETE: localhost/api/deleteOneGame/5
        [HttpDelete("/api/deleteOneGame/{id}")]
        public async Task<IActionResult> DeleteSavedGame(int id)
        {
            await _savedGamesDAO.DeleteSavedGame(id);
            return NoContent(); // 204 Success
        }
    }
}
