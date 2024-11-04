using FluentValidation;
using MediatR;
using My.Custom.Template.ResultPattern;

namespace My.Custom.Template.Api.MyCustomTemplateController.GetWeatherForecast;



public record GetWeatherForecastQuery(int Id) : IRequest<Result<WeatherForecast>>;

public class getWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastQuery>
{
    public getWeatherForecastQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}


public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, Result<WeatherForecast>>
{
    public async Task<Result<WeatherForecast>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        
        var errors = new List<Error>();
        
        errors.Add(Error.Conflict("Not implemented1"));
        errors.Add(Error.Conflict("Not implemented2"));

        return errors;
        return new WeatherForecast();
        
    }
}
