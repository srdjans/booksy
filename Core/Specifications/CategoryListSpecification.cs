using Core.Entities;

namespace Core.Specifications;

public class CategoryListSpecification : BaseSpecification<Book, string>
{
    public CategoryListSpecification()
    {
        AddSelect(x => x.Category);
        ApplyDistinct();
    }
}