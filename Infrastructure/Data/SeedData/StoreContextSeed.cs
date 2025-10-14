using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.Books.Any())
        {
            var booksData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/books.json");

            var books = JsonSerializer.Deserialize<List<Book>>(booksData);

            if (books != null)
            {
                context.Books.AddRange(books);

                await context.SaveChangesAsync();
            }
        }
    }
}