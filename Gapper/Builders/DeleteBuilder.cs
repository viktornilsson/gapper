using Dapper;
using Gapper.Helpers;
using System.Data;
using System.Threading.Tasks;

namespace Gapper.Builders
{
    public interface IDeleteBuilder<T> : IStatementBuilder
    {
        IConditionBuilder<IDeleteWhereBuilder<T>> Where(string columnName);
    }

    public interface IDeleteWhereBuilder<T> : IStatementBuilder
    {
        IConditionBuilder<IDeleteWhereBuilder<T>> And(string columnName);
        IConditionBuilder<IDeleteWhereBuilder<T>> Or(string columnName);
        void Execute();
        Task ExecuteAsync();
    }

    internal class DeleteBuilder<T> : StatementBuilder, IDeleteBuilder<T>, IDeleteWhereBuilder<T>
    {
        private readonly IDbConnection DbConnection;

        public DeleteBuilder(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;

            AddLine($"DELETE FROM {TableNameHelper.GenerateTableName<T>()}");
        }

        public IConditionBuilder<IDeleteWhereBuilder<T>> Where(string columnName)
        {
            return new ConditionBuilder<IDeleteWhereBuilder<T>>(this, "WHERE", columnName);
        }

        public IConditionBuilder<IDeleteWhereBuilder<T>> And(string columnName)
        {
            return new ConditionBuilder<IDeleteWhereBuilder<T>>(this, "AND", columnName);
        }

        public IConditionBuilder<IDeleteWhereBuilder<T>> Or(string columnName)
        {
            return new ConditionBuilder<IDeleteWhereBuilder<T>>(this, "OR", columnName);
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
