using System.Data.Common;

namespace Rando;

public interface ISqlDbBuilder {

    Task<string> BuildDataTable(string dbName, string dbTable);

    Task<string> InsertTableData<T>(string dbTable, T data);

}