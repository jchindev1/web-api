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
builder.Services.AddScoped<ToDoSvc>();

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
app.MapGet("/todos", async (ToDoSvc toDoSvc) =>
{
    return await toDoSvc.GetAllAsync();
})
.WithName("GetToDos")
.WithOpenApi();

app.MapGet("/todos/{id}", async (int id, ToDoSvc toDoSvc) =>
{
    var todo = await toDoSvc.GetByIdAsync(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
})
.WithName("GetToDo")
.WithOpenApi();

app.MapPost("/todos", async (ToDo todo, ToDoSvc toDoSvc) =>
{
    var createdTodo = await toDoSvc.CreateAsync(todo);
    return Results.Created($"/todos/{createdTodo.Id}", createdTodo);
})
.WithName("CreateToDo")
.WithOpenApi();

app.MapPut("/todos/{id}", async (int id, ToDo inputTodo, ToDoSvc toDoSvc) =>
{
    inputTodo.Id = id; // Ensure the ID matches the route parameter
    var success = await toDoSvc.UpdateAsync(inputTodo);
    return success ? Results.Ok(await toDoSvc.GetByIdAsync(id)) : Results.NotFound();
})
.WithName("UpdateToDo")
.WithOpenApi();

app.MapDelete("/todos/{id}", async (int id, ToDoSvc toDoSvc) =>
{
    var success = await toDoSvc.DeleteAsync(id);
    return success ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteToDo")
.WithOpenApi();

app.Run();