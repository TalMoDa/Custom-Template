using MediatR;
using Microsoft.AspNetCore.Mvc;
using My.Custom.Template.Api.MyCustomTemplateController.GetWeatherForecast;

namespace My.Custom.Template.Controllers;

[Route("[controller]")]
public class MyCustomTemplateController : AppBaseController
{


    private readonly IMediator _mediator;

    public MyCustomTemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(typeof(WeatherForecast), 200)]
    public async Task<IActionResult> GetWeatherForecast()
    {
        var query = await _mediator.Send(new GetWeatherForecastQuery(0));
        return ResultOf(query);

    }
}