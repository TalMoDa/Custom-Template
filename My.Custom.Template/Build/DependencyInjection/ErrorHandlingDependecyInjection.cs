using Microsoft.AspNetCore.Mvc.Infrastructure;
using My.Custom.Template.Factories;

namespace My.Custom.Template.Build.DependencyInjection;

public static class ErrorHandlingDependencyInjection
{
    
    public static IServiceCollection UseProblemDetails(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
        return services;
    }

    
}