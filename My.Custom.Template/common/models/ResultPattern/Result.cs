namespace My.Custom.Template.ResultPattern;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public Error Error { get; }
        
    private Result(T value, bool isSuccess, Error error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    // Success factory method
    private static Result<T> Success(T value) => new Result<T>(value, true, null);

    // Common failure types with specific status codes
    private static Result<T> Failure(Error error) => new Result<T>(default, false, error);

        
    // Implicit conversion from T (success value) to Result<T>
    public static implicit operator Result<T>(T value) => Success(value);

    // Implicit conversion from Error to Result<T> (for easy error handling)
    public static implicit operator Result<T>(Error error) => Failure(error);

    public void Deconstruct(out bool isSuccess, out T value, out Error error)
    {
        isSuccess = IsSuccess;
        value = Value;
        error = Error;
    }
}