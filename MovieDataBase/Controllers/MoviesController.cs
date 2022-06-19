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

        [HttpPatch("updatemovie")]
        public List<Movie> Update(string title, string correctTitle)
        {
            _logger.LogInformation($"Update request has been called to update the movie title of movie: \"{title}\" to: \"{correctTitle}\".");
            Movie targetMovie = _movieRepository.GetAllMovies().Where(m => m.Name.Equals(title)).First();
            _movieRepository.UpdateMovie(targetMovie, correctTitle);

            return _movieRepository.GetAllMovies();
        }

        [HttpDelete("removemovie")]
        public void Delete([FromBody]string id)
        {
            _logger.LogInformation($"removemovie request has been called to remove movie with the following ID: {id}");
            Movie targetMovie = _movieRepository.GetAllMovies().Where(m => m.Equals(id)).FirstOrDefault();
            _movieRepository.RemoveMovie(targetMovie);
        }
    }
}