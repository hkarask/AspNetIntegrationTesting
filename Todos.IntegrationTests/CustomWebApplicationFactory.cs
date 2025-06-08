using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Moq;

namespace Todos.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>()
                .CatchAllNonHandledRequests()
                .SetupJsonPlaceHolder();

            services.RemoveAll<HttpMessageHandlerBuilder>();
            services.AddSingleton<HttpMessageHandlerBuilder>(new TestHttpMessageHandler(httpMessageHandlerMock.Object));
        });

        builder.UseEnvironment("Test");
    }
}

public class TestHttpMessageHandler : HttpMessageHandlerBuilder
{
    private HttpMessageHandler _handler;

    public TestHttpMessageHandler(HttpMessageHandler handler) =>
        _handler = handler;

    public override IList<DelegatingHandler> AdditionalHandlers { get; } = [];

    public override string? Name { get; set; }

    public override HttpMessageHandler PrimaryHandler
    {
        get => _handler;
        set => _handler = value;
    }

    public override HttpMessageHandler Build() => _handler;
}
