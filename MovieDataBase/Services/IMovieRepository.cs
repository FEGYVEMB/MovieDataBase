public interface IMovieRepository
{
    public List<Movie> GetAllMovies();
    public void Add(string title);

    public void Remove(Movie movie);
}

public class Movie 
{
    public string Name { get; set; }
        
    public Movie(string name)
    {
        Name = name;
        var id = Guid.NewGuid();
    }

    public bool Equals(string title)
    {
        return Name == title;
    }
}

public class InMemoryMovieRepository : IMovieRepository
{
    private List<Movie> _movies = new List<Movie>();

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

    public List<Movie> GetAllMovies()
    {
        return _movies;
    }

    public void Remove(Movie movie)
    {
        _movies.Remove(movie);
    }
}

