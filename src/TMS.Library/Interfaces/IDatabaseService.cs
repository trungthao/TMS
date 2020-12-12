using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace TMS.Library.Interfaces
{
    public interface IDatabaseService
    {
        Task<T> ExecuteScalaAsync<T>(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null);

        Task<int> ExecuteNonQueryAsync(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null);

        Task<IEnumerable<T>> GetListObjectAsync<T>(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null);

        Task<T> GetObjectAsync<T>(string sql, object param = null, IDbConnection conn = null, IDbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure, int? timeout = null);
    }
}