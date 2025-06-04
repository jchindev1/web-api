using System;
using System.Linq;
using web_api.web.Services;

namespace web_api.tests;

public class WeatherForecastTests
{
    [Fact]
    public void GetWeatherForecast_ShouldReturn5Forecasts()
    {
        // Arrange
        var svc = new WeatherForecastSvc();

        // Act
        var result = svc.GetWeatherForecast();

        // Assert
        Assert.Equal(5, result.Length);
    }

    [Fact]
    public void GetWeatherForecast_DatesShouldBeIncremental()
    {
        // Arrange
        var svc = new WeatherForecastSvc();
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var result = svc.GetWeatherForecast();

        // Assert
        for (int i = 0; i < result.Length; i++)
        {
            Assert.Equal(today.AddDays(i + 1), result[i].Date);
        }
    }

    [Fact]
    public void GetWeatherForecast_TemperatureCShouldBeWithinRange()
    {
        // Arrange
        var svc = new WeatherForecastSvc();

        // Act
        var result = svc.GetWeatherForecast();

        // Assert
        foreach (var forecast in result)
        {
            Assert.InRange(forecast.TemperatureC, -20, 55);
        }
    }

    [Fact]
    public void GetWeatherForecast_SummaryShouldNotBeNullOrEmpty()
    {
        // Arrange
        var svc = new WeatherForecastSvc();
        var validSummaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        // Act
        var result = svc.GetWeatherForecast();

        // Assert
        foreach (var forecast in result)
        {
            Assert.Contains(forecast.Summary, validSummaries);
        }
    }

    [Theory]
    [InlineData(-20, -4)]  // Min temperature
    [InlineData(0, 32)]    // Zero Celsius
    [InlineData(10, 50)]   // Mild temperature
    [InlineData(30, 86)]   // Warm temperature
    [InlineData(55, 131)]  // Max temperature
    public void WeatherForecast_TemperatureF_ShouldCalculateCorrectly(int celsius, int expectedFahrenheit)
    {
        // Arrange
        var forecast = new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now),
            celsius,
            "Test"
        );

        // Act & Assert
        Assert.Equal(expectedFahrenheit, forecast.TemperatureF);
    }
}
