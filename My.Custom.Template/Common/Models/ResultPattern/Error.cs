namespace My.Custom.Template.Common.Models.ResultPattern;

public class Error
{
    public string Message { get; }
    public int StatusCode { get; }
    public string Code { get; } // Optional error code for application-specific errors

    private Error(string message, int statusCode, string code = null)
    {
        Message = message;
        StatusCode = statusCode;
        Code = code;
    }

    // Static factory methods for common error types
    public static Error BadRequest(string message, string code = null) => new Error(message, StatusCodes.Status400BadRequest, code);
    public static Error Unauthorized(string message, string code = null) => new Error(message, StatusCodes.Status401Unauthorized, code);
    public static Error Forbidden(string message, string code = null) => new Error(message, StatusCodes.Status403Forbidden, code);
    public static Error NotFound(string message, string code = null) => new Error(message, StatusCodes.Status404NotFound, code);
    public static Error Conflict(string message, string code = null) => new Error(message, StatusCodes.Status409Conflict, code);
    public static Error InternalServerError(string message, string code = null) => new Error(message, StatusCodes.Status500InternalServerError, code);

    // Custom error method for any status code
    public static Error Custom(string message, int statusCode, string code = null) => new Error(message, statusCode, code);
}
