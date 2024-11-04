using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using My.Custom.Template.ResultPattern;
using Serilog;

namespace My.Custom.Template.Controllers
{
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {
        protected IActionResult ResultOf<T>(Result<T> result, IActionResult? successResult = null, IActionResult? errorResult = null)
        {
            return result.IsSuccess 
                ? successResult ?? Ok(result.Value) 
                : errorResult ?? Problem(result);
        }

        private IActionResult Problem<T>(Result<T> result)
        {
            if (result.IsSuccess || result.Errors == null || !result.Errors.Any())
            {
                return Problem();
            }

            // Log detailed error information
            foreach (var error in result.Errors)
            {
                Log.Warning("Error encountered: {Message}, Code: {Code}, StatusCode: {StatusCode}",
                    error.Message, error.Code, error.StatusCode);
            }

            
            // Set custom HTTP context items for error tracking/logging if needed
            HttpContext.Items["Errors"] = result.Errors;

            // For simplicity, take the first error to generate ProblemDetails

            var statusCode = result.Errors.MaxBy(error => error.StatusCode).StatusCode;
            return Problem(
                statusCode: statusCode,
                title: GetTitleForStatusCode(statusCode),
                detail: string.Join(", ", result.Errors.Select(error => error.Message)),
                instance: HttpContext.TraceIdentifier
            );
        }

        private static string GetTitleForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad Request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status403Forbidden => "Forbidden",
                StatusCodes.Status404NotFound => "Not Found",
                StatusCodes.Status409Conflict => "Conflict",
                StatusCodes.Status500InternalServerError => "Internal Server Error",
                _ => "An Unexpected Error Occurred"
            };
        }
        
    }
}
