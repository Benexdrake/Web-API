namespace Web_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CrunchyrollController : ControllerBase
{
    private readonly CrunchyrollDBContext _context;
    public CrunchyrollController(IServiceProvider service)
    {
        _context = service.GetRequiredService<CrunchyrollDBContext>();
    }

    [HttpGet("animes")]
    public async Task<ActionResult> GetAllAnimes()
    {
        var animes = _context.Animes;
        if(animes is not null)
            return Ok(animes);
        return BadRequest("No Animes Found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAnimeById(string id)
    {
        var anime = _context.Animes.FindAsync(id).Result;
        if (anime is not null)
            return Ok(anime);
        return BadRequest("No Anime Found");
    }

    [HttpGet("animesbyname")]
    public async Task<ActionResult> GetAnimeByName(string name)
    {
        var animes = _context.Animes.Where(x => x.Name.Contains(name)).Take(100);
        if (animes is not null)
            return Ok(animes);
        return BadRequest("No Animes Found");
    }

    [HttpGet("animesbygenre")]
    public async Task<ActionResult> GetAnimesByGenre(string genre)
    {
        var animes = _context.Animes.Where(x => x.Tags.Contains(genre));
        if(animes is not null)
            return Ok(animes);
        return BadRequest("No Animes found");
    }

    [HttpGet("animebyepisodes")]
    public async Task<ActionResult> GetAnimeByEpisodes(int episodes)
    {
        var animes = _context.Animes.Where(x => x.Episodes <= episodes);
        if(animes is not null)
            return Ok(animes);
        return BadRequest("No Animes found");
    }

    [HttpGet("animebyrating")]
    public async Task<ActionResult> GetAnimeByRating(double rating)
    {
        var animes = _context.Animes.Where(x => x.Rating >= rating);
        if(animes is not null)
            return Ok(animes);
        return BadRequest("No Animes found");
    }

    [HttpGet("episodes/{animeId}")]
    public async Task<ActionResult> GetEpisodesByAnimeId(string animeId)
    {
        var episodes = _context.Episodes.Where(x => x.AnimeId.Equals(animeId));
        if(episodes is not null)
            return Ok(episodes);
        return BadRequest("No Episodes found");
    }

    // Anime Eintragen
    [HttpPost("createAnime/{anime.Id}")]
    public async Task<ActionResult> CreateAnime(Anime_Episodes AE)
    {
        var animeDb = _context.Animes.Where(x => x.Id.Equals(AE.Anime.Id));
        if(animeDb is null)
        {
            _context.Animes.Add(AE.Anime);
        }

        List<Episode> episodes = new();
        foreach (var episode in AE.Episodes)
        {
            var episodeDb = _context.Episodes.Where(x => x.Id.Equals(episode.Id));
            if(episodeDb is null)
                _context.Episodes.Add(episode);
        }
        _context.SaveChanges();
        return Ok(AE.Anime);
    }

    // Anime Updaten
    [HttpPut("updateAnime/{AE.Anime.Id}")]
    public async Task<ActionResult> UpdateAnime(Anime_Episodes AE)
    {
        var animeDb = _context.Animes.Where(x => x.Id.Equals(AE.Anime.Id)).FirstOrDefault();
        animeDb.Episodes = AE.Anime.Episodes;
        _context.SaveChanges();
        return Ok(AE.Anime);
    }
}
