using System.Security.Principal;
using boklista_api.Data;
using boklista_api.Models;
using Microsoft.EntityFrameworkCore;

namespace boklista_api.Services
{
    public class BookService(BookListDbContext context) : IBookService
    {
        public async Task<Book> AddBookAsync(BookCreateDTO bookDto)
        {
            var existingUser = await context.Users.FindAsync(bookDto.UserId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                PublicationYear = bookDto.PublicationYear,
                User = existingUser
            };

            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null) return false;

            context.Books.Remove(book);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookCardDTO>> GetAllBooksAsync()
        {
            return await context.Books.Select(u => new BookCardDTO
            {
                Id = u.Id,
                Title = u.Title,
                Author = u.Author,
                PublicationYear = u.PublicationYear,
                UserId = u.UserId,
                Username = u.User.Username,
            }).ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await context.Books.FindAsync(id);
        }

        public async Task<Book> UpdateBookAsync(Guid id, BookDTO bookDto)
        {
            var existingBook = await context.Books.FindAsync(bookDto.Id);
            var existingUser = await context.Users.FindAsync(id);
            Console.WriteLine($"Existing user {existingUser != null}");
            Console.WriteLine($"Existing book {existingBook != null}");
            if (existingBook == null || existingUser == null || existingUser.Id != existingBook.UserId) return null;

            // Update the properties of the existing book with the new values
            existingBook.Title = bookDto.Title;
            existingBook.Author = bookDto.Author;
            existingBook.PublicationYear = bookDto.PublicationYear;
            // ... update other properties as needed

            await context.SaveChangesAsync();
            return existingBook;
        }
    }
}

