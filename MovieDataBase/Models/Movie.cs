public class Movie
{
    public string? Title { get; set; }
    public string? Id { get; set; }

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