using Books.Shared;

namespace Books.App;

public class BooksClient
{
    private readonly HttpClient _httpClient;

    public BooksClient(HttpClient httpClient)
        => _httpClient = httpClient;

    public async Task<Book[]?> GetBooksAsync()
    {
        return await _httpClient.GetFromJsonAsync<Book[]>("/api/books");
    }
}
