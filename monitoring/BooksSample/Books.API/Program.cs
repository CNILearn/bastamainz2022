using Books.API;
using Books.API.Services;

using var tracerProvider = Tracing.ConfigureTracing();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<BooksService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/books", (HttpContext context, BooksService booksService) =>
{
    using var activity = Tracing.StartGetBooksActivity(context.Request);
    
    var books = booksService.GetBooks();

    Tracing.CompleteGetBooks(activity);
    
    return Results.Ok(books);
});

app.MapGet("/api/weather", () =>
{
    string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var weather = Enumerable.Range(1, 5)
       .Select(index => new WeatherForecast(DateTime.Now.AddDays(index), Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]))
        .ToArray();
    return Results.Ok(weather);
});

app.Run();
