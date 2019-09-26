using Dapper;
using Gapper.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Gapper.Builders
{
    public interface ISelectExecute<T>
    {
        Task<List<T>> ToListAsync();
        Task<T> FirstOrDefaultAsync();
    }

    public interface ISelectBuilder<T> : IStatementBuilder, ISelectExecute<T>
    {
        IConditionBuilder<ISelectWhereBuilder<T>> Where(string columnName);
    }

    public interface ISelectWhereBuilder<T> : IStatementBuilder, ISelectExecute<T>
    {
        IConditionBuilder<ISelectWhereBuilder<T>> And(string columnName);
        IConditionBuilder<ISelectWhereBuilder<T>> Or(string columnName);
    }

    internal class SelectBuilder<T> : StatementBuilder, ISelectBuilder<T>, ISelectWhereBuilder<T>
    {
        private readonly IDbConnection DbConnection;

        public SelectBuilder(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;

            AddLine($"SELECT * FROM {TableNameHelper.GenerateTableName<T>()}");
        }

        public IConditionBuilder<ISelectWhereBuilder<T>> Where(string columnName)
        {
            return new ConditionBuilder<ISelectWhereBuilder<T>>(this, "WHERE", columnName);
        }

        public IConditionBuilder<ISelectWhereBuilder<T>> And(string columnName)
        {
            return new ConditionBuilder<ISelectWhereBuilder<T>>(this, "AND", columnName);
        }

        public IConditionBuilder<ISelectWhereBuilder<T>> Or(string columnName)
        {
            return new ConditionBuilder<ISelectWhereBuilder<T>>(this, "OR", columnName);
        }

        private new ISelectBuilder<T> AddLine(string line)
        {
            return base.AddLine(line) as ISelectBuilder<T>;
        }

        public async Task<List<T>> ToListAsync()
        {
            var sql = ToSql();

            var result = await DbConnection.QueryAsync<T>(
                sql: sql,
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.ToList();
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            var sql = ToSql();

            var result = await DbConnection.QueryAsync<T>(
                sql: sql,
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.FirstOrDefault();
        }
    }
}
