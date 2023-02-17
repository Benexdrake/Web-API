
using Webscraper_API.Scraper.Insight_Digital_Handy.Models;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsightDigitalController : ControllerBase
    {
        private readonly InsightDigitalDbContext _context;
        public InsightDigitalController(IServiceProvider service)
        {
            _context = service.GetRequiredService<InsightDigitalDbContext>();
        }

        [HttpGet("id")]
        public async Task<ActionResult> GetPhone(string id = "samsung-galaxy-s22-ultra")
        {
                var filter = Builders<Handy>.Filter.Eq("Id", id);
                var handy  = _context.Handys.FindAsync(filter).Result.FirstOrDefault();
                return Ok(handy);
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetPhones()
        {
            var handy = _context.Handys.FindAsync(_ => true).Result.ToList();
            return Ok(handy);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrUpdate(Handy handy)
        {
            var filter = Builders<Handy>.Filter.Eq(x => x.Id, handy.Id);
            var handyDb = _context.Handys.FindAsync(filter).Result.FirstOrDefault();
            if (handyDb is null)
                await _context.Handys.InsertOneAsync(handy);
            else
                await _context.Handys.ReplaceOneAsync(filter, handy, new ReplaceOptions { IsUpsert = true });
            return Ok();
        }

    }
}
