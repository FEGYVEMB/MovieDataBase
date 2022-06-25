using Microsoft.AspNetCore.Mvc;

namespace MovieDataBase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieRepository _movieRepository;

        public MoviesController(ILogger<MoviesController> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _logger.LogDebug($"Movies ctor initialized: {GetHashCode()}");
        }

        [HttpGet("Get")]
        public Task<List<Movie>> GetMovies()
        {
            _logger.LogDebug($"getmovies was called...");

            return _movieRepository.GetAllMovies();
        }

        [HttpPost("Add")]
        public void Post([FromBody] string title)
        {
            _logger.LogInformation($"Post1 was called to add movie titled \"{title}\".");
            _movieRepository.Add(title);
        }

        [HttpPatch("Update")]
        public async Task<List<Movie>> Update(string id, string correctTitle)
        {
            _logger.LogInformation($"Update request has been called to update the movie title of movie: \"{id}\" to: \"{correctTitle}\".");
            _movieRepository.UpdateMovie(id, correctTitle);

            return await _movieRepository.GetAllMovies();
        }

        [HttpDelete("Remove")]
        public IActionResult Delete([FromBody] string id)
        {
            try
            {
                _logger.LogInformation($"removemovie request has been called to remove movie with the following ID: {id}");
                _movieRepository.RemoveMovie(id);

                return Ok();
            }
            catch (Exception e)
            {
                return Ok();
            }
        }
    }
}