using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly StoreContext context;

    public BooksController(StoreContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await context.Books.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook(Book book)
    {
        context.Books.Add(book);

        await context.SaveChangesAsync();

        return book;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBook(int id, Book book)
    {
        if (book.Id != id || !BookExists(id))
        {
            return BadRequest("Cannot update this book");
        }

        context.Entry(book).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        context.Books.Remove(book);

        await context.SaveChangesAsync();

        return NoContent();
    }

    
    private bool BookExists(int id)
    {
        return context.Books.Any(x => x.Id == id);
    }
}