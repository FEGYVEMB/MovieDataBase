public interface IMovieRepository
{
    public Task<List<Movie>> GetAllMovies();
    public void Add(string title);

    public void RemoveMovie(string id);

    public void UpdateMovie(string id, string updatedTitle);
}

public class Movie
{
    public string Name { get; set; }
    public string Id { get; set; }

    public Movie(string name)
    {
        Name = name;
        // this is the unique id manufacturer
        Id = Guid.NewGuid().ToString();
    }
}

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
        var updateTask = await Task.FromResult(_movies.Where(m => m.Id == id).First().Name = updatedTitle);
    }
}

public class DbMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies;

    public DbMovieRepository()
    {

    }

    public async Task<List<Movie>> GetAllMovies()
    {
            return null;
    }

    public void Add(string title)
    {
    }

    public void RemoveMovie(string id)
    {

    }

    public void UpdateMovie(string id, string updatedTitle)
    {

    }
}
