using Dapper;
using Gapper.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Gapper.Builders
{
    public interface IInsertBuilder<T> : IStatementBuilder
    {
        int Execute();
        Task<int> ExecuteAsync();
    }

    internal class InsertBuilder<T> : StatementBuilder, IInsertBuilder<T>
    {
        private readonly IDbConnection DbConnection;

        public InsertBuilder(IDbConnection dbConnection, T obj)
        {
            DbConnection = dbConnection;

            GenerateInsertStatement(obj);
        }

        private void GenerateInsertStatement(object parameters)
        {
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var propInfo in parameters.GetType().GetProperties().Where(x => x.Name != "Id").OrderBy(x => x.Name))
            {
                var name = propInfo.Name;
                var value = propInfo.GetValue(parameters, null);

                var paramterName = AddParameter(name, value);

                columns.Add($"[{name}]");
                values.Add($"@{paramterName}");
            }

            AddLine($"INSERT INTO {TableNameHelper.GenerateTableName<T>()} ({string.Join(",", columns.ToArray())}) ");
            AddLine($"VALUES ({string.Join(",", values.ToArray())})");
            AddLine("SELECT CAST(SCOPE_IDENTITY() AS INT)");
        }

        public int Execute()
        {
            return DbConnection.Query<int>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).FirstOrDefault();
        }

        public async Task<int> ExecuteAsync()
        {
            var result = await DbConnection.QueryAsync<int>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.FirstOrDefault();
        }
    }
}
