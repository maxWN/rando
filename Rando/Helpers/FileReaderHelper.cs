namespace Rando;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Rando.Common;
using Kurukuru;
using Rando.Helpers;

public class FileReaderHelper: IFileReaderHelper {

    #region Class Fields

    private readonly ILogger<FileReaderHelper> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IFileCreatorHelper _fileCreatorHelper;
    private readonly IInputEvaluatorHelper _inputEvaluatorHelper;

    #endregion Class Fields

    private Dictionary<string, string> UserInputDictionary = new Dictionary<string, string>();

    #region Constructor

    public FileReaderHelper(ILogger<FileReaderHelper> logger, IHttpClientFactory httpClientFactory, IFileCreatorHelper fileCreatorHelper, IInputEvaluatorHelper inputEvaluatorHelper)
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
    public async Task<string> HandleUserSelectionAsync(UserInput userInput) {

        var apiTask = GetMockDataAsync(userInput);
        string result = default;

        try {
            await Spinner.StartAsync("Loading...", async () => {
                result = await apiTask;
            });
            if (!string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(userInput.FlagType)
                && userInput.FlagType.Equals(FlagType.FileFlag)) {
                _fileCreatorHelper.CreateFile(userInput.FilePath, result, userInput.FileName);
            }
        }
        finally {
            Console.Beep();
        }
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{result}");
        return result.ToString();
    }

    /// <summary>
    /// Makes request to API to fetch mock data
    /// </summary>
    /// <param name="dataType"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public async Task<string> GetMockDataAsync(UserInput userInput) {
        var httpClient = _httpClientFactory.CreateClient("randomDataApi");

        try {
            var response = await httpClient.GetAsync(requestUri: $"{userInput.DataType.ToLowerInvariant()}?size={userInput.Quantity}&is_xml=true");
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        catch (Exception ex) {
            _logger.LogError("Request failed : {Exception}", ex.ToString());
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
        // TODO: strongly consider splitting out remaining args into a new str []
        // pass remaining args into the handle method, so you can reduce total number of args in method def

        switch(userInput.DataType) {
            case DataType.USERS:
                Console.WriteLine("User choosen");
                this.HandleUserSelectionAsync(userInput).Wait();
                break;
            case DataType.BANKS:
                Console.WriteLine("Bank choosen.");
                this.HandleUserSelectionAsync(userInput).Wait();
                break;
            case DataType.APPLIANCES: 
                Console.WriteLine("Appliance choosen.");
                this.HandleUserSelectionAsync(userInput).Wait();                    
                break;
            case DataType.CREDIT_CARDS:
                Console.WriteLine("Credit choosen.");
                this.HandleUserSelectionAsync(userInput).Wait();
                break;
            case DataType.ADDRESSES:
                Console.WriteLine("Addresses choosen.");
                this.HandleUserSelectionAsync(userInput).Wait();
                break;
            default:
                Console.WriteLine($"{AppConstants.USER_DIRECTIONS}");
                break;

        }
    }
}