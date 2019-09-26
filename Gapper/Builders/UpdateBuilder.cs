using Dapper;
using Gapper.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Gapper.Gapper;

namespace Gapper.Builders
{
    public interface IUpdateExecute<T>
    {
        void Execute();
        Task ExecuteAsync();
    }

    public interface IUpdateBuilder<T> : IStatementBuilder, IUpdateExecute<T>
    {
        IConditionBuilder<IUpdateWhereBuilder<T>> Where(string columnName);
    }

    public interface IUpdateWhereBuilder<T> : IStatementBuilder, IUpdateExecute<T>
    {
        IConditionBuilder<IUpdateWhereBuilder<T>> And(string columnName);
        IConditionBuilder<IUpdateWhereBuilder<T>> Or(string columnName);
    }

    internal class UpdateBuilder<T> : StatementBuilder, IUpdateBuilder<T>, IUpdateWhereBuilder<T>
    {
        private readonly IDbConnection DbConnection;

        public UpdateBuilder(IDbConnection dbConnection, UpdateValues values)
        {
            DbConnection = dbConnection;
           
            var updateValues = new List<string>();

            foreach (var keyValue in values)
            {
                var parameterName = AddParameter(keyValue.Key, keyValue.Value);

                updateValues.Add($"[{keyValue.Key}] = @{parameterName}");
            }

            AddLine($"UPDATE {TableNameHelper.GenerateTableName<T>()} SET {string.Join(",", updateValues)}");
        }

        public IConditionBuilder<IUpdateWhereBuilder<T>> Where(string columnName)
        {
            return new ConditionBuilder<IUpdateWhereBuilder<T>>(this, "WHERE", columnName);
        }

        public IConditionBuilder<IUpdateWhereBuilder<T>> And(string columnName)
        {
            return new ConditionBuilder<IUpdateWhereBuilder<T>>(this, "AND", columnName);
        }

        public IConditionBuilder<IUpdateWhereBuilder<T>> Or(string columnName)
        {
            return new ConditionBuilder<IUpdateWhereBuilder<T>>(this, "OR", columnName);
        }

        public void Execute()
        {
            DbConnection.Execute(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text);
        }

        public async Task ExecuteAsync()
        {
            await DbConnection.ExecuteAsync(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);
        }
    }
}
