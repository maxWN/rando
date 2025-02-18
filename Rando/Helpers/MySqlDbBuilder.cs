using Microsoft.Extensions.Logging;
using Dapper;
using MySqlConnector;
using Rando.Scripts;
using Microsoft.Extensions.Configuration;
using Rando.Common;

namespace Rando.Helpers;

public class MySqlDbBuilder : ISqlDbBuilder
{
    private readonly ILogger<MySqlDbBuilder> _logger;
    private readonly IDbFactory _dbFactory;
    private readonly IConfiguration _configuration;

    public MySqlDbBuilder(ILogger<MySqlDbBuilder> logger, IDbFactory dbFactory, IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async void BuildDataTable(string dbTable, UserInput userInput)
    {
        MySqlConnection connection;
        try
        {
            using (connection = _dbFactory.GetMySqlConnection())
            {
                connection.Open();

                // TODO: Implement new helper method here that switches between SQL queries
                // based on user input...
                var sqlNonQuery = string.Format(MySqlQueries.CREATE_BANKS_TABLE, _configuration["DatabaseConfiguration:Database"],
                    (string.IsNullOrWhiteSpace(dbTable) ? userInput.DataType.ToLower() : dbTable));

                MySqlCommand mySqlCommand = new(sqlNonQuery, connection);
                
                MySqlDataReader rdr = await mySqlCommand.ExecuteReaderAsync();

                connection.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "The following exception occurred: {ErrorMessage} {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    public Task<string> InsertTableData<T>(string dbTable, UserInput userInput, List<T> data)
    {
        throw new NotImplementedException();
    }
}
