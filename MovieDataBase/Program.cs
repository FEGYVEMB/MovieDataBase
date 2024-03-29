using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection magic
builder.Services.AddSingleton<IMovieRepository, DbMovieRepository>(); // factory so simple it is not even factory
builder.Services.AddSingleton<NpgsqlConnection>(serviceProvider => { 
    var config = serviceProvider.GetService<IConfiguration>();
    return new NpgsqlConnection(config.GetValue<string>("MovieDBConnectionString")); 
    }); // factory
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
