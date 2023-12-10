using System.Net.Http.Json;

namespace MockHttpClient;
public class HackerNewsTypedClient(HttpClient httpClient) : IHackerNews
{
    public async Task<IEnumerable<int>> GetTopStoriesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<int>>("https://hacker-news.firebaseio.com/v0/topstories.json") ?? [];
    }
}
