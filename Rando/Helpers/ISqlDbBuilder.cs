using Rando.Common;
using System.Data.Common;

namespace Rando;

public interface ISqlDbBuilder 
{
    // TODO: Merge these two methods
    void BuildDataTable(string dbTable, UserInput userInput);

    Task<string> InsertTableData<T>(string dbTable, UserInput userInput, List<T> data);

}