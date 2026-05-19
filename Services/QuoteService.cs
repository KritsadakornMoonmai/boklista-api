using boklista_api.Data;
using boklista_api.Models;
using Microsoft.EntityFrameworkCore;

namespace boklista_api.Services
{
    public class QuoteService(BookListDbContext context, IUserService userService, IBookService bookService) : IQuoteService
    {
        public async Task<IEnumerable<QuoteCardDTO>> GetAllQuotesAsync()
        {
            List<QuoteCardDTO> quotes = await context.Quotes.Include(q => q.User).Include(q => q.Book).Select(q => new QuoteCardDTO
            {
                Id = q.Id,
                Text = q.Text,
                UserId = q.UserId,
                Username = q.User.Username,
                BookId = q.BookId,
                Title = q.Book.Title
            }).ToListAsync();


            return quotes;
        }

        public async Task<Quote> GetQuoteByIdAsync(Guid id)
        {
            return await context.Quotes.Include(q => q.User).Include(q => q.Book).FirstOrDefaultAsync(q => q.Id.Equals(id));
        }

        public async Task<Quote> AddQuoteAsync(QuoteCreateDTO quote)
        {
            User user = await userService.GetUserByIdAsync(quote.UserId);
            Book book = await bookService.GetBookByIdAsync(quote.BookId);

            Quote newQuote = new Quote
            {
                Text = quote.Text,
                User = user,
                Book = book,
            };
            context.Quotes.Add(newQuote);
            await context.SaveChangesAsync();
            return newQuote;
        }

        public async Task<Quote> UpdateQuoteAsync(Guid id, QuoteDTO quote)
        {
            var existingQuote = await context.Quotes.FindAsync(quote.Id);
            var existingUser = await context.Users.FindAsync(quote.UserId);
            if (existingQuote == null || existingUser == null || existingUser.Id != existingQuote.UserId) return null;

            existingQuote.Text = quote.Text;
            existingQuote.UserId = quote.UserId;
            existingQuote.BookId = quote.BookId;

            await context.SaveChangesAsync();
            return existingQuote;
        }

        public async Task<bool> DeleteQuoteAsync(Guid id)
        {
            var quote = await context.Quotes.FindAsync(id);
            if (quote == null) return false;

            context.Quotes.Remove(quote);
            await context.SaveChangesAsync();
            return true;
        }
    }
}