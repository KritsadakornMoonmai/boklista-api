using boklista_api.Models;

namespace boklista_api.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookCardDTO>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(Guid id);
        Task<Book> AddBookAsync(BookCreateDTO bookDto);
        Task<Book> UpdateBookAsync(Guid id, BookDTO bookDto);
        Task<bool> DeleteBookAsync(Guid id);
    }
}
