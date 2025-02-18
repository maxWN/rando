using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using Rando.Common;
using Rando.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySqlConnector;
using System.Threading.Tasks;
using Rando.Extensions;

namespace Rando;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        try
        {
            // var config = new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json", optional: false)
            //     .Build();

            var serviceProvider = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
                {
                    var config = hostContext.Configuration;
                    services.Configure(config);
            //         services.AddHttpClient("randomDataApi", httpClient =>
            //         {
            //             httpClient.BaseAddress = new Uri($"{AppConstants.RANDOM_DATA_API_BASE_URL}");
            //             httpClient.Timeout = TimeSpan.FromSeconds(10);
            //         });
            //         services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
            //         services.AddTransient<IInputRouterHelper, InputRouterHelper>();
            //         services.AddTransient<IFileCreatorHelper, FileCreatorHelper>();
            //         services.AddTransient<IInputEvaluatorHelper, InputEvaluatorHelper>();
            //         // ArgumentException.ThrowIfNullOrEmpty(config["DatabaseConfiguration:Dialect"]);
            //         // if (config != null && config["DatabaseConfiguration:Dialect"].ToString().Equals("MySQL"))
            //         // {
            //         //     var connStr = config["DatabaseConfiguration:ConnectionString"] ?? throw new ArgumentNullException();
            //         //     services.AddTransient<IDbFactory>(_ => new DbFactory(connStr));
            //         //     services.AddTransient<ISqlDbBuilder, MySqlDbBuilder>();
            //         // }
                })
                .ConfigureAppConfiguration(options => options.AddJsonFile("appsettings.json"))
                .ConfigureLogging(options => options.AddConsole())
                .UseConsoleLifetime();

            var host = serviceProvider.Build();
#pragma warning disable CS8604 // Possible null reference argument.
            ILogger<Program> logger = host.Services.GetService<ILoggerFactory>().CreateLogger<Program>();
#pragma warning restore CS8604 // Possible null reference argument.

            logger.LogDebug("Starting application");

            await ExecuteProgramAsync(host, args, logger);

            logger.LogDebug("Command successfully executed. Application will shutdown now.");
        }
        catch (Exception)
        {
            return (int)AppEnums.EXIT_CODES.ERROR_BAD_COMMAND;
        }

        return (int)AppEnums.EXIT_CODES.SUCCESS;
    }

    /// <summary>
    /// Prompt user for input
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="args"></param>
    /// <param name="logger"></param>
    private static async Task ExecuteProgramAsync(IHost serviceProvider, string[] args, ILogger<Program> logger)
    {
        try
        {
            var inputEvaluatorHelper = serviceProvider.Services.GetService<IInputEvaluatorHelper>();
            var inputRouterHelper = serviceProvider.Services.GetService<IInputRouterHelper>();
            var userInput = inputEvaluatorHelper?.GetUserInputObject(args) ?? throw new ArgumentNullException();
            await inputRouterHelper?.HandleUserInputAsync(userInput: userInput);
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Read and/or write operations failed: {Exception}.\nApplication will shutdown now.", ex.Message);
            throw;
        }
    }
}