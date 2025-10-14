using System.Net.Http.Headers;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBookRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string? author, string? category, string? sort)
    {
        return Ok(await repo.GetBooksAsync(author, category, sort));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await repo.GetBookByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        repo.AddBook(book);

        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        return BadRequest("Problem creating book");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBook(int id, Book book)
    {
        if (book.Id != id || !BookExists(id))
        {
            return BadRequest("Cannot update this book");
        }

        repo.UpdateBook(book);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating book");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var book = await repo.GetBookByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        repo.DeleteBook(book);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting book");
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCategories()
    {
        return Ok(await repo.GetCategoriesAsync());
    }

    [HttpGet("authors")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetAuthors()
    {
        return Ok(await repo.GetAuthorsAsync());
    }

    private bool BookExists(int id)
    {
        return repo.BookExists(id);
    }
}