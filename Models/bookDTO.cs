
namespace boklista_api.Models
{
    public class BookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public Guid UserId { get; set; }
    }
}

namespace boklista_api.Models
{
    public class BookCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public Guid UserId { get; set; }
    }
}

namespace boklista_api.Models
{
    public class BookCardDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}