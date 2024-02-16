using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
