
namespace Rando;

public sealed class SqliteDbBuilder : ISqlDbBuilder
{
    public Task<string> BuildDataTable(string dbName, string dbTable)
    {
        throw new NotImplementedException();
    }

    public Task<string> InsertTableData<T>(string dbTable, T data)
    {
        throw new NotImplementedException();
    }
}