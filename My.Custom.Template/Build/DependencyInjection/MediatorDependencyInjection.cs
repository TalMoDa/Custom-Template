using System.Reflection;
using FluentValidation;
using MediatR;
using My.Custom.Template.Pipelines;


namespace My.Custom.Template.DependencyInjection;

public static class MediatorDependencyInjection
{
    
    public static IServiceCollection AddAppMediatR(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<Program>();
        });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}