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
        var movies = _context.Movies.ToList();
        if(movies is not null)
            return Ok(movies);
        return BadRequest("No Movies found");
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult> GetMovieById(string Id)
    {
        var movie = _context.Movies.FindAsync(Id).Result;
        if(movie is not null)
            return Ok(movie);
        return BadRequest();
    }

    [HttpGet("movierandom")]
    public async Task<ActionResult> GetMovieByRandom()
    {
        var movies = _context.Movies.ToList();
        var movie = movies[rand.Next(0,movies.Count)];
        if(movie is not null)
            return Ok(movie);
        return BadRequest();
    }

    [HttpGet("moviebyname")]
    public async Task<ActionResult> GetMovieByName(string title)
    {
        var movies = _context.Movies.Where(x => x.Title.Contains(title)).ToList();
        if(movies.Count >0)
            return Ok(movies);
        return BadRequest();
    }

    [HttpGet("moviebygenre")]
    public async Task<ActionResult> GetMovieByGenre(string genre)
    {
        var movies = _context.Movies.Where(x => x.Genres.Contains(genre)).ToList();
        if (movies.Count > 0)
            return Ok(movies);
        return BadRequest();
    }

    [HttpGet("moviebyrating")]
    public async Task<ActionResult> GetMovieByRating(double rating)
    {
        var movies = _context.Movies.Where(x => x.Rating >= rating);
        return Ok(movies);
    }

    [HttpGet("moviebycast")]
    public async Task<ActionResult> GetMovieByCast(string name)
    {
        var movies = _context.Movies.Where(x => x.MainCast.Contains(name)).ToList();
        if(movies.Count > 0)
            return Ok(movies);
        return BadRequest();
    }

    // Film eintragen
    [HttpPost("createmovie/{movie.Id}")]
    public async Task<ActionResult> CreateMovie(Movie movie)
    {
        var movieDb = _context.Movies.Where(x => x.Id.Equals(movie.Id)).FirstOrDefault();
        if(movieDb is null)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }
        return Ok(movie);
    }

}
