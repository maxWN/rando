using MySqlConnector;

namespace Rando.Helpers
{
    public class DbFactory : IDbFactory
    {
        public MySqlConnection MySqlConnection { get; set; }

        public DbFactory(string connectionString) {
            if(string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            MySqlConnection = new MySqlConnection(connectionString);
        }
        public MySqlConnection GetMySqlConnection()
        {
            return MySqlConnection;
        }
    }
}
