using Dapper;
using Npgsql;
public interface IMovieRepository
{
    public Task<List<Movie>> GetAllMovies();
    public void Add(string title);
    public void RemoveMovie(string id);
    public void UpdateMovie(string id, string updatedTitle);
}

public class Movie
{
    public string Title { get; set; }
    public string Id { get; set; }

    public Movie(string title)
    {
        Title = title;
        // this is the unique id manufacturer
        Id = Guid.NewGuid().ToString();
    }
    
    public Movie()
    {
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
        var updateTask = await Task.FromResult(_movies.Where(m => m.Id == id).First().Title = updatedTitle);
    }
}

public class DbMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies;
    private readonly string connectionString;

    public DbMovieRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetValue<string>("MovieDBConnectionString");
        using(var connection = new NpgsqlConnection(connectionString))
        {
            connection.ExecuteAsync($"create table if not exists movies(id VARCHAR(50), title VARCHAR(50));");
        }
    }

    public async Task<List<Movie>> GetAllMovies()
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            return (await connection.QueryAsync<Movie>("select * from movies;")).ToList();
        }
    }

    public async void Add(string title)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            await connection.ExecuteAsync($"insert into movies (id, title) values ('{Guid.NewGuid()}', '{title}');");
        }
    }

    public async void RemoveMovie(string id)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            
            await connection.ExecuteAsync($"delete from movies where id = '{id}';");
        }
    }

    public async void UpdateMovie(string id, string updatedTitle)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            await connection.ExecuteAsync($"update movies set title = '{updatedTitle}' where id = '{id}';");
        }
    }
}
