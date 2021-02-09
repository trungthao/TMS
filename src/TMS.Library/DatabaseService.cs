using System.Collections.Specialized;
using System.Text;
using System.Data;
using System;
using System.Collections.Generic;
using TMS.Library.Interfaces;
using MySql.Data.MySqlClient;
using Dapper;
using System.Threading.Tasks;

namespace TMS.Library
{
    public class DatabaseService : IDatabaseService
    {
        private string _connectionString = null;
        private IConfigService _configService;

        public DatabaseService(IConfigService configService)
        {
            _configService = configService;
        }

        public string GetConnectionString()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _connectionString = _configService.GetConnectionStrings();
            }

            return _connectionString;
        }

        public async Task<IEnumerable<T>> GetListObjectAsync<T>(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null)
        {
            if (conn != null)
            {
                return await conn.QueryAsync<T>(sql, param, transaction, timeout, commandType);
            }
            else
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    return await conn.QueryAsync<T>(sql, param, transaction, timeout, commandType);
                }
            }
        }

        public async Task<T> GetObjectAsync<T>(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null)
        {
            if (conn != null)
            {
                return await conn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, timeout, commandType);
            }
            else
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    return await conn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, timeout, commandType);
                }
            }
        }

        public async Task<T> ExecuteScalaAsync<T>(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null)
        {
            if (conn != null)
            {
                return await conn.ExecuteScalarAsync<T>(sql, param, transaction, timeout, commandType);
            }
            else
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    return await conn.ExecuteScalarAsync<T>(sql, param, transaction, timeout, commandType);
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null)
        {
            if (conn != null)
            {
                return await conn.ExecuteAsync(sql, param, transaction, timeout, commandType);
            }
            else
            {
                using (conn = new MySqlConnection(GetConnectionString()))
                {
                    return await conn.ExecuteAsync(sql, param, transaction, timeout, commandType);
                }
            }
        }
    }
}
