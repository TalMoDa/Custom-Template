using My.Custom.Template.ResultPattern;

namespace My.Custom.Template;

public interface IExampleService
{
    Task<Result<WeatherForecast>> GetWeatherForecastAsync();
}