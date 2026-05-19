using Microsoft.AspNetCore.Mvc;
using boklista_api.Models;
using boklista_api.Services;

namespace boklista_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var book = await bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(BookCreateDTO bookDto)
    {
        var createdBook = await bookService.AddBookAsync(bookDto);
        return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
    }


    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateBook(Guid userId, BookDTO bookDto)
    {
        var existingBook = await bookService.UpdateBookAsync(userId, bookDto);
        if (existingBook == null) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {


        var existingBook = await bookService.GetBookByIdAsync(id);
        if (existingBook == null)
        {
            return NotFound();
        }
        // Placeholder for deleting the book from the service
        await bookService.DeleteBookAsync(id);
        return NoContent();
    }

}