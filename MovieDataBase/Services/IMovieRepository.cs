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
    public string id { get; set; }
      
    public Movie(string name)
    {
        Name = name;
        // this is the unique id manufacturer
        id = Guid.NewGuid().ToString();
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
        await Task.Delay(2000);
        return _movies;
    }

    public async void RemoveMovie(string id)
    {
        var deleted = await Task.Run(() => _movies.RemoveAll(m => m.id == id));
        if (deleted == 0) throw new KeyNotFoundException();
    }

    public void UpdateMovie(string id, string updatedTitle)
    {
        _movies.Where(m => m.id == id).First().Name = updatedTitle;
    }
}
