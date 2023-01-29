using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MyFinanceAppLibrary.Models;
using MySql.Data.MySqlClient;

namespace MyFinanceAppLibrary.DataAccess.Sql;

public class MysqlDataAccess : IDataAccess
{

#nullable disable
    private readonly IConfiguration _config;
    private dynamic _lastInsertedId;
#nullable enable

    public MysqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
    {
        try
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var rows = await connection.QueryAsync<T>(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure);

                    _lastInsertedId = connection.ExecuteScalar("SELECT LAST_INSERT_ID();");

                    return rows.ToList();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Connection exception: " + ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }

            };

        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occurred: " + ex.Message);
            throw;
        }
    }

    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
    {
        try
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    await connection.ExecuteAsync(
                            storedProcedure,
                            parameters,
                            commandType: CommandType.StoredProcedure);

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Connection exception: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection exception: " + ex.Message);
            throw;
        }
    }

    public async Task<ulong> GetLastInsertedId()
    {
        ulong lastInsertedId = _lastInsertedId;
        return await Task.FromResult(lastInsertedId);
    }
}
