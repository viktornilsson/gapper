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
    public interface IUpdateSetBuilder<TClass> : IStatementBuilder
    {
        IUpdateWhereBuilder<TClass> Set<TProp>(Expression<Func<TClass, TProp>> expression, TProp value);
    }

    public interface IUpdateWhereBuilder<TClass> : IStatementBuilder, IUpdateSetBuilder<TClass>
    {
        IConditionBuilder<IUpdateExecuteBuilder<TClass>> Where<TProp>(Expression<Func<TClass, TProp>> exp);
    }

    public interface IUpdateExecuteBuilder<TClass> : IStatementBuilder
    {
        IConditionBuilder<IUpdateExecuteBuilder<TClass>> And<TProp>(Expression<Func<TClass, TProp>> exp);
        IConditionBuilder<IUpdateExecuteBuilder<TClass>> Or<TProp>(Expression<Func<TClass, TProp>> exp);
        void Execute();
        Task ExecuteAsync();
    }

    internal class UpdateBuilder<TClass> : StatementBuilder, IUpdateSetBuilder<TClass>, IUpdateWhereBuilder<TClass>, IUpdateExecuteBuilder<TClass>
    {
        private readonly IDbConnection _dbConnection;
        private int setCount = 0;

        public UpdateBuilder(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            AddLine($"UPDATE {TableNameHelper.GenerateTableName<TClass>()} SET ");
        }

        public IUpdateWhereBuilder<TClass> Set<TProp>(Expression<Func<TClass, TProp>> expression, TProp value)
        {
            setCount++;

            var propertyName = ExpressionHelper.GetPropName(expression);
            var parameterName = AddParameter(propertyName, value);
            var commaSign = setCount > 1 ? "," : string.Empty;

            AddLine($"{commaSign}[{propertyName}] = @{parameterName}");

            return this;
        }

        public IConditionBuilder<IUpdateExecuteBuilder<TClass>> Where<TProp>(Expression<Func<TClass, TProp>> exp)
        {
            return new ConditionBuilder<IUpdateExecuteBuilder<TClass>>(this, "WHERE", ExpressionHelper.GetPropName(exp));
        }

        public IConditionBuilder<IUpdateExecuteBuilder<TClass>> And<TProp>(Expression<Func<TClass, TProp>> exp)
        {
            return new ConditionBuilder<IUpdateExecuteBuilder<TClass>>(this, "AND", ExpressionHelper.GetPropName(exp));
        }

        public IConditionBuilder<IUpdateExecuteBuilder<TClass>> Or<TProp>(Expression<Func<TClass, TProp>> exp)
        {
            return new ConditionBuilder<IUpdateExecuteBuilder<TClass>>(this, "OR", ExpressionHelper.GetPropName(exp));
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