using Core.Entities;

namespace Core.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    public BookSpecification(string? author, string? category, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(author) || x.Author == author) &&
        (string.IsNullOrWhiteSpace(category) || x.Category == category))
    {
        switch (sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Title);
                break;
        }
    }
}