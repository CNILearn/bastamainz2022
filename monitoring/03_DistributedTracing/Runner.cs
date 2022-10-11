using Microsoft.EntityFrameworkCore;

using System.Diagnostics.Metrics;

namespace DiagnosticsSample;

public class Runner
{
    public static readonly ActivitySource activitySource = new("DiagnosticsSample", "1.0");
    static readonly Meter s_meter = new("DiagnosticsSample", "1.0.0");
    private static int s_observableFooValue = 0;
    static readonly ObservableCounter<int> s_observableFooCounter = s_meter.CreateObservableCounter("foo-counter2", () => s_observableFooValue);
    static readonly Histogram<int> s_histogramCounter = s_meter.CreateHistogram<int>("histogram-foos");
    static readonly Counter<int> s_fooCounter = s_meter.CreateCounter<int>("foo-counter", "foos", "the number of foos started");
    static readonly Counter<int> s_errors = s_meter.CreateCounter<int>("error-counter", "errors", "the number of errors");

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
        s_errors.Add(3);
    }

    public void Foo()
    {
        _logger.LogInformation("foo");
        s_fooCounter.Add(1);

        using var fooActivity = activitySource.StartActivity("Foo", ActivityKind.Internal);
        fooActivity?.AddEvent(new ActivityEvent("inFoo***"));
        fooActivity?.AddBaggage("bag1", "foobag");
        fooActivity?.AddTag("tag1", "footag");
        
        Bar();
        fooActivity?.Stop();
        s_observableFooValue++;
        s_histogramCounter.Record(Random.Shared.Next(5, 25));
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
