using Core.Entities;

namespace Core.Specifications;

public class AuthorListSpecification : BaseSpecification<Book, string>
{
    public AuthorListSpecification()
    {
        AddSelect(x => x.Author);
        ApplyDistinct();
    }
}