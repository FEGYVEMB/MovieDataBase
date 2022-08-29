
public interface IMovieRepository
{
    public Task<List<Movie>> GetAllMovies();
    public Task<Movie> GetMovieById(string id);
    public void Add(string title);
    public void RemoveMovie(string id);
    public void UpdateMovie(string id, string updatedTitle);
    public Task<bool> MovieExists(string id);
}