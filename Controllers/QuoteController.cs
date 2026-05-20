using Microsoft.AspNetCore.Mvc;
using boklista_api.Models;
using boklista_api.Services;
using Microsoft.AspNetCore.Authorization;


namespace boklista_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuoteController(IQuoteService quoteService) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAllQuotes()
        {
            var quotes = await quoteService.GetAllQuotesAsync();
            return Ok(quotes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuote(Guid id)
        {
            var quote = await quoteService.GetQuoteByIdAsync(id);
            if (quote == null) return NotFound();
            return Ok(quote);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuote(QuoteCreateDTO quoteDto)
        {
            var createdQuote = await quoteService.AddQuoteAsync(quoteDto);
            return CreatedAtAction(nameof(GetQuote), new { id = createdQuote.Id }, createdQuote);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(Guid id, QuoteDTO quoteDto)
        {
            if (id != quoteDto.UserId) return BadRequest();
            var existingQuote = await quoteService.UpdateQuoteAsync(id, quoteDto);
            if (existingQuote == null) return NotFound();
            // Placeholder for updating the quote in the service
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(Guid id)
        {
            var existingQuote = await quoteService.GetQuoteByIdAsync(id);
            if (existingQuote == null) return NotFound();
            // Placeholder for deleting the quote from the service
            await quoteService.DeleteQuoteAsync(id);
            return NoContent();
        }

    }
}