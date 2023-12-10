namespace MockHttpClient;
public interface IHackerNews
{
    Task<IEnumerable<int>> GetTopStoriesAsync();
}
