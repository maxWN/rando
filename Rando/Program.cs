namespace Rando;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using Rando.Common;
using Rando.Helpers;

public class Program
{
    public static int Main(string[] args)
    {
#pragma warning disable warning-list
        try
        {
            var serviceProvider = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient("randomDataApi", httpClient =>
                    {
                        httpClient.BaseAddress = new Uri($"{AppConstants.RANDOM_DATA_API_BASE_URL}");
                    });
                    services.AddTransient<IInputRouterHelper, InputRouterHelper>();
                    services.AddTransient<IFileCreatorHelper, FileCreatorHelper>();
                    services.AddTransient<IInputEvaluatorHelper, InputEvaluatorHelper>();
                })
                .UseConsoleLifetime();

            var host = serviceProvider.Build();
            var logger = host.Services.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");

            ExecuteProgram(host, args, logger);

            logger.LogDebug("Command successfully executed. Application will shutdown now.");
        }
        catch (Exception)
        {
            return (int)AppEnums.EXIT_CODES.ERROR_BAD_COMMAND;
        }

        return (int)AppEnums.EXIT_CODES.SUCCESS;
#pragma warning restore warning-list
    }

    /// <summary>
    /// Prompt user for input
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    private static void ExecuteProgram(IHost serviceProvider, string[] args, ILogger<Program> logger)
    {
        try
        {
            var _inputEvaluatorHelper = serviceProvider.Services.GetService<IInputEvaluatorHelper>();
            var fileReaderHelper = serviceProvider.Services.GetService<IInputRouterHelper>();
            var userInput = _inputEvaluatorHelper?.GetUserInputObject(args);
            fileReaderHelper?.ReadInput(userInput);
        }
        catch (Exception)
        {
            // logger.LogDebug("Read and/or write operations failed: {Exception}.\nApplication will shutdown now.", Ex);
            // Environment.Exit(1);
            throw;
        }
    }
}