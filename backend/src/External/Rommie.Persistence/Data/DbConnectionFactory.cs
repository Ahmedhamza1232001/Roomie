using System.Data.Common;
using Microsoft.Data.SqlClient;
using Rommie.Application.Abstractions;

namespace Rommie.Persistence.Data
{
    public class DbConnectionFactory(string ConnectionString) : IDbConnectionFactory
    {
        public async Task<DbConnection> CreateSqlConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}