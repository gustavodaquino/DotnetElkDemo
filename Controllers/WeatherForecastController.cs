using Microsoft.AspNetCore.Mvc;

namespace DotnetElkDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var rng = new Random();
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast(
                DateTime.Now.AddDays(index),
                rng.Next(-20, 55),
                Summaries[rng.Next(Summaries.Length)]
            )
        ).ToArray();

        _logger.LogInformation("The highest temperature was {Temperature}", forecast.Max(x => x.TemperatureC));

        return forecast;
    }
}