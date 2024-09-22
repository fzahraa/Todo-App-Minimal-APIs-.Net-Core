using Microsoft.EntityFrameworkCore;
using MinimalAPIToDoProject.Model;
using MinimalAPIToDoProject.ToDoDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TodoDB>(opt => opt.UseInMemoryDatabase("ToDo"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/todoitems", async (TodoDB db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/{id}", async (long id, TodoDB db) =>
    await db.Todos.FindAsync(id));

app.MapPost("/todoitems/{id}", async (TodoItem todo, TodoDB db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (long id, TodoItem inputTodo, TodoDB db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo == null) return Results.NotFound();
    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (long id, TodoDB db) =>
{
    if (await db.Todos.FindAsync(id) is TodoItem todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound() ;
});

app.Run();

