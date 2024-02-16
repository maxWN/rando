

using Rando.Common;

namespace Rando;

public sealed class SqliteDbBuilder : ISqlDbBuilder
{
    public SqliteDbBuilder()
    { }

    public void BuildDataTable(string dbTable, UserInput userInput)
    {
        throw new NotImplementedException();
    }

    public Task<string> InsertTableData<T>(string dbTable, UserInput userInput, List<T> data)
    {
        throw new NotImplementedException();
    }
}