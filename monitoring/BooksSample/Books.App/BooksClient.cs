using Books.Shared;

using System.Diagnostics;

namespace Books.App;

public class BooksClient
{
    private readonly HttpClient _httpClient;
    internal readonly static ActivitySource ActivitySource = new("Books.App", "2.0");

    public BooksClient(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<Book[]?> GetBooksAsync()
    {
        using var activity = ActivitySource.StartActivity("GetBooks", ActivityKind.Client);
        activity?.SetBaggage("BaseAddress", _httpClient.BaseAddress?.ToString());
        activity?.AddEvent(new ActivityEvent("RequestGetBooksEvent"));
        
        var books = await _httpClient.GetFromJsonAsync<Book[]>("/api/books");

        activity?.AddEvent(new ActivityEvent("GetBooksReceivedEvent"));
        return books;
    }
}
