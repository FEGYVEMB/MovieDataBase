using Dapper;
using Npgsql;

public class DbMovieRepository : IMovieRepository
{
    private readonly string connectionString;
    private NpgsqlConnection _connection;

    public DbMovieRepository(IConfiguration configuration, NpgsqlConnection connection)
    {
        connectionString = configuration.GetValue<string>("MovieDBConnectionString");
        _connection = connection;
        _connection.ExecuteAsync($"create table if not exists movies(id VARCHAR(50), title VARCHAR(50));");
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            return (await connection.QueryAsync<Movie>("select * from movies;")).ToList();
        }
    }
    public async Task<Movie> GetMovieByIdAsync(string id)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            if(!(await MovieExistsInDbAsync(connection, id)))
            {
                throw new Exception();
            }
            
            string sqlScript = $"select title from movies where id = '{id}'";
                var resultTitle = connection.ExecuteScalarAsync<string>(sqlScript, new {id});
                Movie resultMovie = new Movie(await resultTitle, id);

                return resultMovie;
        }
    }

    public async void AddAsync(string title)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            await connection.ExecuteAsync($"insert into movies (id, title) values ('{Guid.NewGuid()}', '{title}');");
        }
    }

    public async void RemoveMovieAsync(string id)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            if(!(await MovieExistsInDbAsync(connection, id)))
            {
                throw new Exception();
            }

            await connection.ExecuteAsync($"delete from movies where id = '{id}';");
        }
    }

    public async void UpdateMovieAsync(string id, string updatedTitle)
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            if(!(await MovieExistsInDbAsync(connection, id)))
            {
                throw new Exception();
            }
            
            await connection.ExecuteAsync($"update movies set title = '{updatedTitle}' where id = '{id}';");
        }
    }

    public Task<bool> MovieExists(string id) 
    {
        using(var connection = new NpgsqlConnection(connectionString))
        {
            return MovieExistsInDbAsync(connection, id);
        }
    }
    
        private async Task<bool> MovieExistsInDbAsync(NpgsqlConnection connection, string id) 
    {
        string sqlScript = "select count(distinct 1) from movies where Id=@id";
        //bool exists = await connection.ExecuteScalarAsync<bool>(sqlScript, new {id});
         var exists = await connection.ExecuteScalarAsync<bool>(sqlScript, new {id});
    
        return exists;
    }
}
