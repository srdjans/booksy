using System.Security.Cryptography.X509Certificates;
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
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
        string? authors,
        string? categories,
        string? sort,
        int pageNumber = 1,
        int pageSize = 10)
    {
        const int maxPageSize = 50;
        pageSize = Math.Min(pageSize, maxPageSize);

        var query = dbContext.Books.AsQueryable();

        // Filter by authors
        if (!string.IsNullOrWhiteSpace(authors))
        {
            var authorList = authors
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.ToLower())
                .ToArray();

            query = query.Where(x => authorList.Contains(x.Author.ToLower()));
        }
        
        // Filter by categories
        if (!string.IsNullOrWhiteSpace(categories))
        {
            var categoryList = categories
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.ToLower())
                .ToArray();

            query = query.Where(x => categoryList.Contains(x.Category.ToLower()));
        }

        // Sort
        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x => x.Title)
        };

        // Pagination
        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        // TODO: Add DTOs
        var book = await dbContext.Books.FindAsync(id);

        if (book == null)
            return NotFound();

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
            return BadRequest("Book ID mismatch");

        var existingBook = await dbContext.Books.FindAsync(id);

        if (existingBook == null)
            return NotFound();

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
            return NotFound();

        dbContext.Remove(book);

        if (await dbContext.SaveChangesAsync() > 0)
            return NoContent();

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