using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(StoreContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string? author, string? category, string? sort)
    {
        var query = dbContext.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(x => x.Author == author);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(x => x.Category == category);
        }

        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _ => query
        };

        // TODO: Add pagination (+limit) and searching by multiple authors/categories

        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        // TODO: Add DTOs
        var book = await dbContext.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        dbContext.Books.Add(book);

        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBook(int id, Book updatedBook)
    {
        if (updatedBook.Id != id)
        {
            return BadRequest("Book ID mismatch");
        }

        var existingBook = await dbContext.Books.FindAsync(id);

        if (existingBook == null)
        {
            return NotFound();
        }

        existingBook.Title = updatedBook.Title;
        existingBook.Summary = updatedBook.Summary;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var book = await dbContext.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        dbContext.Remove(book);

        if (await dbContext.SaveChangesAsync() > 0)
        {
            return NoContent();
        }

        return BadRequest("Problem deleting book");
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCategories()
    {
        return await dbContext.Books.Select(x => x.Category)
            .Distinct()
            .ToListAsync();
    }

    [HttpGet("authors")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetAuthors()
    {
        return await dbContext.Books.Select(x => x.Author)
            .Distinct()
            .ToListAsync();
    }
}