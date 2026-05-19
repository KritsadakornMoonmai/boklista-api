using boklista_api.Models;

namespace boklista_api.Services
{
    public interface IQuoteService
    {
        Task<IEnumerable<QuoteCardDTO>> GetAllQuotesAsync();
        Task<Quote> GetQuoteByIdAsync(Guid id);
        Task<Quote> AddQuoteAsync(QuoteCreateDTO quote);
        Task<Quote> UpdateQuoteAsync(Guid id, QuoteDTO quote);
        Task<bool> DeleteQuoteAsync(Guid id);
    }
}