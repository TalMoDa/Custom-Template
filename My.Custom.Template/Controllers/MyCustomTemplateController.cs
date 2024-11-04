using MediatR;
using Microsoft.AspNetCore.Mvc;
using My.Custom.Template.Api.MyCustomTemplateController.GetUser;
using My.Custom.Template.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace My.Custom.Template.Controllers;

[Route("[controller]")]
public class MyCustomTemplateController : AppBaseController
{


    private readonly IMediator _mediator;

    public MyCustomTemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <response code="200">Returns the user with the specified id.</response>
    [HttpGet]
    [Route("/user/{id}")]
    [SwaggerOperation(Summary = "Get user by id")]
    [SwaggerResponse(200, "Returns the user with the specified id.", typeof(UserDto))]
    
    public async Task<IActionResult> GetUser([FromRoute (Name = "id")] int id, CancellationToken cancellationToken)
    {
        var query = await _mediator.Send(new GetUserQuery(id), cancellationToken);
        return ResultOf(query);
    }

}