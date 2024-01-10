namespace Rando;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Rando.Common;
using Kurukuru;
using Rando.Helpers;

public class FileReaderHelper: IFileReaderHelper {

    #region Class Fields

    private readonly ILogger<FileReaderHelper> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IFileCreatorHelper _fileCreatorHelper;
    private readonly IInputEvaluator inputEvaluator;

    #endregion Class Fields

    private Dictionary<string, string> UserInputDictionary = new Dictionary<string, string>();

    #region Constructor

    public FileReaderHelper(ILogger<FileReaderHelper> logger, IHttpClientFactory httpClientFactory, IFileCreatorHelper fileCreatorHelper, IInputEvaluator inputEvaluator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _fileCreatorHelper = fileCreatorHelper ?? throw new ArgumentNullException(nameof(fileCreatorHelper));
        this.inputEvaluator = inputEvaluator ?? throw new ArgumentNullException(nameof(inputEvaluator));
    }

    #endregion Constructor

    public void ReadInput(UserInput userInput)
    {
        // TODO: Try this approach to logging?
        // https://www.youtube.com/shorts/PvQGVmozCdU
        // In addition... need to rely on LoggerMessage
        // method/library?
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging
        // https://www.youtube.com/watch?v=bnVfrd3lRv8
        _logger.LogInformation("Entered {Namespace}.{MethodName}", base.ToString(), nameof(ReadInput));

        // if (!IsUserInputValid(ref userInput))
        // {
        //     RetryInput();
        //     return;
        // }

        Console.WriteLine("Prompted.\n");
        FilterInput(userInput);
    }

    /// <summary>
    /// Prompts user to re-enter data after an invalid or erroneous situation
    /// </summary>
    /// <param name="args"></param>
    // public void RetryInput() {
    //     Console.WriteLine(AppConstants.USER_DIRECTIONS);
    //     var input = Console.ReadLine();            
    //     ReadInput(input?.Split(" "));
    // }

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
            // await Task.Delay(10000);
            return data;
        }
        catch (Exception ex) {
            _logger.LogError("Request failed : {Exception}", ex.ToString());
            throw;
        }
        return default;
    }

    /// <summary>
    /// Determines if user input is valid
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private bool IsUserInputValid(ref string[]? args) {

        bool isValid = true;

        if (args == null || args?.Length == 0)
        {
            return !isValid;
        }
        if ((args?.Length) <= 1) {
            Array.Resize(ref args, args.Length+1);
            args[1] = "1";
        }
        if (string.IsNullOrWhiteSpace(args?[0]))
        {
            Console.WriteLine($"{AppConstants.USER_DIRECTIONS}");
            return !isValid;
        }
        if (args[1].Any(x => !char.IsDigit(x))) {
            Console.WriteLine($"{AppConstants.USER_DIRECTIONS}");
            return !isValid;
        }
        // begin evaluating flags and their accompanied args
        if (args.Length > 2) {
            // TODO: create new method to explicitly verify flags and options
            return IsFlagValid(args) ? !isValid : isValid; 
        }

        return isValid;
    }

    /// <summary>
    /// Evaluates flags in argument to determine if they are valid
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private bool IsFlagValid(string [] args) {
        return (!args[2].Equals("--file-output") && !args[2].Equals("--db-output"));
    }

    /// <summary>
    /// Filters user input by first argument
    /// </summary>
    /// <param name="args"></param>
    public void FilterInput(UserInput userInput) {

        // int quantity = Convert.ToInt32(args[1]);

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