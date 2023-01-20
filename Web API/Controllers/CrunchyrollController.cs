using System.Diagnostics;

namespace Web_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CrunchyrollController : ControllerBase
{
    private readonly CrunchyrollDBContext _context;
    private readonly ICR_API _api;
    private readonly Browser _browser;
    private readonly Random rand;
    public CrunchyrollController(IServiceProvider service)
    {
        _context = service.GetRequiredService<CrunchyrollDBContext>();
        _api = service.GetRequiredService<ICR_API>();
        _browser = service.GetRequiredService<Browser>();

        rand = new Random();
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

    [HttpGet("animerandom")]
    public async Task<ActionResult> GetAnimeByRandom()
    {
        var animes = _context.Animes.ToList();
        var anime = animes[rand.Next(0, animes.Count)];
        if(anime is not null)
            return Ok(anime);
        return BadRequest();
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

    // Crunchyroll Scrape
    [HttpGet("crunchyrollscraper/geturls")]
    public async Task<ActionResult> GetCrunchyrollUrls()
    {
        var urls = _api.GetAllAnimeUrlsAsync();

        while(!urls.IsCompleted)
        {
            Console.WriteLine(_api.Message);
            await Task.Delay(1000);
        }
        
        return Ok(urls.Result);
    }

    [HttpGet("crunchyrollscraper/getanime")]
    public async Task<ActionResult> GetAnime(string url)
    {
        var AE = _api.GetAnimewithEpisodes(url,2000);
        if (AE is not null)
            return Ok(AE);
        else
            return BadRequest();
    }

    [HttpGet("crunchyrollscraper/fullupdate")]
    public async Task<ActionResult> GetFullUpdate(bool debug)
    {
        if(debug)
            _browser.WebDriver = _browser.FirefoxDebug();

        var urls = _api.GetAllAnimeUrlsAsync();

        while (!urls.IsCompleted)
        {
            Console.WriteLine(_api.Message);
            await Task.Delay(1000);
        }

        List<Anime_Episodes> AEs = new();

        foreach (var url in urls.Result)
        {
            var AE = _api.GetAnimewithEpisodes(url, 2000).Result;
            if(AE is not null)
            {
                AEs.Add(AE);
            }
        }
        return Ok(AEs.ToArray());
    }

    [HttpGet("test")]
    public async Task<ActionResult> Test()
    {
        var t1 = Task.Run(async () =>
        {
            _browser.WebDriver = _browser.FirefoxDebug();
            var sw = Stopwatch.StartNew();
            var ae = await _api.GetAnimewithEpisodes("https://www.crunchyroll.com/de/series/GRMG8ZQZR/one-piece", 2000);
            Console.WriteLine("Task 2" + sw.Elapsed);
            sw.Stop();
            Console.WriteLine(ae.Episodes.Length);
        });

        await Task.WhenAll(t1);


        return Ok();
    }
}
