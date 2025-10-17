using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;


namespace MinibusTracker.Infrastructure.Persistence.Common;
public class MySqlConnectionFactory : IDBConnectionFactory
{
    private readonly string _connectionString;

    public MySqlConnectionFactory(IConfiguration config)
    {
        this._connectionString = config.GetConnectionString("TaxiDb");
    }

    public IDbConnection create()
    {
        return new MySqlConnection(_connectionString);
    }
} 

