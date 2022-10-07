using Books.Shared;

using System.Diagnostics;

namespace Books.App;

public class BooksClient
{
    private readonly HttpClient _httpClient;

    public BooksClient(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<Book[]?> GetBooksAsync()
    {
        using var activity = Tracing.StartGetBooksActivity();
        activity?.StartGetBooksEvent(_httpClient);
        
        var books = await _httpClient.GetFromJsonAsync<Book[]>("/api/books");

        activity.CompleteGetBooksEvent();
        
        return books;
    }
}
