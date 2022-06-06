using Microsoft.AspNetCore.Mvc;

namespace MovieDataBase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        // this is our in memory repo of movies (?)
        private readonly IMovieRepository _movieRepository;

        public MoviesController(ILogger<MoviesController> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _logger.LogInformation($"Movies ctor initialized: {GetHashCode()}");
        }

        [HttpGet("movies")]
        public List<Movie> GetMovies()
        {
            _logger.LogInformation($"getmovies was called...");
            return _movieRepository.GetAllMovies();
        }

        [HttpPost("addmovie")]
        public void Post([FromBody]string title)
        {
            _logger.LogInformation($"Post1 was called to add movie titled \"{title}\".");
            _movieRepository.Add(title);
        }

        [HttpDelete("removemovie")]
        public void Delete([FromBody]string title)
        {

        }
    }
}