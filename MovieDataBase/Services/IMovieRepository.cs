public interface IMovieRepository
{
    public List<Movie> GetAllMovies();
    public void Add(string title);

    public void RemoveMovie(Movie movie);

    public void UpdateMovie(Movie movie, string updatedTitle);
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

    public bool Equals(string targetId)
    {
        return targetId == id;
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

    public void RemoveMovie(Movie movie)
    {
        _movies.Remove(movie);
    }

    public void UpdateMovie(Movie movie, string updatedTitle)
    {
        _movies.Where(m => m.Equals(movie)).First().Name = updatedTitle;
    }

}
