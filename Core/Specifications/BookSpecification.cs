using Core.Entities;

namespace Core.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    public BookSpecification(string? author, string? category) : base(x =>
        (string.IsNullOrWhiteSpace(author) || x.Author == author) &&
        (string.IsNullOrWhiteSpace(category) || x.Category == category))
    {
        
    }
}