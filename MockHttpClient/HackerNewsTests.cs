using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace MockHttpClient;

public class Tests
{
    private HttpClient CreateMockedHttpClientWithResponse(IEnumerable<int> topStories)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(topStories),
            });
        return new HttpClient(handlerMock.Object);
    }

    [Test]
    public async Task TypedClientReturnsMockedResponse()
    {
        var httpClient = CreateMockedHttpClientWithResponse([1, 2, 3]);

        var hackerNews = new HackerNewsTypedClient(httpClient);
        var topStories = await hackerNews.GetTopStoriesAsync();

        topStories.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Test]
    public async Task BasicUsageReturnsMockedResponse()
    {
        var httpClient = CreateMockedHttpClientWithResponse([1, 2, 3]);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var hackerNews = new HackerNewsBasicUsage(httpClientFactoryMock.Object);
        var topStories = await hackerNews.GetTopStoriesAsync();

        topStories.Should().BeEquivalentTo([1, 2, 3]);
    }
}