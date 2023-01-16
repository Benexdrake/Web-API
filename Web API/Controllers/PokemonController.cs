namespace Web_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : ControllerBase
{
    private readonly PokemonDBContext _context;
    public PokemonController(IServiceProvider service)
    {
        _context = service.GetRequiredService<PokemonDBContext>();
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
    {
        var pokemons = _context.Pokemons.Where(x => x.Nr == nr);
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonbyname")]
    public async Task<ActionResult> GetPokemonByName(string name)
    {
        var pokemons = _context.Pokemons.Where(x => x.Name.Contains(name));
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonbytype")]
    public async Task<ActionResult> GetPokemonByType(string type)
    {
        var pokemons = _context.Pokemons.Where(x => x.Type.Contains(type));
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonbyweakness")]
    public async Task<ActionResult> GetPokemonByWeakness(string weakness)
    {
        var pokemons = _context.Pokemons.Where(x => x.Weakness.Contains(weakness));
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }

    [HttpGet("pokemonbycategory")]
    public async Task<ActionResult> GetPokemonByCategory(string category)
    {
        var pokemons = _context.Pokemons.Where(x => x.Category.Contains(category));
        if (pokemons is not null)
            return Ok(pokemons);
        return BadRequest();
    }


    // pokemon eintragen
    [HttpPost("createpokemon/{nr}")]
    public async Task<ActionResult> CreatePokemon(Pokemon[] pokemons, int nr)
    {
        var pokemonDb = _context.Pokemons.Where(x => x.Nr == nr).FirstOrDefault();
        if(pokemonDb is null)
        {
            foreach (var pokemon in pokemons)
            {
                _context.Add(pokemons);
            }
            _context.SaveChanges();
        }
        return Ok(pokemons);
    }
}
