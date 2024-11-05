using FluentValidation;
using MediatR;
using My.Custom.Template.Common.Models.ResultPattern;
using My.Custom.Template.Data.Repositories.Interfaces;
using My.Custom.Template.Dto;

namespace My.Custom.Template.Api.MyCustomTemplateController.GetExample;



public record GetExampleQuery(int Id) : IRequest<Result<ExampleDto>>;

public class getExampleQueryValidator : AbstractValidator<GetExampleQuery>
{
    public getExampleQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}


public class GetExampleQueryHandler : IRequestHandler<GetExampleQuery, Result<ExampleDto>>
{
    private readonly IExampleRepository _exampleRepository;
    
    public GetExampleQueryHandler(IExampleRepository exampleRepository)
    {
        _exampleRepository = exampleRepository;
    }
    public async Task<Result<ExampleDto>> Handle(GetExampleQuery request, CancellationToken cancellationToken)
    {
        var example = await _exampleRepository.GetExampleAsNoTrackingAsync(request.Id, cancellationToken);
        
        if (example is null)
        {
            return Error.NotFound($"User with id {request.Id} was not found");
        }
        
        return new ExampleDto(example.Id, example.Name);
        
        
    }
}
