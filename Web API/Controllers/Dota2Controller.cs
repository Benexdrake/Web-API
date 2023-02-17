using MongoDB.Driver;
using Webscraper_API.Scraper.Dota2.Models;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Dota2Controller : ControllerBase
    {
        private readonly Dota2DbContext _context;

        public Dota2Controller(IServiceProvider service)
        {
            _context = service.GetRequiredService<Dota2DbContext>();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllHeroes()
        {
            var heroes = _context.Heroes.FindAsync(_ => true).Result.ToList();
            return Ok(heroes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetHeroById(int id)
        {
            var hero = _context.Heroes.FindAsync(_ => true).Result.ToList().Where(x => x.id.Equals(id)).FirstOrDefault();
            if (hero is not null)
                return Ok(hero);
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateHero(Hero hero)
        {
            var filter = Builders<Hero>.Filter.Eq(x => x.id, hero.id);
            var heroDb = _context.Heroes.FindAsync(filter).Result.FirstOrDefault();
            if (heroDb is not null)
                await _context.Heroes.InsertOneAsync(hero);
            else
                await _context.Heroes.ReplaceOneAsync(filter, hero, new ReplaceOptions { IsUpsert = true });
            return Ok();

        }
    }
}
