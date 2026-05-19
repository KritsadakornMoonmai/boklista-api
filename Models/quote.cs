using System.Text.Json.Serialization;

namespace boklista_api.Models;

public class Quote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;


    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
}