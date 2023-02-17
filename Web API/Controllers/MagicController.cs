using Webscraper_API.Scraper.Insight_Digital_Handy.Models;
using Webscraper_API.Scraper.Steam.Models;
using Webscraper_API.Scraper.TCG_Magic.Model;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MagicController : ControllerBase
    {
        private readonly MagicDbContext _context;
        public MagicController(IServiceProvider service)
        {
            _context= service.GetRequiredService<MagicDbContext>();
        }

        [HttpGet]
        public async Task<ActionResult> GetCards()
        {
            var cards = _context.Cards.FindAsync(_=> true).Result.ToList();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCardsById(string id)
        {
            var filter = Builders<Card>.Filter.Eq("Id", id);
            var card = _context.Cards.FindAsync(filter).Result.FirstOrDefault();
            if(card is not null)
                return Ok(card);
            return BadRequest();
        }

        [HttpGet("Set/{id}")]
        public async Task<ActionResult> GetCardsBySet(string id)
        {
            var filter = Builders<Card>.Filter.Eq(x => x.Set_id, id);
            var cards = _context.Cards.FindAsync(filter).Result.ToList();
            return Ok(cards); 
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate(Card card)
        {
            var filter = Builders<Card>.Filter.Eq(x => x.Id, card.Id);
            var c = _context.Cards.FindAsync(filter).Result.FirstOrDefault();
            if(c is null)
                await _context.Cards.InsertOneAsync(card);
            else
            {
                await _context.Cards.ReplaceOneAsync(filter, card, new ReplaceOptions { IsUpsert = true });
            }
            return Ok();
        }
    }
}
