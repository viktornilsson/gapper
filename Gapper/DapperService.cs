﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System.Data.SqlClient;
using Gapper.Helpers;

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
            using (var conn = GetConnection())
            {
                return conn.Query<int>(
                    sql: StatementHelper.GenerateInsertStatement<T>(parameters),
                    param: parameters,
                    commandType: CommandType.Text).FirstOrDefault();
            }
        }

        /// <summary>
        /// Runs an UPDATE query.
        /// </summary>
        /// <typeparam name="T">Object that should be updated in database.</typeparam>
        /// <param name="parameters">Udpate parameters, plus Id of object.</param>
        /// <returns></returns>
        protected void Update<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Execute(
                    sql: StatementHelper.GenerateUpdateStatement<T>(parameters),
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
            using (var conn = GetConnection())
            {
                return conn.Query<T>(
                    sql: StatementHelper.GenerateSelectStatement<T>(parameters),
                    param: parameters,
                    commandType: CommandType.Text).ToList();
            }
        }

        /// <summary>
        /// Runs a DELETE query.
        /// </summary>
        /// <typeparam name="T">Object that should be deleted from in database.</typeparam>
        /// <param name="parameters">Where parameters.</param>
        /// <returns>Void</returns>
        protected void Delete<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Execute(
                    sql: StatementHelper.GenerateDeleteStatement<T>(parameters),
                    param: parameters,
                    commandType: CommandType.Text);
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

