﻿using System.Data;
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
        string connectionString = _config.GetConnectionString(connectionStringName);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var rows = await connection.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);

            _lastInsertedId = connection.ExecuteScalar("SELECT LAST_INSERT_ID();");

            connection.Close();

            return rows.ToList();
        };
    }

    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            await connection.ExecuteAsync(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure);

            connection.Close();
        };
    }

    public async Task<ulong> GetLastInsertedId()
    {
        ulong lastInsertedId = _lastInsertedId;
        return await Task.FromResult(lastInsertedId);
    }
}
