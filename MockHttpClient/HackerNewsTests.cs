using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;

namespace MockHttpClient;

public class Tests
{
    [Test]
    public async Task TypedClientReturnsMockedResponse()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.SetupRequest(HttpMethod.Get, "https://hacker-news.firebaseio.com/v0/topstories.json")
            .ReturnsJsonResponse(new[] { 1, 2, 3 });

        var httpClient = handlerMock.CreateClient();

        var hackerNews = new HackerNewsTypedClient(httpClient);
        var topStories = await hackerNews.GetTopStoriesAsync();

        topStories.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task BasicUsageReturnsMockedResponse()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.SetupRequest(HttpMethod.Get, "https://hacker-news.firebaseio.com/v0/topstories.json")
            .ReturnsJsonResponse(new[] { 1, 2, 3 });
        var httpClientFactory = handlerMock.CreateClientFactory();

        var hackerNews = new HackerNewsBasicUsage(httpClientFactory);
        var topStories = await hackerNews.GetTopStoriesAsync();

        topStories.Should().BeEquivalentTo([1, 2, 3]);
    }
}