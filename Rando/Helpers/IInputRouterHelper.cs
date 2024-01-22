using Rando.Common;

namespace Rando;

public interface IInputRouterHelper {

    /// <summary>
    /// Handle validated user input from console to complete command
    /// </summary>
    /// <param name="args"></param>
    void HandleUserInput(UserInput userInput);

    Task<string> GetMockDataAsync(UserInput userInput);
}