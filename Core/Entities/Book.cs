namespace Core.Entities;

public class Book : BaseEntity
{
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }
    public required string Author { get; set; }
    public required string Category { get; set; }
    public int QuantityInStock { get; set; }
}