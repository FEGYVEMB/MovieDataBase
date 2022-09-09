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
            return _movieRepository.GetAllMoviesAsync();
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetMovieById([FromHeader] string id)
        {
            // _logger.LogDebug($"getmoviebyid was called for id: {id}...");
            // var result = await _movieRepository.GetMovieByIdAsync(id);

            // if(result.Title == null)
            // {
            //     return NotFound();
            // }

            // return Ok(result);

            try
            {
                _logger.LogDebug($"getmoviebyid was called for id: {id}...");
                return Ok(await _movieRepository.GetMovieByIdAsync(id));
            }
            catch(Exception e)
            {
                _logger.LogDebug($"{e.GetType()}: Entry with id: {id} was not found.");
                return NotFound();
            }
        }

        [HttpPost("Add")]
        public void Post([FromBody] string title)
        {
            _logger.LogDebug($"Post request was called to add movie titled \"{title}\".");
            _movieRepository.AddAsync(title);
        }

        [HttpPatch("Update")]
        public async Task<List<Movie>> Update(string id, string correctTitle)
        {
            _logger.LogDebug($"Update request has been called to update the movie title of movie: \"{id}\" to: \"{correctTitle}\".");
            _movieRepository.UpdateMovieAsync(id, correctTitle);

            return await _movieRepository.GetAllMoviesAsync();
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {
            try
            {
                _logger.LogDebug($"remove request has been called to remove movie with the following ID: {id}");
                await Task.Run(() =>_movieRepository.RemoveMovieAsync(id));

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogDebug($"remove request has run into an exception: {e}");
                return NoContent();
            }
        }
    }
}