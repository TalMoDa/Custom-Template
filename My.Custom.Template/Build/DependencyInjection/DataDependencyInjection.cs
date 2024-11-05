using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using My.Custom.Template.Data;
using My.Custom.Template.Data.Repositories.Implementations;
using My.Custom.Template.Data.Repositories.Interfaces;
using My.Custom.Template.Settings;

namespace My.Custom.Template.Build.DependencyInjection;

public static class DataDependencyInjection
{
    
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        var connectionString = services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStrings>>().Value.DefaultConnection;
        services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddRepositories();
        return services;
    }
    
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
    
}