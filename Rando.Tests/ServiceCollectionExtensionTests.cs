using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rando.Extensions;
using Rando.Helpers;

namespace Rando.Tests;

public class ServiceCollectionExtensionTests
{
    public ServiceCollectionExtensionTests()
    {}

    [Fact]
    public void Configure_ReturnsExpectedServices()
    {
        try
        {
            var host = SetupHost();

            var app = host.Build();

            app.Services.GetRequiredService<IInputEvaluatorHelper>();
            app.Services.GetRequiredService<IInputRouterHelper>();
            app.Services.GetRequiredService<IFileCreatorHelper>();
        }
        catch (Exception)
        {
            Assert.Fail("Service provider did not return expected services!");
        }
    }

    public IHostBuilder SetupHost()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                var config = hostContext.Configuration;
                services.Configure(config);
            });
    }
}