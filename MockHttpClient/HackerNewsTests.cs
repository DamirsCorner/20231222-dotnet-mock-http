using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;

namespace MockHttpClient;

public class Tests
{
    [Test]
    public async Task TypedClientReturnsMockedResponse()
    {
        var handlerMock = new MockHttpMessageHandler();
        handlerMock
            .When(HttpMethod.Get, "https://hacker-news.firebaseio.com/v0/topstories.json")
            .Respond("application/json", "[1, 2, 3]");

        var httpClient = handlerMock.ToHttpClient();

        var hackerNews = new HackerNewsTypedClient(httpClient);
        var topStories = await hackerNews.GetTopStoriesAsync();

        topStories.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task BasicUsageReturnsMockedResponse()
    {
        var handlerMock = new MockHttpMessageHandler();
        handlerMock
            .When(HttpMethod.Get, "https://hacker-news.firebaseio.com/v0/topstories.json")
            .Respond("application/json", "[1, 2, 3]");

        var httpClient = handlerMock.ToHttpClient();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);


        var hackerNews = new HackerNewsBasicUsage(httpClientFactoryMock.Object);
        var topStories = await hackerNews.GetTopStoriesAsync();

        topStories.Should().BeEquivalentTo([1, 2, 3]);
    }
}