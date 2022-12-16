﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Assignment.Repository
{

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly string _connectionStringTransactionDB;
        public readonly string _connectionStringUserDB;

        public GenericRepository(IConfiguration configuration)
        {
            _connectionStringUserDB = configuration.GetConnectionString("UserDBConnection");
        }

        public async Task<IList<T>> GetAllAsync(string query, object? param = null)
        {
            using var connection = new SqlConnection(_connectionStringUserDB);

            var result = await connection.QueryAsync<T>(query, param);

            return result.ToList();
        }

        public async Task<TResult> GetAllSingleAsync<TResult>(string query, object? param = null)
        {
            using var connection = new SqlConnection(_connectionStringUserDB);
            var result = await connection.QueryAsync<TResult>(query, param);

            return result.FirstOrDefault();
        }

        public async Task<int> GetCountAsync(string tableName, string columnName, dynamic columnData)
        {
            using var connection = new SqlConnection(_connectionStringUserDB);
            string query = "select Count(" + columnName + ") from " + tableName + " where " + columnName + " = @ColumnData ";
            var result = await connection.QueryAsync<int>(query, new { ColumnData = columnData });

            return result.FirstOrDefault();
        }

        public async Task<DataTable> GetDataInDataTableAsync(string query, object? selector = null)
        {
            using var connection = new SqlConnection(_connectionStringUserDB);
            //var result = await connection.ExecuteReaderAsync(query, selector);
            DataTable table = new DataTable();
            table.Load(await connection.ExecuteReaderAsync(query, selector));

            return table;
        }

        public async Task<DataSet> GetDataInDataSetAsync(string query, object selector)
        {
            DataSet dsList = new DataSet();

            using (var connection = new SqlConnection(_connectionStringUserDB))
            {
                IDataReader ds = await connection.ExecuteReaderAsync(query, selector);
                dsList = ConvertDataReaderToDataSet(ds);
            }
            return dsList;
        }

        private DataSet ConvertDataReaderToDataSet(IDataReader dataReader)
        {
            DataSet ds = new DataSet();
            int i = 0;
            while (!dataReader.IsClosed)
            {
                ds.Tables.Add("dt" + (i + 1));
                ds.EnforceConstraints = false;

                try
                {
                    ds.Tables[i].Load(dataReader);
                    i++;
                }
                catch (Exception ex)
                {

                }
            }
            return ds;
        }
        public int Execute(string query, SqlConnection con, DbTransaction trn, object? selector = null)
        {
            try
            {
                var affectedRows = con.Execute(query, selector, trn);

                try
                {
                    TransactionHistory(query, JsonConvert.SerializeObject(selector), "", "", "", "");
                }
                catch (Exception ex)
                {
                }

                return affectedRows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ExecuteAsync(string query, SqlConnection con, DbTransaction trn, object? selector = null)
        {
            try
            {
                var affectedRows = await con.ExecuteAsync(query, selector, trn);

                try
                {
                    await TransactionHistoryAsync(query, JsonConvert.SerializeObject(selector), "", "", "", "");
                }
                catch (Exception ex)
                {
                }

                return affectedRows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> ExecuteAsync(string query, object? selector = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionStringUserDB))
                {
                    var affectedRows = await connection.ExecuteAsync(query, selector);

                    try
                    {
                        await TransactionHistoryAsync(query, JsonConvert.SerializeObject(selector), "", "", "", "");
                    }
                    catch (Exception ex)
                    {
                    }

                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int TransactionHistory(string sqlQuery, string queryValue, string userId, string ipAddress, string macAddress, string pcName)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[TransactionHistory]
           ([TransactionType]
           ,[SqlQuery]
           ,[QueryValue]
           ,[Date]
           ,[UserId]
           ,[IPAddress]
           ,[MacAddress]
           ,[PCName])
     VALUES
           (@TransactionType
           ,@SqlQuery
           ,@QueryValue
           ,getdate()
           ,@UserId
           ,@IPAddress
           ,@MacAddress
           ,@PCName)";

                string transactionType = "";
                if (sqlQuery.ToLower().Contains("insert"))
                {
                    transactionType = "insert";
                }
                else if (sqlQuery.ToLower().Contains("update"))
                {
                    transactionType = "update";
                }
                else if (sqlQuery.ToLower().Contains("delete"))
                {
                    transactionType = "delete";
                }

                var selector = new
                {
                    TransactionType = transactionType,
                    SqlQuery = sqlQuery,
                    QueryValue = queryValue,
                    UserId = userId,
                    IPAddress = ipAddress,
                    MacAddress = macAddress,
                    PCName = pcName,
                };

                using (var connection = new SqlConnection(_connectionStringUserDB))
                {
                    var affectedRows = connection.Execute(query, selector);
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<int> TransactionHistoryAsync(string sqlQuery, string queryValue, string userId, string ipAddress, string macAddress, string pcName)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[TransactionHistory]
           ([TransactionType]
           ,[SqlQuery]
           ,[QueryValue]
           ,[Date]
           ,[UserId]
           ,[IPAddress]
           ,[MacAddress]
           ,[PCName])
     VALUES
           (@TransactionType
           ,@SqlQuery
           ,@QueryValue
           ,getdate()
           ,@UserId
           ,@IPAddress
           ,@MacAddress
           ,@PCName)";

                string transactionType = "";
                if (sqlQuery.ToLower().Contains("insert"))
                {
                    transactionType = "insert";
                }
                else if (sqlQuery.ToLower().Contains("update"))
                {
                    transactionType = "update";
                }
                else if (sqlQuery.ToLower().Contains("delete"))
                {
                    transactionType = "delete";
                }

                var selector = new
                {
                    TransactionType = transactionType,
                    SqlQuery = sqlQuery,
                    QueryValue = queryValue,
                    UserId = userId,
                    IPAddress = ipAddress,
                    MacAddress = macAddress,
                    PCName = pcName,
                };

                using (var connection = new SqlConnection(_connectionStringUserDB))
                {
                    var affectedRows = await connection.ExecuteAsync(query, selector);
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
