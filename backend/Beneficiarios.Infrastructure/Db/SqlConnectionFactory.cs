using Microsoft.Data.SqlClient;
using System.Data;

namespace Beneficiarios.Infrastructure.Db;

public class SqlConnectionFactory : IDisposable
{
    private readonly string _connectionString;
    private SqlConnection? _connection;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }
        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}