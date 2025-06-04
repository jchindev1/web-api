using System.Reflection.Metadata.Ecma335;
using web_api.web.Services;
using web_api.web.Models;

var builder = WebApplication.CreateBuilder(args);

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


app.MapGet("/weatherforecast", (WeatherForecastSvc weatherSvc) => weatherSvc.GetWeatherForecast())
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/todo", async (ToDoSvc svc) => await svc.GetAllAsync())
    .WithName("GetAllToDos")
    .WithOpenApi();

app.MapGet("/todo/{id:int}", async (int id, ToDoSvc svc) =>
{
    var todo = await svc.GetByIdAsync(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
})
.WithName("GetToDoById")
.WithOpenApi();

app.MapPost("/todo", async (ToDo toDo, ToDoSvc svc) =>
{
    var created = await svc.CreateAsync(toDo);
    return Results.Created($"/todo/{created.Id}", created);
})
.WithName("CreateToDo")
.WithOpenApi();

app.MapPut("/todo/{id:int}", async (int id, ToDo toDo, ToDoSvc svc) =>
{
    toDo.Id = id;
    var updated = await svc.UpdateAsync(toDo);
    return updated ? Results.NoContent() : Results.NotFound();
})
.WithName("UpdateToDo")
.WithOpenApi();

app.MapDelete("/todo/{id:int}", async (int id, ToDoSvc svc) =>
{
    var deleted = await svc.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
})
.WithName("DeleteToDo")
.WithOpenApi();

app.Run();