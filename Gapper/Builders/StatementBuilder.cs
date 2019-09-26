using System.Collections.Generic;

namespace Gapper.Builders
{
    public interface IStatementBuilder
    {
        string ToSql();
    }

    internal abstract class StatementBuilder : IStatementBuilder
    {
        private readonly IList<string> Lines = new List<string>();
        internal readonly IDictionary<string, object> Parameters = new Dictionary<string, object>();

        public string ToSql()
        {
            return string.Join("\n", Lines);
        }

        public IStatementBuilder AddLine(string line)
        {
            Lines.Add(line);

            return this;
        }

        public string AddParameter(string name, object value)
        {
            var parameterName = $"{name}_{Parameters.Count + 1}".ToLower();

            Parameters.Add(parameterName, value);

            return parameterName;
        }
    }
}
