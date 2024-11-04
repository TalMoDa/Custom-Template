using My.Custom.Template.Services.Implementations;

namespace My.Custom.Template.DependencyInjection;

public static class ServicesDependencyInjection
{
        
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IExampleService, ExampleService>();
            return services;
        }
    
}