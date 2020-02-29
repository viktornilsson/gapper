using Dapper;
using Gapper.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gapper.Builders
{
    public interface ISelectExecute<TClass>
    {
        List<TClass> ToList();
        Task<List<TClass>> ToListAsync();
        TClass FirstOrDefault();
        Task<TClass> FirstOrDefaultAsync();
    }

    public interface ISelectBuilder<TClass> : IStatementBuilder, ISelectExecute<TClass>
    {
        IConditionBuilder<ISelectWhereBuilder<TClass>> Where<TProp>(Expression<Func<TClass, TProp>> prop);
    }

    public interface ISelectWhereBuilder<TClass> : IStatementBuilder, ISelectExecute<TClass>
    {
        IConditionBuilder<ISelectWhereBuilder<TClass>> And<TProp>(Expression<Func<TClass, TProp>> prop);
        IConditionBuilder<ISelectWhereBuilder<TClass>> Or<TProp>(Expression<Func<TClass, TProp>> prop);
    }

    internal class SelectBuilder<TClass> : StatementBuilder, ISelectBuilder<TClass>, ISelectWhereBuilder<TClass>
    {
        private readonly IDbConnection _dbConnection;

        public SelectBuilder(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            AddLine($"SELECT * FROM {TableNameHelper.GenerateTableName<TClass>()}");
        }

        public IConditionBuilder<ISelectWhereBuilder<TClass>> Where<TProp>(Expression<Func<TClass, TProp>> prop)
        {
            return new ConditionBuilder<ISelectWhereBuilder<TClass>>(this, "WHERE", ExpressionHelper.GetPropName(prop));
        }

        public IConditionBuilder<ISelectWhereBuilder<TClass>> And<TProp>(Expression<Func<TClass, TProp>> prop)
        {
            return new ConditionBuilder<ISelectWhereBuilder<TClass>>(this, "AND", ExpressionHelper.GetPropName(prop));
        }

        public IConditionBuilder<ISelectWhereBuilder<TClass>> Or<TProp>(Expression<Func<TClass, TProp>> prop)
        {
            return new ConditionBuilder<ISelectWhereBuilder<TClass>>(this, "OR", ExpressionHelper.GetPropName(prop));
        }

        public List<TClass> ToList()
        {
            return _dbConnection.Query<TClass>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ToList();
        }

        public async Task<List<TClass>> ToListAsync()
        {
            var result = await _dbConnection.QueryAsync<TClass>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.ToList();
        }

        public TClass FirstOrDefault()
        {
            return _dbConnection.Query<TClass>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).FirstOrDefault();
        }

        public async Task<TClass> FirstOrDefaultAsync()
        {
            var result = await _dbConnection.QueryAsync<TClass>(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);

            return result.FirstOrDefault();
        }
    }
}
