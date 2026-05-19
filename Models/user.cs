using System.Text.Json.Serialization;

namespace boklista_api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Token { get; set; }
    public DateTime TokenExpiration { get; set; }

    [JsonIgnore]
    public ICollection<Book> Books { get; set; } = new List<Book>();

    [JsonIgnore]
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}