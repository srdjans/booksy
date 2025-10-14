using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class BookRepository(StoreContext context) : IBookRepository
{
    public void AddBook(Book book)
    {
        context.Books.Add(book);
    }

    public bool BookExists(int id)
    {
        return context.Books.Any(x => x.Id == id);
    }

    public void DeleteBook(Book book)
    {
        context.Books.Remove(book);
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await context.Books.FindAsync(id);
    }

    public async Task<IReadOnlyList<Book>> GetBooksAsync()
    {
        return await context.Books.ToListAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateBook(Book book)
    {
        context.Entry(book).State = EntityState.Modified;
    }
}