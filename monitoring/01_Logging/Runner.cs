using Microsoft.EntityFrameworkCore;

namespace DiagnosticsSample;

public class Runner
{
    private readonly ILogger _logger;
    private readonly IDbContextFactory<BooksContext> _contextFactory;
    
    public Runner(IDbContextFactory<BooksContext> contextFactory, ILogger<Runner> logger)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }
    
    public void InfoMessage1()
    {
        _logger.LogInformation("Sample1");
    }

    public void InfoMessage2()
    {
        _logger.GameStarted("a game");
    }

    public void ErrorMessage()
    {
        _logger.GameIdNotFound("4711");
    }

    public void Foo()
    {
        _logger.LogInformation("foo");
        Bar();
    }

    private void Bar()
    {
        _logger.LogInformation("Bar");
    }

    public void AddARecord()
    {
        using var context = _contextFactory.CreateDbContext();
        context.Database.EnsureCreated();
        context.Books.Add(new Book($"title {Random.Shared.Next(2000)}", "sample pub"));
        context.SaveChanges();
    }
}
