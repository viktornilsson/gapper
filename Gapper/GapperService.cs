using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Gapper
{
    public class GapperService
    {
        private readonly string _connectionString;

        public GapperService(string connectionString)
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
                return conn.Insert<T>(parameters);
            }
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
                return await conn.InsertAsync<T>(parameters);                
            }
        }

        /// <summary>
        /// Runs an UPDATE query.
        /// </summary>
        /// <typeparam name="T">Object that should be updated in database.</typeparam>
        /// <param name="parameters">Update parameters, plus Id of object.</param>
        /// <returns></returns>
        protected void Update<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Update<T>(parameters);
            }
        }

        /// <summary>
        /// Runs an UPDATE query.
        /// </summary>
        /// <typeparam name="T">Object that should be updated in database.</typeparam>
        /// <param name="parameters">Update parameters, plus Id of object.</param>
        /// <returns></returns>
        protected async Task UpdateAsync<T>(object parameters)
        {
            using (var conn = GetConnection())
            {
                await conn.UpdateAsync<T>(parameters);
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
                return conn.Select<T>(parameters);
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
                return (await conn.SelectAsync<T>(parameters)).ToList();
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
                conn.Delete<T>(parameters);
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
                await conn.DeleteAsync<T>(parameters);
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
    }
}

