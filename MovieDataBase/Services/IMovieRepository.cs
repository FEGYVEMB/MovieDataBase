
public interface IMovieRepository
{
    public Task<List<Movie>> GetAllMoviesAsync();
    public Task<Movie> GetMovieByIdAsync(string id);
    public void AddAsync(string title);
    public void RemoveMovieAsync(string id);
    public void UpdateMovieAsync(string id, string updatedTitle);
    public Task<bool> MovieExists(string id);
}