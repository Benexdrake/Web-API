using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : ControllerBase
{
    private readonly PokemonDBContext _context;
    private readonly Random rand;
    public PokemonController(IServiceProvider service)
    {
        _context = service.GetRequiredService<PokemonDBContext>();
        rand = new Random();
    }

    [HttpGet("pokemons")]
    public async Task<ActionResult> GetAllPokemons()
    {
        var pokemons = _context.Pokemons;
        if(pokemons is not null) 
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("{nr}")]
    public async Task<ActionResult> GetPokemonByNr(int nr)
    {
        var filter = Builders<Pokemon>.Filter.Eq(x => x.Nr, nr);
        var pokemons = _context.Pokemons.FindAsync(filter).Result.ToList();
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPokemonByNr(string id)
    {
        var filter = Builders<Pokemon>.Filter.Eq(x => x.Id, id);
        var pokemons = _context.Pokemons.FindAsync(filter).Result.FirstOrDefault();
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonrandom")]
    public async Task<ActionResult> GetPokemonByRandom()
    {
        var pokemons = _context.Pokemons.FindAsync(_ => true).Result.ToList();
        var pokemon = pokemons[rand.Next(0, pokemons.Count)];
        if(pokemon is not null)
            return Ok(pokemon);
        return BadRequest();

    }

    [HttpGet("pokemonbyname")]
    public async Task<ActionResult> GetPokemonByName(string name)
    {
        var filter = Builders<Pokemon>.Filter.Eq(x => x.Name, name);
        var pokemons = _context.Pokemons.FindAsync(filter).Result.FirstOrDefault();
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonbytype")]
    public async Task<ActionResult> GetPokemonByType(string type)
    {
        var filter = Builders<Pokemon>.Filter.Eq(x => x.Type, type);
        var pokemons = _context.Pokemons.FindAsync(filter).Result.ToList();
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonbyweakness")]
    public async Task<ActionResult> GetPokemonByWeakness(string weakness)
    {
        var filter = Builders<Pokemon>.Filter.Eq(x => x.Weakness, weakness);
        var pokemons = _context.Pokemons.FindAsync(filter).Result.ToList();
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonbycategory")]
    public async Task<ActionResult> GetPokemonByCategory(string category)
    {
        var filter = Builders<Pokemon>.Filter.Eq(x => x.Category, category);
        var pokemons = _context.Pokemons.FindAsync(filter).Result.ToList();
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    // pokemon eintragen oder updaten
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdate(Pokemon[] pokemons)
    {
        for (int i = 0; i < pokemons.Length; i++)
        {
            var filter = Builders<Pokemon>.Filter.Eq(x => x.Id, pokemons[i].Id);
            var pokemonDb = _context.Pokemons.FindAsync(filter).Result.FirstOrDefault();
            if(pokemonDb is null)
                await _context.Pokemons.InsertOneAsync(pokemons[i]);
            else
                await _context.Pokemons.ReplaceOneAsync(filter, pokemons[i], new ReplaceOptions { IsUpsert = true });
        }
        return Ok(pokemons);
    }
}
