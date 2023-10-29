using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EmployeeApi.Repositories.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionStringSQL;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionStringSQL = _configuration.GetConnectionString("EmployeeConnectionStringSQL");
        }
        public IDbConnection CreateMasterConnection()
            => new SqlConnection(_connectionStringSQL);
    }
}