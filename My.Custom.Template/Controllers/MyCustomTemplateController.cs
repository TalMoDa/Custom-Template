using Microsoft.AspNetCore.Mvc;

namespace My.Custom.Template.Controllers;

[Route("[controller]")]
public class MyCustomTemplateController : AppBaseController
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<MyCustomTemplateController> _logger;
    private readonly IExampleService _exampleService;

    public MyCustomTemplateController(ILogger<MyCustomTemplateController> logger, IExampleService exampleService)
    {
        _logger = logger;
        _exampleService = exampleService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        throw new Exception("This is an exception");
        var query = await _exampleService.GetWeatherForecastAsync();
        return ResultOf(query);

    }
}