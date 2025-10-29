using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IGenericRepository<Book> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks(string? author, string? category, string? sort)
    {
        var spec = new BookSpecification(author, category, sort);
        var books = await repo.ListAsync(spec);

        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await repo.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        repo.Add(book);

        if (await repo.SaveAllAsync())
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

        repo.Update(book);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating book");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var book = await repo.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        repo.Remove(book);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting book");
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCategories()
    {
        var spec = new CategoryListSpecification();

        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("authors")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetAuthors()
    {
        var spec = new AuthorListSpecification();

        return Ok(await repo.ListAsync(spec));
    }

    private bool BookExists(int id)
    {
        // TODO: Implement method
        return repo.Exists(id);
    }
}