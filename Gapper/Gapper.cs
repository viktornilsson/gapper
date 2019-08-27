using Dapper;
using Gapper.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Gapper
{
    public static class Gapper
    {
        /// <summary>
        /// Insert query.
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Insert parameters</param>
        /// <returns>Id of inserted object.</returns>
        public static int Insert<T>(this IDbConnection connection, object parameters)
        {
            return connection.Query<int>(                
                sql: StatementHelper.GenerateInsertStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text).FirstOrDefault();
        }

        /// <summary>
        /// Insert async query.
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Insert parameters</param>
        /// <returns>Id of inserted object.</returns>
        public static async Task<int> InsertAsync<T>(this IDbConnection connection, object parameters)
        {
            var result = await connection.QueryAsync<int>(
                sql: StatementHelper.GenerateInsertStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Update query.
        /// </summary>
        /// <typeparam name="T">The type to be update.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Update parameters, plus Id of object.</param>
        public static void Update<T>(this IDbConnection connection, object parameters)
        {
            connection.Execute(
                sql: StatementHelper.GenerateUpdateStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text);
        }

        /// <summary>
        /// Update async query.
        /// </summary>
        /// <typeparam name="T">The type to be update.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Update parameters, plus Id of object.</param>
        public static async Task UpdateAsync<T>(this IDbConnection connection, object parameters)
        {
            await connection.ExecuteAsync(
                sql: StatementHelper.GenerateUpdateStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text).ConfigureAwait(false);
        }

        /// <summary>
        /// Select query.
        /// </summary>
        /// <typeparam name="T">The type to be selected.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Where parameters.</param>
        /// <returns>Requested object.</returns>
        public static List<T> Select<T>(this IDbConnection connection, object parameters)
        {
            return connection.Query<T>(
                sql: StatementHelper.GenerateSelectStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text).ToList();
        }

        /// <summary>
        /// Select async query.
        /// </summary>
        /// <typeparam name="T">The type to be selected.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Where parameters.</param>
        /// <returns>Requested object.</returns>
        public static async Task<List<T>> SelectAsync<T>(this IDbConnection connection, object parameters)
        {
            var result = await connection.QueryAsync<T>(
                sql: StatementHelper.GenerateSelectStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.ToList();
        }

        /// <summary>
        /// Delete query.
        /// </summary>
        /// <typeparam name="T">The type to be deleted.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Where parameters.</param>
        public static void Delete<T>(this IDbConnection connection, object parameters)
        {
            connection.Execute(
                sql: StatementHelper.GenerateDeleteStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text);
        }

        /// <summary>
        /// Delete async query.
        /// </summary>
        /// <typeparam name="T">The type to be deleted.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="parameters">Where parameters.</param>
        public static async Task DeleteAsync<T>(this IDbConnection connection, object parameters)
        {
            await connection.ExecuteAsync(
                sql: StatementHelper.GenerateDeleteStatement<T>(parameters),
                param: parameters,
                commandType: CommandType.Text).ConfigureAwait(false);
        }

        /// <summary>
        /// Get table name from type.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns>Table name as string.</returns>      
        public static string GetTableName<T>()
        {
            return StatementHelper.GenerateTableName<T>();
        }
    }
}
