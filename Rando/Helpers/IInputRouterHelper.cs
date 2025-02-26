using Rando.Common;

namespace Rando.Helpers;

public interface IInputRouterHelper {

    /// <summary>
    /// Handle validated user input from console to complete command
    /// </summary>
    /// <param name="args"></param>
    Task HandleUserInputAsync(UserInput userInput);

    Task<string> GetMockDataAsync(UserInput userInput);

    Task HandleAdditionalUserInputAsync(UserInput userInput, string result);
}