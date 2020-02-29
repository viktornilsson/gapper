using Dapper;
using Gapper.Helpers;
using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gapper.Builders
{
    public interface IDeleteBuilder<TClass> : IStatementBuilder
    {
        IConditionBuilder<IDeleteWhereBuilder<TClass>> Where<TProp>(Expression<Func<TClass, TProp>> exp);
    }

    public interface IDeleteWhereBuilder<TClass> : IStatementBuilder
    {
        IConditionBuilder<IDeleteWhereBuilder<TClass>> And<TProp>(Expression<Func<TClass, TProp>> exp);
        IConditionBuilder<IDeleteWhereBuilder<TClass>> Or<TProp>(Expression<Func<TClass, TProp>> exp);
        void Execute();
        Task ExecuteAsync();
    }

    internal class DeleteBuilder<TClass> : StatementBuilder, IDeleteBuilder<TClass>, IDeleteWhereBuilder<TClass>
    {
        private readonly IDbConnection _dbConnection;

        public DeleteBuilder(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            AddLine($"DELETE FROM {TableNameHelper.GenerateTableName<TClass>()}");
        }

        public IConditionBuilder<IDeleteWhereBuilder<TClass>> Where<TProp>(Expression<Func<TClass, TProp>> exp)
        {
            return new ConditionBuilder<IDeleteWhereBuilder<TClass>>(this, "WHERE", ExpressionHelper.GetPropName(exp));
        }

        public IConditionBuilder<IDeleteWhereBuilder<TClass>> And<TProp>(Expression<Func<TClass, TProp>> exp)
        {
            return new ConditionBuilder<IDeleteWhereBuilder<TClass>>(this, "AND", ExpressionHelper.GetPropName(exp));
        }

        public IConditionBuilder<IDeleteWhereBuilder<TClass>> Or<TProp>(Expression<Func<TClass, TProp>> exp)
        {
            return new ConditionBuilder<IDeleteWhereBuilder<TClass>>(this, "OR", ExpressionHelper.GetPropName(exp));
        }

        public void Execute()
        {
            _dbConnection.Execute(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text);
        }

        public async Task ExecuteAsync()
        {
            await _dbConnection.ExecuteAsync(
                sql: ToSql(),
                param: Parameters,
                commandType: CommandType.Text).ConfigureAwait(false);
        }
    }
}
