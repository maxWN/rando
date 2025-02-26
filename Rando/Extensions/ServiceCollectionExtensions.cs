using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Rando.Common;
using Rando.Helpers;

namespace Rando.Extensions;

public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Configures application service collection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection Configure(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddHttpClient("randomDataApi", httpClient =>
        {
            httpClient.BaseAddress = new Uri($"{AppConstants.RANDOM_DATA_API_BASE_URL}");
        });
        services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
        services.AddScoped<IInputRouterHelper, InputRouterHelper>();
        services.AddScoped<IFileCreatorHelper, FileCreatorHelper>();
        services.AddScoped<IInputEvaluatorHelper, InputEvaluatorHelper>();
        services.ConfigureSqlDatabase(configuration);
        return services;
    }

    public static IServiceCollection ConfigureSqlDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration["DatabaseConfiguration:Dialect"].ToString().Equals("MySQL"))
        {
            services.AddTransient<IDbFactory>(_ => new DbFactory(configuration["DatabaseConfiguration:ConnectionString"] ?? throw new ArgumentNullException()));
            services.AddTransient<ISqlDbBuilder, MySqlDbBuilder>();
        }
        return services;
    }

}