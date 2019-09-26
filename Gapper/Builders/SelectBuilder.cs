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
        List<T> ToList();        
        Task<List<T>> ToListAsync();
        T FirstOrDefault();
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

        public List<T> ToList()
        {
            return DbConnection.Query<T>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ToList();
        }

        public async Task<List<T>> ToListAsync()
        {
            var result = await DbConnection.QueryAsync<T>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.ToList();
        }

        public T FirstOrDefault()
        {
            return DbConnection.Query<T>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).FirstOrDefault();
        }

        public async Task<T> FirstOrDefaultAsync()
        {
            var result = await DbConnection.QueryAsync<T>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.FirstOrDefault();
        }
    }
}
