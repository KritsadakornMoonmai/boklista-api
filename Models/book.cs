using System.Text.Json.Serialization;

namespace boklista_api.Models;

public class Book
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}