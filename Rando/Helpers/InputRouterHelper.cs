using Microsoft.Extensions.Logging;
using Rando.Common;
using Kurukuru;
using Rando.Helpers;

namespace Rando;

public class InputRouterHelper : IInputRouterHelper
{
    #region Class Fields

    private readonly ILogger<InputRouterHelper> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IFileCreatorHelper _fileCreatorHelper;
    private readonly IInputEvaluatorHelper _inputEvaluatorHelper;

    #endregion Class Fields

    #region Constructor

    public InputRouterHelper(ILogger<InputRouterHelper> logger, IHttpClientFactory httpClientFactory, IFileCreatorHelper fileCreatorHelper, IInputEvaluatorHelper inputEvaluatorHelper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _fileCreatorHelper = fileCreatorHelper ?? throw new ArgumentNullException(nameof(fileCreatorHelper));
        _inputEvaluatorHelper = inputEvaluatorHelper ?? throw new ArgumentNullException(nameof(inputEvaluatorHelper));
    }

    #endregion Constructor

    public void ReadInput(UserInput userInput)
    {
        _logger.LogDebug("Entered {Namespace}.{MethodName}", base.ToString(), nameof(ReadInput));

        FilterInput(userInput);
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
            var apiTask = GetMockDataAsync(userInput);
            await Spinner.StartAsync("Loading...", async () =>
            {
                result = await apiTask;
            });
            // TODO: Extract the following logic to new function when implementing DB & API handling logic
            if (!string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(userInput.FlagType)
                && userInput.FlagType.Equals(FlagType.FileFlag))
            {
                _fileCreatorHelper.CreateFile(userInput.FilePath, result, userInput.FileName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("The following exception occurred: {Exception}", ex.Message.ToString());
            throw;
        }
        finally
        {
            Console.Beep();
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n{result}\n");
        return result.ToString();
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
            _logger.LogError("Random API request failed: {Exception}", ex.Message.ToString());
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
                Console.WriteLine("User choosen");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.BANKS:
                Console.WriteLine("Bank choosen.");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.APPLIANCES:
                Console.WriteLine("Appliance choosen.");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.CREDIT_CARDS:
                Console.WriteLine("Credit choosen.");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.ADDRESSES:
                Console.WriteLine("Addresses choosen.");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.BLOOD_TYPES:
                Console.WriteLine("Blood Types choosen.");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            case DataType.BEERS:
                Console.WriteLine("Beers choosen.");
                HandleUserSelectionAsync(userInput).Wait();
                break;

            default:
                Console.WriteLine($"{AppConstants.USER_DIRECTIONS}");
                break;
        }
    }
}