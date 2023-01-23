using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MyFinanceAppLibrary.DataAccess.Sql;

public class MysqlDataAccess : IDataAccess
{

#nullable disable
    private readonly IConfiguration _config;
#nullable enable

    public MysqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using (IDbConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var rows = await connection.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);

            return rows.ToList();
        };
    }

    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using (IDbConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            await connection.ExecuteAsync(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure);
        };
    }
}
