using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using web_api.web.Data;
using web_api.web.Models;
using web_api.web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger UI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<WeatherForecastSvc>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Configure Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API v1");
    });
}

app.UseHttpsRedirection();

//Endpoints
app.MapGet("/weatherforecast", (WeatherForecastSvc weatherSvc) => weatherSvc.GetWeatherForecast())
.WithName("GetWeatherForecast")
.WithOpenApi();

// ToDo API endpoints
app.MapGet("/todos", async (ToDoContext context) =>
{
    return await context.ToDos.ToListAsync();
})
.WithName("GetToDos")
.WithOpenApi();

app.MapGet("/todos/{id}", async (int id, ToDoContext context) =>
{
    var todo = await context.ToDos.FindAsync(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
})
.WithName("GetToDo")
.WithOpenApi();

app.MapPost("/todos", async (ToDo todo, ToDoContext context) =>
{
    context.ToDos.Add(todo);
    await context.SaveChangesAsync();
    return Results.Created($"/todos/{todo.Id}", todo);
})
.WithName("CreateToDo")
.WithOpenApi();

app.MapPut("/todos/{id}", async (int id, ToDo inputTodo, ToDoContext context) =>
{
    var todo = await context.ToDos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    todo.Title = inputTodo.Title;
    todo.Description = inputTodo.Description;
    todo.IsCompleted = inputTodo.IsCompleted;

    await context.SaveChangesAsync();
    return Results.Ok(todo);
})
.WithName("UpdateToDo")
.WithOpenApi();

app.MapDelete("/todos/{id}", async (int id, ToDoContext context) =>
{
    var todo = await context.ToDos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    context.ToDos.Remove(todo);
    await context.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteToDo")
.WithOpenApi();

app.Run();