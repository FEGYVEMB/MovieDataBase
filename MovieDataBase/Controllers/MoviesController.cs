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

        [HttpGet("GetAll")]
        public Task<List<Movie>> GetMovies()
        {
            _logger.LogDebug($"getmovies was called...");
            return _movieRepository.GetAllMovies();
        }

        [HttpGet("GetById")]
        public async Task<Movie> GetMovieById([FromHeader] string id)
        {
            _logger.LogDebug($"getmoviebyid was called for id: {id}...");

            if(await _movieRepository.MovieExists(id))
            {
                return await _movieRepository.GetMovieById(id);
            }
            else
            {
                return null;
            }
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
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            try
            {
                _logger.LogInformation($"remove request has been called to remove movie with the following ID: {id}");
                _movieRepository.RemoveMovie(id);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"remove request has run into an exception");

                return NoContent();
            }
        }
    }
}