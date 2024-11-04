using Microsoft.AspNetCore.Mvc;

namespace My.Custom.Template.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    
    [HttpGet("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        return Problem();
    }
    
}