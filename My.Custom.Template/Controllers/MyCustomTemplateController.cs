using MediatR;
using Microsoft.AspNetCore.Mvc;
using My.Custom.Template.Api.MyCustomTemplateController.GetExample;
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
    /// <response code="200">Returns the example with the specified id.</response>
    [HttpGet]
    [Route("/example/{id}")]
    [SwaggerOperation(Summary = "Get example by id")]
    [SwaggerResponse(200, "Returns the example with the specified id.", typeof(ExampleDto))]
    
    public async Task<IActionResult> GetExample([FromRoute (Name = "id")] int id, CancellationToken cancellationToken)
    {
        var query = await _mediator.Send(new GetExampleQuery(id), cancellationToken);
        return ResultOf(query);
    }

}