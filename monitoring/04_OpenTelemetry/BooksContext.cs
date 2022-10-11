using Microsoft.EntityFrameworkCore;

namespace DiagnosticsSample;

public class BooksContext : DbContext
{
    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
}

public record Book(string Title, string Publisher, int BookId = default);
