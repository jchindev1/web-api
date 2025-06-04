namespace web_api.web.Services
{
    public class WeatherForecastSvc
    {
        private string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public virtual WeatherForecast[] GetWeatherForecast()
        {
            var forecast = new WeatherForecast[5];
            for (int i = 0; i < 5; i++)
            {
                forecast[i] = new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(i + 1)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                );
            }

            var x = Enumerable.Range(2, 10).Select(index =>
            {
                return index;
            });

            return forecast;
        }

    }

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC * 9 / 5);
    }

}
