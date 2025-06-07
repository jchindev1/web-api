using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api.web.Data;
using web_api.web.Models;
using web_api.web.Repositories;
using web_api.web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

// Add CacheService and IMemoryCache to DI
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CacheService>();

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


//Padlet endpoints

app.MapGet("/rates", async ([FromQuery(Name = "base")] string baseValue) =>
{
    if (baseValue == "fiat")
    {
        var fiatCurrencies = new[] { "USD", "SGD", "EUR" };
        var httpClient = new HttpClient();
        var results = new Dictionary<string, Dictionary<string, string>>();

        foreach (var fiat in fiatCurrencies)
        {
            var fiatUrl = $"https://api.coinbase.com/v2/exchange-rates?currency={Uri.EscapeDataString(fiat)}";
            var response = await httpClient.GetAsync(fiatUrl);
            if (!response.IsSuccessStatusCode)
            {
                continue;
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(responseBody);
            var rates = doc.RootElement
                .GetProperty("data")
                .GetProperty("rates");

            var selectedRates = new Dictionary<string, string>();
            foreach (var symbol in new[] { "BTC", "DOGE", "ETH" })
            {
                if (rates.TryGetProperty(symbol, out var rateProp))
                {
                    selectedRates[symbol] = rateProp.GetString();
                }
            }
            results[fiat] = selectedRates;
        }

        return Results.Ok(results);
    }

    if (baseValue == "tokens")
    {
        var tokenSymbols = new[] { "BTC", "DOGE", "ETH" };
        var httpClient = new HttpClient();
        var results = new Dictionary<string, Dictionary<string, string>>();

        foreach (var token in tokenSymbols)
        {
            var tokenUrl = $"https://api.coinbase.com/v2/exchange-rates?currency={Uri.EscapeDataString(token)}";
            var response = await httpClient.GetAsync(tokenUrl);
            if (!response.IsSuccessStatusCode)
            {
                continue;
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(responseBody);
            var rates = doc.RootElement
            .GetProperty("data")
            .GetProperty("rates");

            // Select only fiat currencies
            var selectedRates = new Dictionary<string, string>();
            foreach (var fiat in new[] { "USD", "SGD", "EUR" })
            {
                if (rates.TryGetProperty(fiat, out var rateProp))
                {
                    selectedRates[fiat] = rateProp.GetString();
                }
            }
            results[token] = selectedRates;
        }

        return Results.Ok(results);
    }

    return Results.NotFound();
}).WithName("GetRates")
.WithOpenApi();


app.MapPost("/webhook", ([FromBody] WebhookPayload payload) =>
{
    // Example: log or process the payload as needed
    // For now, just return the received payload for demonstration
    return Results.Ok(payload);
})
.WithName("webhook")
.WithOpenApi();

app.MapGet("/rates1", (
    [FromQuery(Name = "base_currency")] string baseCurrency,
    [FromQuery(Name = "target_currency")] string targetCurrency,
    [FromQuery(Name = "start")] long? start,
    [FromQuery(Name = "end")] long? end) =>
{
    // Example: Validate input
    if (string.IsNullOrWhiteSpace(baseCurrency) || string.IsNullOrWhiteSpace(targetCurrency) || start is null || end is null)
    {
        return Results.BadRequest("Missing required query parameters.");
    }

    // TODO: Replace with actual rate lookup logic
    var result = new
    {
        BaseCurrency = baseCurrency,
        TargetCurrency = targetCurrency,
        Start = start,
        End = end,
        Rates = new[]
        {
            new { Timestamp = start, Rate = "12345.67" },
            new { Timestamp = end, Rate = "12500.00" }
        }
    };

    return Results.Ok(result);
})
.WithName("GetRatesByCurrencyAndRange")
.WithOpenApi();

app.Run();

public record WebhookPayload(
    string Type,
    WebhookData Data
);

public record WebhookData(
    [property: System.Text.Json.Serialization.JsonPropertyName("base_currency")]
    string BaseCurrency,
    [property: System.Text.Json.Serialization.JsonPropertyName("published_at")]
    long PublishedAt,
    [property: System.Text.Json.Serialization.JsonPropertyName("rates")]
    Dictionary<string, string> Rates
);






//app.MapGet("/weatherforecast", (WeatherForecastSvc weatherSvc) => weatherSvc.GetWeatherForecast())
//.WithName("GetWeatherForecast")
//.WithOpenApi();


////Endpoints
//app.MapGet("/weatherforecast", (WeatherForecastSvc weatherSvc) => weatherSvc.GetWeatherForecast())
//.WithName("GetWeatherForecast")
//.WithOpenApi();

//// ToDo API endpoints
//app.MapGet("/todos", async (ToDoSvc toDoSvc) =>
//{
//    return await toDoSvc.GetAllAsync();
//})
//.WithName("GetToDos")
//.WithOpenApi();

//app.MapGet("/todos/{id}", async (int id, ToDoSvc toDoSvc) =>
//{
//    var todo = await toDoSvc.GetByIdAsync(id);
//    return todo is not null ? Results.Ok(todo) : Results.NotFound();
//})
//.WithName("GetToDo")
//.WithOpenApi();

//app.MapPost("/todos", async (ToDo todo, ToDoSvc toDoSvc) =>
//{
//    var createdTodo = await toDoSvc.CreateAsync(todo);
//    return Results.Created($"/todos/{createdTodo.Id}", createdTodo);
//})
//.WithName("CreateToDo")
//.WithOpenApi();

//app.MapPut("/todos/{id}", async (int id, ToDo inputTodo, ToDoSvc toDoSvc) =>
//{
//    inputTodo.Id = id; // Ensure the ID matches the route parameter
//    var success = await toDoSvc.UpdateAsync(inputTodo);
//    return success ? Results.Ok(await toDoSvc.GetByIdAsync(id)) : Results.NotFound();
//})
//.WithName("UpdateToDo")
//.WithOpenApi();

//app.MapDelete("/todos/{id}", async (int id, ToDoSvc toDoSvc) =>
//{
//    var success = await toDoSvc.DeleteAsync(id);
//    return success ? Results.NoContent() : Results.NotFound();
//})
//.WithName("DeleteToDo")
//.WithOpenApi();

