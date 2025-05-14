
using Microsoft.Data.SqlClient;

using System.Data;

using Microsoft.Extensions.Configuration;


namespace GarageInfrastructure.Data
{
    public class DapperDbContext
    {
        private readonly string _connectionString;

        public DapperDbContext(IConfiguration configuration)
        {
           
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
