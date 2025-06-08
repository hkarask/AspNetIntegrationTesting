# AspNetIntegrationTesting

Sample project for setting up integration tests in ASP.NET

## Mocking all HTTP calls with WebApplicationFactory

I've created a sample solution where all HTTP calls are captured using a test `MessageHandlerBuilder`. This is using a Mocked `HttpMessageHandler` where one can map URI and HTTP verb with JSON payloads that are getting returned.

```csharp
var httpMessageHandlerMock = new Mock<HttpMessageHandler>()
    .CatchAllNonHandledRequests()
    .SetupJsonPlaceHolder();
```

```csharp
public static Mock<HttpMessageHandler> SetupJsonPlaceHolder(this Mock<HttpMessageHandler> handler)
{
    handler
        .SetupSendAsync(HttpMethod.Get, "https://jsonplaceholder.typicode.com/todos")
        .ReturnsHttpResponseAsync(HttpStatusCode.OK, "ResponsePayloads/todos.json");

    return handler;
}
```

In case you've missed a mock for HTTP call, an exception gets thrown:

```
UNHANDLED REQUEST 'GET https://jsonplaceholder.typicode.com/todos', register a mock for it
   at Todos.IntegrationTests.HttpMockExtensions...
```
