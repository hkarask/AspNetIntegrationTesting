namespace Todos.Endpoints;

public interface ITodosService
{
    public Task<IReadOnlyList<Todo>> GetTodos();
}

public class TodosService : ITodosService
{
    private readonly HttpClient _httpClient;

    public TodosService(HttpClient httpClient) =>
        _httpClient = httpClient;

    public async Task<IReadOnlyList<Todo>> GetTodos() =>
        await _httpClient.GetFromJsonAsync<IReadOnlyList<Todo>>("/todos") ?? [];
}

public record Todo
{
    public int UserId { get; set; }

    public int Id { get; set; }

    public string Title { get; set; }

    public bool Completed { get; set; }
}
