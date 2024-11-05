using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using My.Custom.Template.Common.Models.ResultPattern;
using Serilog;

namespace My.Custom.Template.Factories
{
    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly IHostEnvironment _environment;
        private readonly ApiBehaviorOptions _options;

        public CustomProblemDetailsFactory(
            IOptions<ApiBehaviorOptions> options,
            IHostEnvironment environment)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            statusCode ??= StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title ?? GetDefaultTitleForStatusCode(statusCode.Value),
                Type = type,
                Detail = detail,
                Instance = instance ?? httpContext?.TraceIdentifier
            };

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= StatusCodes.Status400BadRequest;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Title = title ?? "Validation Error",
                Type = type,
                Detail = detail,
                Instance = instance ?? httpContext?.TraceIdentifier
            };

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;

            if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            // Check for unhandled exceptions or multiple errors
            var exceptionFeature = httpContext?.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature?.Error is Exception exception)
            {
                Log.Error(exception, "An unhandled exception occurred");

                if (_environment.IsDevelopment())
                {
                    problemDetails.Extensions["exceptionMessage"] = exception.Message;
                    problemDetails.Extensions["exceptionType"] = exception.GetType().Name;
                    problemDetails.Extensions["stackTrace"] = exception.StackTrace;
                    problemDetails.Extensions["innerException"] = exception.InnerException?.Message;
                }
                else
                {
                    problemDetails.Detail = "An internal error occurred. Please contact support.";
                }
            }

            // Check if there are multiple errors in the HttpContext.Items
            if (httpContext?.Items["Errors"] is List<Error> { Count: > 0 } errors)
            {
                problemDetails.Extensions["errors"] = errors.Select(e => new
                {
                    message = e.Message,
                    statusCode = e.StatusCode,
                    code = e.Code
                }).ToList();
            }
        }

        private static string GetDefaultTitleForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad Request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status403Forbidden => "Forbidden",
                StatusCodes.Status404NotFound => "Not Found",
                StatusCodes.Status409Conflict => "Conflict",
                StatusCodes.Status500InternalServerError => "Internal Server Error",
                _ => "An unexpected error occurred"
            };
        }
    }
}
