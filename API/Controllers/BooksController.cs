using System.Security.Cryptography.X509Certificates;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.RequestHelpers;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(StoreContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Pagination<Book>>> GetBooks(
        string? authors,
        string? categories,
        string? sort,
        int pageNumber = 1,
        int pageSize = 10)
    {
        const int maxPageSize = 50;
        const int minPageNumber = 1;

        pageSize = Math.Min(pageSize, maxPageSize);
        pageNumber = Math.Max(pageNumber, minPageNumber);

        var query = dbContext.Books.AsQueryable();

        // Filters
        query = ApplyAuthorFilter(query, authors);
        query = ApplyCategoryFilter(query, categories);

        // Count 
        var count = await query.CountAsync();

        // Sort
        query = ApplySorting(query, sort);

        // Pagination
        var books = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var pagedResult = new Pagination<Book>(pageNumber, pageSize, count, books);

        return Ok(pagedResult);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
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

    private static IQueryable<Book> ApplyAuthorFilter(IQueryable<Book> query, string? authors)
    {
        if (string.IsNullOrWhiteSpace(authors))
            return query;

        var authorList = authors
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.ToLower())
            .ToArray();

        return query.Where(x => authorList.Contains(x.Author.ToLower()));
    }

    private static IQueryable<Book> ApplyCategoryFilter(IQueryable<Book> query, string? categories)
    {
        if (string.IsNullOrWhiteSpace(categories)) 
            return query;

        var categoryList = categories
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(x => x.ToLower())
            .ToArray();

        return query.Where(x => categoryList.Contains(x.Category.ToLower()));
    }

    private static IQueryable<Book> ApplySorting(IQueryable<Book> query, string? sort)
    {
        return sort switch
        {
            "priceAsc" => query.OrderBy(b => b.Price),
            "priceDesc" => query.OrderByDescending(b => b.Price),
            _ => query.OrderBy(b => b.Title)
        };   
    }
}