

namespace My.Custom.Template.Common.Models.ResultPattern;

public interface IResult
{
    bool IsSuccess { get; }
    List<Error> Errors { get; set; }
}

public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public List<Error> Errors { get; set; } = new List<Error>();

    private Result(T value, bool isSuccess, List<Error> errors)
    {
        Value = value;
        IsSuccess = isSuccess;
        Errors = errors ?? new List<Error>();
    }

    // Success factory method
    public static Result<T> Success(T value) => new Result<T>(value, true, null);

    // Failure factory method
    public static Result<T> Failure(List<Error> errors) => new Result<T>(default, false, errors);

    // Specific error methods using predefined Error types

    // Additional methods for other error types...
    // ...

    // Implicit conversion from T (success value) to Result<T>
    public static implicit operator Result<T>(T value) => Success(value);

    // Implicit conversion from Error to Result<T> (for easy error handling)
    public static implicit operator Result<T>(Error error) => Failure(new List<Error> { error });
    
    public static implicit operator Result<T>(List<Error> errors) => Failure(errors);

    public void Deconstruct(out bool isSuccess, out T value, out List<Error> errors)
    {
        isSuccess = IsSuccess;
        value = Value;
        errors = Errors;
    }
}
