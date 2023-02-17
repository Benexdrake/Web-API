using MongoDB.Driver;
using Web_API.Data;
using Webscraper_API.Scraper.Crunchyroll.Models;
using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CrunchyrollController : ControllerBase
{
    private readonly CrunchyrollDBContext _context;
    private readonly Random rand;
    public CrunchyrollController(IServiceProvider service)
    {
        _context = service.GetRequiredService<CrunchyrollDBContext>();
        rand = new Random();
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAnimes()
    {
        var animes = _context.Animes.FindAsync(_ => true).Result.ToList();
        if(animes is not null)
            return Ok(animes);
        return BadRequest("No Animes Found");
    }

    [HttpGet("AE")]
    public async Task<ActionResult> GetAnimeWithEpisodes(string id)
    {
        var filter = Builders<Anime>.Filter.Eq(x => x.Id, id);
        var anime = _context.Animes.FindAsync(filter).Result.FirstOrDefault();
        var episodes = _context.Episodes.FindAsync(_ => true).Result.ToList().Where(x => x.AnimeId.Equals(anime.Id)).ToList();
        Anime_Episodes AE = new(anime,episodes.ToArray());
        return Ok(AE);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAnimeById(string id)
    {
        var filter = Builders<Anime>.Filter.Eq(x => x.Id, id);
        var anime = _context.Animes.FindAsync(filter).Result.FirstOrDefault();
        if (anime is not null)
            return Ok(anime);
        return Ok(new Anime());
    }

    [HttpGet("animerandom")]
    public async Task<ActionResult> GetAnimeByRandom()
    {
        var animes = _context.Animes.FindAsync(_ => true).Result.ToList();
        var anime = animes[rand.Next(0, animes.Count)];
        if(anime is not null)
            return Ok(anime);
        return BadRequest();
    }

    [HttpGet("animesbyname")]
    public async Task<ActionResult> GetAnimesByName(string name)
    {
        var filter = Builders<Anime>.Filter.Eq(x => x.Name, name);
        var animes = _context.Animes.FindAsync(filter).Result.ToList();
        if (animes is not null)
            return Ok(animes);
        return BadRequest("No Animes Found");
    }

    [HttpGet("animesbygenre")]
    public async Task<ActionResult> GetAnimesByGenre(string tags)
    {
        var animes = _context.Animes.FindAsync(_ => true).Result.ToList().Where(x => x.Tags.Equals(tags));
        if (animes is not null)
            return Ok(animes);
        return BadRequest("No Animes found");
    }

    [HttpGet("animesbyepisodes")]
    public async Task<ActionResult> GetAnimeByEpisodes(int episodes)
    {
        var animes = _context.Animes.FindAsync(_ => true).Result.ToList().Where(x => x.Episodes <= episodes);
        if(animes is not null)
            return Ok(animes);
        return BadRequest("No Animes found");
    }

    [HttpGet("animesbyrating")]
    public async Task<ActionResult> GetAnimeByRating(double rating)
    {
        var animes = _context.Animes.FindAsync(_ => true).Result.ToList().Where(x => x.Rating >= rating);
        if(animes is not null)
            return Ok(animes);
        return BadRequest("No Animes found");
    }

    [HttpGet("episodes")]
    public async Task<ActionResult> GetEpisodesByAnimeId(string animeId)
    {
        var episodes = _context.Episodes.FindAsync(_ => true).Result.ToList().Where(x => x.AnimeId.Equals(animeId));
        if(episodes is not null)
            return Ok(episodes);
        return BadRequest("No Episodes found");
    }

    // Anime Eintragen oder Updaten
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdate(Anime_Episodes AE)
    {
        var anime = _context.Animes.FindAsync(_ => true).Result.ToList().Where(x => x.Id.Equals(AE.Anime.Id)).FirstOrDefault();
        var episodes = _context.Episodes.FindAsync(_ => true).Result.ToList().Where(x => x.AnimeId.Equals(AE.Anime.Id));
        if (anime is not null)
            await UpdateAnime(AE.Anime);
        else
            await CreateAnime(AE.Anime);

        foreach (var episode in AE.Episodes)
        {
            var e = episodes.Where(x => x.Id.Equals(episode.Id));
            if (e is not null)
                await UpdateEpisode(episode);
            else
                await CreateEpisode(episode);
        }
        return Ok(AE);
    }

    [HttpGet("all")]
    public async Task<ActionResult> GetAll()
    {
        var animes = _context.Animes.FindAsync(_ => true).Result.ToList();
        List<Anime_Episodes> AEs = new();
        foreach (var a in animes)
        {
            var filter = Builders<Anime>.Filter.Eq(x => x.Id, a.Id);
            var anime = _context.Animes.FindAsync(filter).Result.FirstOrDefault();
            var episodes = _context.Episodes.FindAsync(_ => true).Result.ToList().Where(x => x.AnimeId.Equals(anime.Id)).ToList();
            Anime_Episodes AE = new(anime, episodes.ToArray());
            AEs.Add(AE);
        }
        return Ok(AEs);
    }


    private async Task CreateAnime(Anime anime)
    {
        await _context.Animes.InsertOneAsync(anime);
    }
    private async Task UpdateAnime(Anime anime)
    {
        var filter = Builders<Anime>.Filter.Eq("Id", anime.Id);
        _context.Animes.ReplaceOneAsync(filter, anime, new ReplaceOptions { IsUpsert = true });
    }
    private async Task CreateEpisode(Episode episode)
    {
        await _context.Episodes.InsertOneAsync(episode);
    }
    private async Task UpdateEpisode(Episode episode)
    {
        var filter = Builders<Episode>.Filter.Eq("Id", episode.Id);
        _context.Episodes.ReplaceOneAsync(filter, episode, new ReplaceOptions { IsUpsert = true });
    }
}
