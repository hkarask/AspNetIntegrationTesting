using System.Net;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;

namespace Todos.IntegrationTests;

public static class HttpMockExtensions
{
    /// <summary>
    /// Configures the mock to throw an exception for any request that doesn't have a matching setup.
    /// This is useful for catching any requests that were not explicitly mocked.
    /// </summary>
    public static Mock<HttpMessageHandler> CatchAllNonHandledRequests(this Mock<HttpMessageHandler> handler)
    {
        handler
            .Protected()
            .As<IHttpMessageHandler>()
            .Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(r => r.RequestUri != null), It.IsAny<CancellationToken>()))
            .Callback((HttpRequestMessage request, CancellationToken _) =>
                throw new InvalidOperationException($"UNHANDLED REQUEST '{request.Method} {request.RequestUri}', register a mock for it")
            );

        return handler;
    }

    public static Mock<HttpMessageHandler> SetupJsonPlaceHolder(this Mock<HttpMessageHandler> handler)
    {
        handler
            .SetupSendAsync(HttpMethod.Get, "https://jsonplaceholder.typicode.com/todosx")
            .ReturnsHttpResponseAsync(HttpStatusCode.OK, "ResponsePayloads/todos.json");

        return handler;
    }

    /// <summary>
    /// Sets up the mock to handle HTTP requests matching the specified method and URI.
    /// </summary>
    public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupSendAsync(
        this Mock<HttpMessageHandler> handler,
        HttpMethod method,
        string uri)
    {
        return handler.Protected()
            .As<IHttpMessageHandler>()
            .Setup(x => x.SendAsync(
                It.Is<HttpRequestMessage>(r => r.Method == method && r.RequestUri == new Uri(uri)),
                It.IsAny<CancellationToken>()
            ));
    }

    /// <summary>
    /// Returns a <see cref="HttpResponseMessage"/> with the given status code and
    /// the content of the file with the given name.
    /// </summary>
    public static void ReturnsHttpResponseAsync(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
        HttpStatusCode statusCode,
        string payloadFileName)
    {
        var jsonString = File.ReadAllText(payloadFileName);
        var stringContent = new StringContent(jsonString);
        var responseMessage = new HttpResponseMessage(statusCode)
        {
            Content = stringContent,
        };
        setup.ReturnsAsync(responseMessage);
    }
}

/// <summary>
/// An interface that allows mocking <see cref="HttpMessageHandler"/> more easily.
/// </summary>
public interface IHttpMessageHandler
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}
