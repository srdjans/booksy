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

    public async Task<IReadOnlyList<string>> GetAuthorsAsync()
    {
        return await context.Books.Select(x => x.Author)
            .Distinct()
            .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await context.Books.FindAsync(id);
    }

    public async Task<IReadOnlyList<Book>> GetBooksAsync(string? author, string? category, string? sort)
    {
        var query = context.Books.AsQueryable();

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
            _ => query.OrderBy(x => x.Title)
        };

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetCategoriesAsync()
    {
        return await context.Books.Select(x => x.Category)
            .Distinct()
            .ToListAsync();
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