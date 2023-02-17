using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonCardController : ControllerBase
    {
        private readonly PokemonDBContext _context;
        public PokemonCardController(IServiceProvider service)
        {
            _context = service.GetRequiredService<PokemonDBContext>();
        }

        [HttpGet]
        public async Task<ActionResult> GetCards()
        {
            var cards = _context.PokemonCards.FindAsync(_ => true).Result.ToList();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCard(string id)
        {
            var filter = Builders<PokemonCard>.Filter.Eq(x => x.Id, id);
            var card = _context.PokemonCards.FindAsync(filter).Result.FirstOrDefault();
            if(card is not null)
            {
                return Ok(card);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate(PokemonCard card)
        {
            var filter = Builders<PokemonCard>.Filter.Eq(x => x.Id, card.Id);
            var cardDb = _context.PokemonCards.FindAsync(filter).Result.FirstOrDefault();
            if(cardDb is null)
                await _context.PokemonCards.InsertOneAsync(card);
            else
                await _context.PokemonCards.ReplaceOneAsync(filter, card, new ReplaceOptions { IsUpsert = true });
            return Ok(card);
        }
    }
}
