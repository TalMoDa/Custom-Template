using My.Custom.Template.Settings;

namespace My.Custom.Template.DependencyInjection;

public static class SettingsDependencyInjection 
{
    
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddOptions<ConnectionStrings>().BindConfiguration(nameof(ConnectionStrings)).ValidateDataAnnotations();
        return services;
    }
    
}