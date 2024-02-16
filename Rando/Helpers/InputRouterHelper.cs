using Microsoft.Extensions.Logging;
using Rando.Common;
using Kurukuru;
using Rando.Helpers;
using Microsoft.Extensions.Configuration;

namespace Rando;

public class InputRouterHelper : IInputRouterHelper
{
    #region Class Fields

    private readonly ILogger<InputRouterHelper> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IFileCreatorHelper _fileCreatorHelper;
    private readonly IInputEvaluatorHelper _inputEvaluatorHelper;
    private readonly IConfiguration _configuration;
    private readonly ISqlDbBuilder _sqlDbBuilder;

    #endregion Class Fields

    #region Constructor

    public InputRouterHelper(ILogger<InputRouterHelper> logger, IHttpClientFactory httpClientFactory, 
        IFileCreatorHelper fileCreatorHelper, IInputEvaluatorHelper inputEvaluatorHelper, IConfiguration configuration, ISqlDbBuilder sqlDbBuilder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _fileCreatorHelper = fileCreatorHelper ?? throw new ArgumentNullException(nameof(fileCreatorHelper));
        _inputEvaluatorHelper = inputEvaluatorHelper ?? throw new ArgumentNullException(nameof(inputEvaluatorHelper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _sqlDbBuilder = sqlDbBuilder ?? throw new ArgumentNullException(nameof(sqlDbBuilder));
    }

    #endregion Constructor

    public void HandleUserInput(UserInput userInput)
    {
        _logger.LogDebug("Entered {Namespace}.{MethodName}", base.ToString(), nameof(HandleUserInput));
        try
        {
            FilterInput(userInput);
        }
        catch (Exception ex)
        {
            _logger.LogError("The following exception occurred: {ErrorMessage} {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Shows output from API call
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public async Task<string> HandleUserSelectionAsync(UserInput userInput)
    {
        string result = default;

        try
        {
            await Spinner.StartAsync("Loading...", async () =>
            {
                result = await GetMockDataAsync(userInput);
            });
            // TODO: Extract the following logic to new function after implementing DB & API handling logic
            if (!string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(userInput.FlagType))
                HandleAdditionalUserInput(userInput, result);
        }
        catch (Exception ex)
        {
            _logger.LogError("The following exception occurred: {ErrorMessage} {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
        finally
        {
            Console.Beep();
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n{result}\n");
        return result?.ToString();
    }

    public void HandleAdditionalUserInput(UserInput userInput, string result)
    {
        try
        {
            if (userInput.FlagType.Equals(FlagType.FileFlag))
            {
                _fileCreatorHelper.CreateFile(userInput.FilePath, result, userInput.FileName);
            }
            else if (userInput.FlagType.Equals(FlagType.DatabaseFlag))
            {                
                _sqlDbBuilder.BuildDataTable(userInput.TableName, userInput);
            }
            else if (userInput.FlagType.Equals(FlagType.ApiFlag))
            {                
                //_apiCallerHelper.SendData(null, userInput);
            }
            else
            {
                throw new ArgumentException();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("The following exception occurred: {ErrorMessage} {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    /// <summary>
    /// Makes request to API to fetch mock data
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public async Task<string> GetMockDataAsync(UserInput userInput)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("randomDataApi");
            var response = await httpClient.GetAsync(requestUri: $"{userInput.DataType.ToLowerInvariant()}?size={userInput.Quantity}&is_xml=true");
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError("Random API request failed: {ErrorMessage} {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
        return default;
    }

    /// <summary>
    /// Filters user input by first argument
    /// </summary>
    /// <param name="args"></param>
    public void FilterInput(UserInput userInput)
    {
        switch (userInput.DataType)
        {
            case DataType.USERS:
                Console.WriteLine("User type choosen.\n");
                // probably need to use async here instead of Wait():
                // https://olegignat.com/task-wait-or-await-task/
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.BANKS:
                Console.WriteLine("Bank type choosen.\n");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.APPLIANCES:
                Console.WriteLine("Appliance type choosen.\n");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.CREDIT_CARDS:
                Console.WriteLine("Credit type choosen.\n");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.ADDRESSES:
                Console.WriteLine("Addresses type choosen.\n");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.BLOOD_TYPES:
                Console.WriteLine("Blood type choosen.\n");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.BEERS:
                Console.WriteLine("Beer type choosen.\n");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            default:
                Console.WriteLine($"{AppConstants.USER_DIRECTIONS}");
                break;
        }
    }
}