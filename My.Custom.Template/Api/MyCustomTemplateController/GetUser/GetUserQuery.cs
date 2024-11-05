using FluentValidation;
using MediatR;
using My.Custom.Template.Common.Models.ResultPattern;
using My.Custom.Template.Data.Repositories.Interfaces;
using My.Custom.Template.Dto;

namespace My.Custom.Template.Api.MyCustomTemplateController.GetUser;



public record GetUserQuery(int Id) : IRequest<Result<UserDto>>;

public class getUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public getUserQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
    }
}


public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;
    
    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Error.NotFound($"User with id {request.Id} was not found");
        }
        
        return new UserDto(user.Id, user.Name);
        
        
    }
}
