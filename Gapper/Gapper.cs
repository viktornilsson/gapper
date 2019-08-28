using Dapper;
using Gapper.Expressions;
using Gapper.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Gapper
{
    public static class Gapper
    {
        /// <summary>
        /// Select query.
        /// </summary>
        /// <typeparam name="T">The type to be selected.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="conditions">Where conditions.</param>
        /// <returns>Requested object.</returns>
        public static List<T> Select<T>(this IDbConnection connection, Action<IGapperCondition> conditions)
        {
            var expression = new GapperCondition();
            conditions(expression);

            var parameters = expression.Statements.ToDictionary(x => x.Name, x => x.Value);

            return connection.Query<T>(
                sql: StatementHelper.GenerateSelectStatement<T>(expression.Statements),
                param: parameters,
                commandType: CommandType.Text).ToList();
        }

        /// <summary>
        /// Select async query.
        /// </summary>
        /// <typeparam name="T">The type to be selected.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="conditions">Where conditions.</param>
        /// <returns>Requested object.</returns>
        public static async Task<List<T>> SelectAsync<T>(this IDbConnection connection, Action<IGapperCondition> conditions)
        {
            var expression = new GapperCondition();
            conditions(expression);

            var parameters = expression.Statements.ToDictionary(x => x.Name, x => x.Value);

            var result = await connection.QueryAsync<T>(
                sql: StatementHelper.GenerateSelectStatement<T>(expression.Statements),
                param: parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.ToList();
        }

        /// <summary>
        /// Insert query.
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="obj">Insert object</param>
        /// <returns>Id of inserted object.</returns>
        public static int Insert<T>(this IDbConnection connection, T obj)
        {
            return connection.Query<int>(
                sql: StatementHelper.GenerateInsertStatement<T>(obj),
                param: obj,
                commandType: CommandType.Text).FirstOrDefault();
        }

        /// <summary>
        /// Insert async query.
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="obj">Insert object</param>
        /// <returns>Id of inserted object.</returns>
        public static async Task<int> InsertAsync<T>(this IDbConnection connection, T obj)
        {
            var result = await connection.QueryAsync<int>(
                sql: StatementHelper.GenerateInsertStatement<T>(obj),
                param: obj,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Update query.
        /// </summary>
        /// <typeparam name="T">The type to be update.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="conditions">Update conditions.</param>
        public static void Update<T>(this IDbConnection connection, UpdateValues values, Action<IGapperCondition> conditions)
        {
            var expression = new GapperCondition();
            conditions(expression);

            var whereConditions = expression.Statements.ToDictionary(x => x.Name, x => x.Value);

            var parameters = values.Concat(whereConditions)
               .GroupBy(kv => kv.Key)
               .ToDictionary(g => g.Key, g => g.First().Value);

            connection.Execute(
                sql: StatementHelper.GenerateUpdateStatement<T>(values, expression.Statements),
                param: parameters,
                commandType: CommandType.Text);
        }

        /// <summary>
        /// Update async query.
        /// </summary>
        /// <typeparam name="T">The type to be update.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="conditions">Update conditions.</param>
        public static async Task UpdateAsync<T>(this IDbConnection connection, UpdateValues values, Action<IGapperCondition> conditions)
        {
            var expression = new GapperCondition();
            conditions(expression);

            var whereConditions = expression.Statements.ToDictionary(x => x.Name, x => x.Value);

            var parameters = values.Concat(whereConditions)
               .GroupBy(kv => kv.Key)
               .ToDictionary(g => g.Key, g => g.First().Value);

            await connection.ExecuteAsync(
                sql: StatementHelper.GenerateUpdateStatement<T>(values, expression.Statements),
                param: parameters,
                commandType: CommandType.Text).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete query.
        /// </summary>
        /// <typeparam name="T">The type to be deleted.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="conditions">Where conditions.</param>
        public static void Delete<T>(this IDbConnection connection, Action<IGapperCondition> conditions)
        {
            var expression = new GapperCondition();
            conditions(expression);

            var parameters = expression.Statements.ToDictionary(x => x.Name, x => x.Value);

            connection.Execute(
                sql: StatementHelper.GenerateDeleteStatement<T>(expression.Statements),
                param: parameters,
                commandType: CommandType.Text);
        }

        /// <summary>
        /// Delete async query.
        /// </summary>
        /// <typeparam name="T">The type to be deleted.</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="conditions">Where conditions.</param>
        public static async Task DeleteAsync<T>(this IDbConnection connection, Action<IGapperCondition> conditions)
        {
            var expression = new GapperCondition();
            conditions(expression);

            var parameters = expression.Statements.ToDictionary(x => x.Name, x => x.Value);

            await connection.ExecuteAsync(
                sql: StatementHelper.GenerateDeleteStatement<T>(expression.Statements),
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
