using System.Collections.Generic;

namespace My.Custom.Template.ResultPattern.Extensions
{
    public static class ResultExtensions
    {
        public static Result<T> WithErrors<T>(this Result<T> result, List<Error> errors)
        {
            result.Errors.AddRange(errors);
            return result;
        }
    }
}