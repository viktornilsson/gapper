
namespace Gapper.Builders
{
    public interface IConditionBuilder<TParent>
    {
        TParent EqualTo(object value);
        TParent NotEqualTo(object value);
        TParent GreaterThan(object value);
        TParent LessThan(object value);
        TParent GreaterThanOrEqualTo(object value);
        TParent LessThanOrEqualTo(object value);
        TParent Like(string value);
    }

    internal class ConditionBuilder<TParent> : IConditionBuilder<TParent>
            where TParent : IStatementBuilder
    {
        private readonly StatementBuilder _parent;
        private readonly string _keyword;
        private readonly string _columnName;

        public ConditionBuilder(StatementBuilder parent, string keyword, string columnName)
        {
            _parent = parent;
            _keyword = keyword;
            _columnName = columnName;
        }

        public TParent EqualTo(object value)
        {
            return RegisterParameter("=", value);
        }

        public TParent NotEqualTo(object value)
        {
            return RegisterParameter("!=", value);
        }

        public TParent GreaterThan(object value)
        {
            return RegisterParameter(">", value);
        }

        public TParent LessThan(object value)
        {
            return RegisterParameter("<", value);
        }

        public TParent GreaterThanOrEqualTo(object value)
        {
            return RegisterParameter(">=", value);
        }

        public TParent LessThanOrEqualTo(object value)
        {
            return RegisterParameter("<=", value);
        }

        public TParent Like(string value)
        {
            return RegisterParameter("LIKE", value);
        }        

        private TParent RegisterParameter(string comparator, object value)
        {
            var parameterName = _parent.AddParameter(_columnName, value);

            var condition = $"{_keyword} [{_columnName}] {comparator} @{parameterName}";

            return (TParent)_parent.AddLine(condition);
        }
    }
}
