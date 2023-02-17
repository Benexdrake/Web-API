using Webscraper_API.Scraper.Steam.Models;

namespace Web_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IMDbController : ControllerBase
{
    private readonly IMDbDBContext _context;
    private readonly Random rand;
    public IMDbController(IServiceProvider service)
    {
        _context = service.GetRequiredService<IMDbDBContext>();
        rand = new Random();
    }

    [HttpGet("movies")]
    public async Task<ActionResult> GetAllMovies()
    {
        var movies = _context.Movies.FindAsync(_ => true).Result.ToList();
        if(movies is not null)
            return Ok(movies);
        return BadRequest("No Movies found");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetMovieById(string id)
    {
        var filter = Builders<Movie>.Filter.Eq(x => x.Id, id);
        var movie = _context.Movies.FindAsync(filter).Result.FirstOrDefault();
        if(movie is not null)
            return Ok(movie);
        return BadRequest();
    }

    [HttpGet("movierandom")]
    public async Task<ActionResult> GetMovieByRandom()
    {
        var movies = _context.Movies.FindAsync(_ => true).Result.ToList();
        var movie = movies[rand.Next(0,movies.Count)];
        if(movie is not null)
            return Ok(movie);
        return BadRequest();
    }

    [HttpGet("moviebyname")]
    public async Task<ActionResult> GetMovieByName(string title)
    {
        var filter = Builders<Movie>.Filter.Eq(x => x.Title, title);
        var movies = _context.Movies.FindAsync(filter).Result.ToList();
        if(movies.Count >0)
            return Ok(movies);
        return BadRequest();
    }

    [HttpGet("moviebygenre")]
    public async Task<ActionResult> GetMovieByGenre(string genre)
    {
        var filter = Builders<Movie>.Filter.Eq(x => x.Genres, genre);
        var movies = _context.Movies.FindAsync(filter).Result.ToList();
        if (movies.Count > 0)
            return Ok(movies);
        return BadRequest();
    }

    [HttpGet("moviebyrating")]
    public async Task<ActionResult> GetMovieByRating(double rating)
    {
        var filter = Builders<Movie>.Filter.AnyGte("Rating", rating);
        var movies = _context.Movies.FindAsync(filter).Result.ToList();
        return Ok(movies);
    }

    [HttpGet("moviebycast")]
    public async Task<ActionResult> GetMovieByCast(string name)
    {
        var filter = Builders<Movie>.Filter.Eq(x => x.MainCast, name);
        var movies = _context.Movies.FindAsync(filter).Result.ToList();
        if(movies.Count > 0)
            return Ok(movies);
        return BadRequest();
    }

    // Film eintragen
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdate(Movie movie)
    {
        var filter = Builders<Movie>.Filter.Eq(x => x.Id, movie.Id);
        var movieDb = _context.Movies.FindAsync(filter).Result.ToList();
        if (movieDb is null)
            await _context.Movies.InsertOneAsync(movie);
        else
            await _context.Movies.ReplaceOneAsync(filter, movie, new ReplaceOptions { IsUpsert = true });
        return Ok(movie);
    }
}
