using Dapper;
using Npgsql;
public interface IMovieRepository
{
    public Task<List<Movie>> GetAllMovies();
    public Task<Movie> GetMovieById(string id);
    public void Add(string title);
    public void RemoveMovie(string id);
    public void UpdateMovie(string id, string updatedTitle);
    public Task<bool> MovieExists(string id);
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

    public Movie(string title, string id)
    {
        Title = title;
        Id = id;
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

public class DbMovieRepository : IMovieRepository
{
    // private readonly List<Movie> _movies;
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

    public async Task<Movie> GetMovieById(string id)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            string sqlScript = $"select title from movies where id = '{id}'";

            if(await MovieExistsInDb(connection, id))
            {
                var resultTitle = connection.ExecuteScalarAsync<string>(sqlScript, new {id});
                Movie resultMovie = new Movie(await resultTitle, id);

                return resultMovie;
            }
            else
            {
                return new Movie();
            }
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

    public async Task<bool> MovieExists(string id) 
    {

        using(var connection = new NpgsqlConnection(connectionString))
        {
            return await MovieExistsInDb(connection, id);
    
        }
    }

        private async Task<bool> MovieExistsInDb(NpgsqlConnection connection, string id) 
    {

        string sqlScript = $"select 1 from movies where exists (select count(1) from movies where id = '{id}')";
        bool exists = await connection.ExecuteScalarAsync<bool>(sqlScript, new {id});
    
        return exists;   
    }
}
