using Books.Shared;

namespace Books.API.Services;

public class BooksService
{
    private readonly ILogger _logger;

    public BooksService(ILogger<BooksService> logger)
        => _logger = logger;

    private readonly List<Book> _books = new()
    {
        new ("Professional C# 7 and .NET Core 3", "Wrox Press", 1),
        new ("Professional C# 9 and .NET 5", "Wrox Press", 2)
    };

    public IEnumerable<Book> GetBooks()
    {
        _logger.LogTrace("GetBooks invoked");
        return _books;
    }
}
