using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SteamController : ControllerBase
    {
        private readonly SteamDbContext _context;
        public SteamController(IServiceProvider service)
        {
           _context = service.GetRequiredService<SteamDbContext>();
        }

        [HttpGet]
        public async Task<ActionResult> GetSteamGames()
        {
            var games = _context.Games.FindAsync(_ => true).Result.ToList();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetSteamGame(string id)
        {
            var filter = Builders<Game>.Filter.Eq(x => x.Id, id);
            var game = _context.Games.FindAsync(filter).Result.FirstOrDefault();
            if(game is not null)
                return Ok(game);
            return Ok(new Game());
        }

        [HttpPost]
        public async Task<ActionResult<Game>> CreateOrUpdate(Game game)
        {
            var filter = Builders<Game>.Filter.Eq(x => x.Id, game.Id);
            var g = _context.Games.FindAsync(filter).Result.FirstOrDefault();
            if (g is null)
                await _context.Games.InsertOneAsync(game);
            else
                await _context.Games.ReplaceOneAsync(filter, game, new ReplaceOptions { IsUpsert = true});
            
            return Ok(game);
        }

    }
}