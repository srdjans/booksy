using Core.Entities;

namespace Core.Interfaces;

public interface IBookRepository
{
    Task<IReadOnlyList<Book>> GetBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    void AddBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Book book);
    bool BookExists(int id);
    Task<bool> SaveChangesAsync();
}