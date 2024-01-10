namespace Rando;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System;
using Rando.Common;
using Rando.Helpers;

public class Program
{
    // need to setup program.cs like this, so httpclient factory can be used
    // https://www.zoneofdevelopment.com/2021/08/25/c-use-ihttpclientfactory-in-a-console-app/
    public static int Main(string[] args)
    {
        try {
            //setup our DI and configure console logging
            var serviceProvider = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
                {
                    // with AddHttpClient we register the IHttpClientFactory
                    services.AddHttpClient("randomDataApi", httpClient => {
                        httpClient.BaseAddress = new Uri($"{AppConstants.RANDOM_DATA_API_BASE_URL}");
                    });
                    // here, we register the dependency injection  
                    services.AddTransient<IFileReaderHelper, FileReaderHelper>();
                    services.AddTransient<IFileCreatorHelper, FileCreatorHelper>();
                    services.AddTransient<IInputEvaluator, InputEvaluator>();
                })
                .UseConsoleLifetime();

            var host = serviceProvider.Build();
            var logger = host.Services.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");

            // need to find better way to update .csproj
            // https://stackoverflow.com/questions/72134225/console-logging-in-net-core-6
            
            ExecuteProgram(host, args, logger);

            logger.LogDebug("File successfully written. Application will shutdown now.");
        } 
        catch (Exception ex) {
            return (int)AppEnums.EXIT_CODES.ERROR_BAD_COMMAND;
        }

        return (int)AppEnums.EXIT_CODES.SUCCESS;
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
            var inputEvaluator = serviceProvider.Services.GetService<IInputEvaluator>();
            var fileReaderHelper = serviceProvider.Services.GetService<IFileReaderHelper>();
            // string combinedArgs = default;
            // if (args != null) {
            //     combinedArgs = string.Join("", args);
            // }
            var userInput = inputEvaluator?.GetFormattedUserInput(ref args);
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
