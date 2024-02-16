using MySqlConnector;

namespace Rando.Helpers
{
    public interface IDbFactory
    {
        MySqlConnection GetMySqlConnection();
    }
}