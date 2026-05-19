namespace boklista_api.Models
{
    public class QuoteDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public Guid BookId { get; set; }
    }
}

namespace boklista_api.Models
{
    public class QuoteCardDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}

namespace boklista_api.Models
{
    public class QuoteCreateDTO
    {
        public string Text { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public Guid BookId { get; set; }
    }
}