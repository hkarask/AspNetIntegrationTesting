using Todos.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ITodosService, TodosService>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
});

var app = builder.Build();

app.MapGet("/todos", async (ITodosService todosService) =>
    await todosService.GetTodos());

app.Run();

public partial class Program;
