using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class BookRepository : IBookRepository
{
    public void AddBook(Book book)
    {
        throw new NotImplementedException();
    }

    public bool BookExists(int id)
    {
        throw new NotImplementedException();
    }

    public void DeleteBook(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<Book?> GetBookByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Book>> GetBooksAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public void UpdateBook(Book book)
    {
        throw new NotImplementedException();
    }
}