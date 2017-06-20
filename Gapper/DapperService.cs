using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using Gapper.Attributes;
using System.Data.SqlClient;

namespace Gapper
{
    public class DapperService
    {
        private readonly string _connectionString;

        public DapperService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Runs an INSERT query.
        /// </summary>
        /// <typeparam name="T">Object that should be updated in database.</typeparam>
        /// <param name="parameters">Insert parameters</param>
        /// <returns>ObjectId</returns>
        protected int Insert<T>(object parameters)
        {
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var propInfo in parameters.GetType().GetProperties().Where(x => x.Name != "Id").OrderBy(x => x.Name))
            {
                columns.Add($"[{propInfo.Name}]");
                values.Add($"@{propInfo.Name}");
            }

            var sql =
                $"INSERT INTO {GetTableName<T>()} ({string.Join(",", columns.ToArray())}) VALUES ({string.Join(",", values.ToArray())}) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            int returnId;

            using (var conn = GetConnection())
            {
                returnId = conn.Query<int>(
                    sql: sql,
                    param: parameters,
                    commandType: CommandType.Text).FirstOrDefault();
            }

            return returnId;
        }

        /// <summary>
        /// Runs an UPDATE query.
        /// </summary>
        /// <typeparam name="T">Object that should be updated in database.</typeparam>
        /// <param name="parameters">Udpate parameters, plus Id of object.</param>
        /// <returns></returns>
        protected void Update<T>(object parameters)
        {
            var propertyInfos = parameters.GetType()
                .GetProperties()
                .Where(x => (x != null) && (x.Name != "Id"))
                .OrderBy(x => x.Name);

            var updates = propertyInfos.Select(propInfo => string.Format("[{0}]=@{0}", propInfo.Name)).ToArray();
            var sql = $"UPDATE {GetTableName<T>()} SET {string.Join(",", updates)} WHERE [ID] = @Id";

            using (var conn = GetConnection())
            {
                conn.Query<int>(
                    sql: sql,
                    param: parameters,
                    commandType: CommandType.Text);
            }
        }

        /// <summary>
        /// Runs a SELECT query.
        /// </summary>
        /// <typeparam name="T">Object that should be selected from in database.</typeparam>
        /// <param name="parameters">Where parameters.</param>
        /// <returns>Requested object.</returns>
        protected List<T> Select<T>(object parameters)
        {
            var propertyInfos = parameters.GetType()
                .GetProperties()
                .Where(x => (x != null))
                .OrderBy(x => x.Name);

            var wheres = propertyInfos.Select(propInfo => string.Format("[{0}]=@{0}", propInfo.Name)).ToArray();

            var sql = $"SELECT * FROM {GetTableName<T>()}";

            if (wheres.Length > 0)
                sql += $" WHERE {string.Join(" AND ", wheres)}";

            object obj;

            using (var conn = GetConnection())
            {
                obj = conn.Query<T>(
                    sql: sql,
                    param: parameters,
                    commandType: CommandType.Text).ToList();
            }

            return (List<T>)obj;
        }

        /// <summary>
        /// Runs a SELECT query.
        /// </summary>
        /// <typeparam name="T">Object that should be selected from in database.</typeparam>
        /// <param name="where">T-SQL Where clause.</param>
        /// <returns>Requested object.</returns>
        protected List<T> Select<T>(string where)
        {
            if (string.IsNullOrEmpty(where))
                throw new InvalidOperationException("Where cannot be empty.");

            var sql = $"SELECT * FROM { GetTableName<T>() } WHERE { where }";

            object obj;

            using (var conn = GetConnection())
            {
                obj = conn.Query<T>(
                    sql: sql,
                    commandType: CommandType.Text).ToList();
            }

            return (List<T>)obj;
        }

        /// <summary>
        /// Runs a DELETE query.
        /// </summary>
        /// <typeparam name="T">Object that should be deleted from in database.</typeparam>
        /// <param name="parameters">Where parameters.</param>
        /// <returns></returns>
        protected void Delete<T>(object parameters)
        {
            var propertyInfos = parameters.GetType()
                .GetProperties()
                .Where(x => (x != null))
                .OrderBy(x => x.Name);

            var wheres = propertyInfos
                .Select(propInfo => string.Format("[{0}]=@{0}", propInfo.Name))
                .ToArray();

            if (wheres.Length < 1)
                throw new InvalidOperationException("Wheres cannot be empty.");

            var sql = $"DELETE FROM {GetTableName<T>()} WHERE {string.Join(" AND ", wheres)}";

            using (var conn = GetConnection())
            {
                conn.Execute(
                    sql: sql,
                    param: parameters,
                    commandType: CommandType.Text);
            }
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected static string GetTableName<T>()
        {
            var type = typeof(T);

            if (type == null)
                throw new InvalidOperationException("Type cannot be null");

            var attributes = type.GetTypeInfo().GetCustomAttributes<TableAttribute>();

            if (attributes.Count() > 0)
            {
                var tableAttr = attributes.First();

                if (string.IsNullOrEmpty(tableAttr.Schema) || string.IsNullOrEmpty(tableAttr.Name))
                    throw new InvalidOperationException("Both Schema and Name must be specified.");

                return $"[{tableAttr.Schema}].[{tableAttr.Name}]";
            }

            return $"[dbo].[{typeof(T).Name}]";
        }
    }
}

