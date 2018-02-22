using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Gapper.Helpers;

namespace Gapper
{
    public class DapperAsyncService
    {
        private readonly string _connectionString;

        public DapperAsyncService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Runs an INSERT query.
        /// </summary>
        /// <typeparam name="T">Object that should be updated in database.</typeparam>
        /// <param name="parameters">Insert parameters</param>
        /// <returns>ObjectId</returns>
        protected async Task<int> InsertAsync<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<int>(
                    sql: StatementHelper.GenerateInsertStatement<T>(parameters),
                    param: parameters,
                    commandType: CommandType.Text).ConfigureAwait(false);

                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Runs an UPDATE query.
        /// </summary>
        /// <typeparam name="T">Object that should be updated in database.</typeparam>
        /// <param name="parameters">Udpate parameters, plus Id of object.</param>
        /// <returns></returns>
        protected async Task UpdateAsync<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(
                    sql: StatementHelper.GenerateUpdateStatement<T>(parameters),
                    param: parameters,
                    commandType: CommandType.Text).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs a SELECT query.
        /// </summary>
        /// <typeparam name="T">Object that should be selected from in database.</typeparam>
        /// <param name="parameters">Where parameters.</param>
        /// <returns>Requested object.</returns>
        protected async Task<List<T>> SelectAsync<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<T>(
                    sql: StatementHelper.GenerateSelectStatement<T>(parameters),
                    param: parameters,
                    commandType: CommandType.Text).ConfigureAwait(false);                

                return result.ToList();
            }           
        }

        /// <summary>
        /// Runs a DELETE query.
        /// </summary>
        /// <typeparam name="T">Object that should be deleted from in database.</typeparam>
        /// <param name="parameters">Where parameters.</param>
        /// <returns>Void</returns>
        protected async Task DeleteAsync<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                await conn.ExecuteAsync(
                    sql: StatementHelper.GenerateDeleteStatement<T>(parameters),
                    param: parameters,
                    commandType: CommandType.Text).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get the SQL connection.
        /// </summary>
        /// <returns>SqlConnection</returns>
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Get table name from type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Table name as string.</returns>
        protected static string GetTableName<T>()
        {
            return StatementHelper.GenerateTableName<T>();
        }
    }
}

