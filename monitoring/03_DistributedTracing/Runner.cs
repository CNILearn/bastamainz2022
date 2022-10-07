using Microsoft.EntityFrameworkCore;

using System.Diagnostics.Metrics;

namespace DiagnosticsSample;

public class Runner
{
    public static readonly ActivitySource activitySource = new("DiagnosticsSample", "1.0");
    static readonly Meter meter = new("DiagnosticsSample", "1.0.0");
    static readonly Counter<int> fooCounter = meter.CreateCounter<int>("foo-counter", "foos", "the number of foos started");
    static readonly Counter<int> errors = meter.CreateCounter<int>("error-counter", "errors", "the number of errors");

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
        errors.Add(3);
    }

    public void Foo()
    {
        _logger.LogInformation("foo");
        fooCounter.Add(1);
        using var fooActivity = activitySource.StartActivity("Foo", ActivityKind.Internal);
        fooActivity?.AddEvent(new ActivityEvent("inFoo***"));
        fooActivity?.AddBaggage("bag1", "foobag");
        fooActivity?.AddTag("tag1", "footag");
        Bar();
        fooActivity?.Stop();
    }

    private void Bar()
    {
        using var barActivity = activitySource.StartActivity("Bar", ActivityKind.Internal);
        barActivity?.AddTag("tag2", "bartag");
        _logger.LogInformation("Bar");
        barActivity?.Stop();
    }

    public void AddARecord()
    {
        using var context = _contextFactory.CreateDbContext();
        context.Books.Add(new Book($"title {Random.Shared.Next(2000)}", "sample pub"));
        context.SaveChanges();
    }
}
