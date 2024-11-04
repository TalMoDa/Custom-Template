using My.Custom.Template.ResultPattern;

namespace My.Custom.Template.Services.Implementations;

public class ExampleService : IExampleService
{
    public async Task<Result<WeatherForecast>> GetWeatherForecastAsync()
    {
        return Error.Conflict("This is a conflict error");

        return new WeatherForecast();

        

    }
}