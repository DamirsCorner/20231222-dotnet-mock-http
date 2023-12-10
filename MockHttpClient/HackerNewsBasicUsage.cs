using System.Net.Http.Json;

namespace MockHttpClient;
public class HackerNewsBasicUsage(IHttpClientFactory httpClientFactory) : IHackerNews
{
    public async Task<IEnumerable<int>> GetTopStoriesAsync()
    {
        using var httpClient = httpClientFactory.CreateClient();
        return await httpClient.GetFromJsonAsync<List<int>>("https://hacker-news.firebaseio.com/v0/topstories.json") ?? [];
    }
}
