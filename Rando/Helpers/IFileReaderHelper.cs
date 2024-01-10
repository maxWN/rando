using Rando.Common;

namespace Rando;

public interface IFileReaderHelper {

    /// <summary>
    /// Read input from console to begin merging files
    /// </summary>
    /// <param name="args"></param>
    void ReadInput(UserInput userInput);

    Task<string> GetMockDataAsync(UserInput userInput);
}