public class InMemoryMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = new List<Movie>();

    public InMemoryMovieRepository()
    {
        _movies.Add(new Movie("Kung Fury"));
        _movies.Add(new Movie("Last Action Hero"));
    }

    public void Add(string title)
    {
        Movie movie = new Movie(title);
        _movies.Add(movie);
    }
    
    public Task<Movie> GetMovieById(string id)
    {
        return null;
    }

    public async Task<List<Movie>> GetAllMovies()
    {
        return await Task.FromResult(_movies);
    }

    public async void RemoveMovie(string id)
    {
        var deleted = await Task.Run(() => _movies.RemoveAll(m => m.Id == id));
        if (deleted == 0) throw new KeyNotFoundException();
    }

    public async void UpdateMovie(string id, string updatedTitle)
    {
        var updateTask = await Task.FromResult(_movies.Where(m => m.Id == id).First().Title = updatedTitle);
    }

    public async Task<bool> MovieExists(string id) 
    {
        return await Task.FromResult(false);
    }
}