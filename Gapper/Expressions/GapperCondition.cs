using System.Collections.Generic;

namespace Gapper.Expressions
{
    public class GapperCondition : IGapperCondition, IGapperConditionAnd
    {
        public readonly List<Statement> Statements = new List<Statement>();

        public IGapperConditionAnd And(string name, Operator @operator, object value)
        {
            Statements.Add(new Statement(name, @operator, value));

            return this;
        }

        public IGapperConditionAnd Condition(string name, Operator @operator, object value)
        {
            Statements.Add(new Statement(name, @operator, value));

            return this;
        }
    }  
}
