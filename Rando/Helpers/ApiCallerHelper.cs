using Microsoft.Extensions.Logging;

using Rando.Models;

namespace Rando.Helpers;

public class ApiCallerHelper : IApiCallerHelper<Bank>
{
    private readonly ILogger<ApiCallerHelper> _logger;

    public ApiCallerHelper(ILogger<ApiCallerHelper> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// TODO: build out; make async; refactor interface...
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public Task<IEnumerable<Bank>> GetExternalDataAsync(string uri, string dataType)
    {
        _logger.LogInformation("Entering {Class}.{Method}", base.ToString(), nameof(GetExternalDataAsync));
        throw new NotImplementedException();
    }
}
