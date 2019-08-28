namespace Gapper.Expressions
{
    public class Statement
    {
        public Statement(string name, Operator @operator, object value)
        {
            Name = name;
            Operator = @operator;
            Value = value;
        }

        public string Name { get; private set; }
        public Operator Operator { get; private set; }
        public object Value { get; private set; }
    }
}
