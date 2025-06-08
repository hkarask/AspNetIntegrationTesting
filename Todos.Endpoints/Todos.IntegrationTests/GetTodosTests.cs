using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Todos.Endpoints;

namespace Todos.IntegrationTests;

public class GetTodosTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GetTodosTests(CustomWebApplicationFactory factory) =>
        _factory = factory;

    [Fact]
    public async Task Test1()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var todos = await client.GetFromJsonAsync<Todo[]>("/todos");

        // Assert
        todos!.Length.ShouldBe(9);
        todos[0].ShouldBeEquivalentTo(new Todo
        {
            UserId = 1,
            Id = 1,
            Title = "delectus aut autem",
            Completed = false,
        });
    }
}
