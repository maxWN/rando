using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using Rando.Common;
using Rando.Helpers;

public class InputEvaluator : IInputEvaluator {
    private readonly ILogger<InputEvaluator> logger;

    public InputEvaluator(ILogger<InputEvaluator> logger) {
        this.logger = logger;
    }

    /// <summary>
    /// Returns formatted class object containing named properties representing args
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public UserInput GetFormattedUserInput(ref string[]? args)
    {
        if (args == null || args?.Length == 0)
        {
            Console.WriteLine(AppConstants.USER_DIRECTIONS + "\n");
            throw new ArgumentException("Invalid arguments supplied");
        }
        UserInput userInput = new();

        try {
            userInput.DataType = IsArgPresent(args, 0);
            userInput.Quantity = Convert.ToInt32(IsArgPresent(args, 1));
            userInput.FlagType = IsArgPresent(args, 2);

            EvaluateRequiredProperties(ref userInput);

            if (!string.IsNullOrEmpty(userInput.FlagType)) {
                SetFlagProperties(args, ref userInput);
                EvaluateFlagProperties(ref userInput);
            }

            //var validationResults = IsUserInputValid(userInput);
        } 
        catch (Exception) {
            throw;    
        }

        return userInput;
    }

    /// <summary>
    /// Evaluates if args passed in are valid
    /// </summary>
    /// <param name="userInput"></param>
    /// <returns></returns>
    private List<ValidationResult> IsUserInputValid(UserInput userInput) {

        List<ValidationResult> validationResults = new List<ValidationResult>();

        Validator.TryValidateObject(userInput, new ValidationContext(userInput, null, null), validationResults, true);

        return validationResults;
    }

    private string IsArgPresent(string[]? args, int index)
    {
        if ((args?.Length - 1) < index) {
            return "";
        }
        return args[index];
    }

    private void SetFlagProperties(string[]? args, ref UserInput userInput) {
        switch(args[2]) {
            case FlagType.FileFlag:
                userInput.FilePath = IsArgPresent(args, 3);
                userInput.FileName = IsArgPresent(args, 4);
                break;
            case FlagType.DatabaseFlag:
                userInput.DatabaseName = IsArgPresent(args, 3);            
                break;
            case FlagType.ApiFlag:
                userInput.ApiUrl = IsArgPresent(args, 3);     
                break;
            default:
                break;
        }
    }

    private void EvaluateRequiredProperties(ref UserInput userInput) 
    {
        List<ValidationResult> QuantityValidationResults = new();
        List<ValidationResult> dataTypeValidationResults = new();
        try {

            bool isDataTypeValid = Validator.TryValidateProperty(userInput.DataType, new ValidationContext(userInput, null, null) { MemberName = "DataType"}, dataTypeValidationResults);
            bool isQuantityValid = Validator.TryValidateProperty(userInput.Quantity, new ValidationContext(userInput, null, null) { MemberName = "Quantity"}, QuantityValidationResults);

            if (!isDataTypeValid || !isQuantityValid) {
                QuantityValidationResults.AddRange(dataTypeValidationResults);
                PrintArgViolations(QuantityValidationResults);
            }
        } catch(Exception) {
            throw;
        }
    }

    private void EvaluateFlagProperties(ref UserInput userInput) 
    {
        if (userInput.FlagType.Equals(FlagType.FileFlag)) {
            List<ValidationResult> filePathValidationResults = new();
            List<ValidationResult> fileNameValidationResults = new();
            bool isFilePathValid = Validator.TryValidateProperty(userInput.FilePath, new ValidationContext(userInput, null, null) { MemberName = "FilePath"}, filePathValidationResults);
            bool isFileNameValid = Validator.TryValidateProperty(userInput.FileName, new ValidationContext(userInput, null, null) { MemberName = "FileName"}, fileNameValidationResults);
            if (!isFileNameValid || !isFilePathValid) {
                fileNameValidationResults.AddRange(filePathValidationResults);
                PrintArgViolations(fileNameValidationResults);
            }
        } else if (userInput.FlagType.Equals(FlagType.ApiFlag)) {
            List<ValidationResult> apiUrlValidationResults = new();
            bool isApiUrlValid = Validator.TryValidateProperty(userInput.ApiUrl, new ValidationContext(userInput, null, null) { MemberName = "ApiUrl"}, apiUrlValidationResults);
            if (!isApiUrlValid) {
                PrintArgViolations(apiUrlValidationResults);
            }
        } else if (userInput.FlagType.Equals(FlagType.DatabaseFlag)) {
            List<ValidationResult> databaseNameValidationResults = new();
            bool isDatabaseNameValid = Validator.TryValidateProperty(userInput.DatabaseName, new ValidationContext(userInput, null, null) { MemberName = "DatabaseName"}, databaseNameValidationResults);
            if (!isDatabaseNameValid) {
                PrintArgViolations(databaseNameValidationResults);
            }
        }
    }

    private void PrintArgViolations(List<ValidationResult> validationResults) 
    {
        if (validationResults.Count > 0) {
            Console.WriteLine(AppConstants.USER_DIRECTIONS + "\n");
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var result in validationResults) {
                Console.WriteLine($"*\t {result}");
            }
            throw new ArgumentException("Invalid arguments supplied");
        }        
    }

}
