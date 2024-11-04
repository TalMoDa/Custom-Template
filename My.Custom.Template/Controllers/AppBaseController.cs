using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using My.Custom.Template.ResultPattern;
using Serilog;

namespace My.Custom.Template.Controllers
{
    [ApiController]
    public abstract class AppBaseController : ControllerBase
    {
        /// <summary>
        /// Handles the result, returning an Ok result if successful or a ProblemDetails result for errors.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="result">The result to process.</param>
        /// <param name="successResult">Optional custom success result.</param>
        /// <param name="errorResult">Optional custom error result.</param>
        /// <returns>IActionResult representing either success or failure.</returns>
        protected IActionResult ResultOf<T>(Result<T> result, IActionResult? successResult = null)
        {
            return result.IsSuccess 
                ? successResult ?? Ok(result.Value) 
                : Problem(result);
        }

        /// <summary>
        /// Generates a ProblemDetails response based on the given Result<T>.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="result">The failed result containing error information.</param>
        /// <returns>IActionResult with ProblemDetails.</returns>
        private IActionResult Problem<T>(Result<T> result)
        {
            if (result.IsSuccess || result.Error == null)
            {
                return Problem();
            }

            // Log detailed error information
            Log.Warning("Error encountered: {Error}, Code: {Code}, StatusCode: {StatusCode}", result.Error.Message, result.Error.Code, result.Error.StatusCode);

            // Handle validation errors specifically (Status Code 400)
            if (result.Error.StatusCode == StatusCodes.Status400BadRequest)
            {
                return ValidationProblem(new List<string> { result.Error.Message });
            }

            // Set custom HTTP context items for error tracking/logging if needed
            HttpContext.Items["Error"] = result.Error;

            // Return detailed ProblemDetails response
            return Problem(
                statusCode: result.Error.StatusCode,
                title: GetTitleForStatusCode(result.Error.StatusCode),
                detail: result.Error.Message,
                instance: HttpContext.TraceIdentifier
            );
        }

        /// <summary>
        /// Provides a descriptive title based on the HTTP status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>A string representing the title for the given status code.</returns>
        private static string GetTitleForStatusCode(int? statusCode)
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

        /// <summary>
        /// Converts a list of validation errors to a ValidationProblemDetails response.
        /// </summary>
        /// <param name="errorMessages">A list of validation error messages.</param>
        /// <returns>An IActionResult with ValidationProblemDetails.</returns>
        private IActionResult ValidationProblem(List<string> errorMessages)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in errorMessages)
            {
                modelState.AddModelError(string.Empty, error);
            }

            return ValidationProblem(modelState);
        }
    }
}
