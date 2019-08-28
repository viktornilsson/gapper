
namespace Gapper.Expressions
{
    public interface IGapperCondition
    {
        IGapperConditionAnd Condition(string name, Operator @operator, object value);   
    }

    public interface IGapperConditionAnd
    {
        IGapperConditionAnd And(string name, Operator @operator, object value);
    }
}
