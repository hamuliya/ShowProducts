﻿using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;

namespace DataAccess.DbAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<T>> LoadData<T, U>(string storeProcedure,U parameters,string connectionId = "Default")
    {
        using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        return await connection.QueryAsync<T>(storeProcedure, parameters,
            commandType: CommandType.StoredProcedure);
    }




    public int SaveData<T>(string StoreProcedure, T Parameters, string connectionId = "Default")
    {
        int id = 0;

        using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId)))
        {
            var result = connection.ExecuteScalar(StoreProcedure, Parameters, commandType: CommandType.StoredProcedure);

            bool IsVaildInt = int.TryParse(result.ToString(), out id);
            if (IsVaildInt == false)
            {
                throw new Exception("Return is not Int");
            }
        }
        return id;
    }



    public async Task ExecData<T>(string storeProcedure,T parameters,string connectionId = "Default")
    {
        using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId)))
        {
            await connection.ExecuteAsync(storeProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }

}
