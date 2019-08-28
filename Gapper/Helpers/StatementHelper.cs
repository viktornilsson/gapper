using Gapper.Attributes;
using Gapper.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Gapper.Tests")]

namespace Gapper.Helpers
{
    internal class StatementHelper
    {
        internal static string GenerateInsertStatement<T>(object parameters)
        {
            var columns = new List<string>();
            var values = new List<string>();

            foreach (var propInfo in parameters.GetType().GetProperties().Where(x => x.Name != "Id").OrderBy(x => x.Name))
            {
                columns.Add($"[{propInfo.Name}]");
                values.Add($"@{propInfo.Name}");
            }

            return
                $"INSERT INTO {GenerateTableName<T>()} ({string.Join(",", columns.ToArray())}) " +
                $"VALUES ({string.Join(",", values.ToArray())}) SELECT CAST(SCOPE_IDENTITY() AS INT)";
        }

        internal static string GenerateUpdateStatement<T>(object parameters)
        {
            var props = parameters.GetType().GetProperties();

            if (!props.Any(x=> x.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase)))
                throw new InvalidOperationException("Id is missing");

            var propertyInfos = props
                .Where(x => (x != null) && (x.Name != "Id"))
                .OrderBy(x => x.Name);

            var updates = propertyInfos.Select(propInfo => string.Format("[{0}]=@{0}", propInfo.Name)).ToArray();

            return $"UPDATE {GenerateTableName<T>()} SET {string.Join(",", updates)} WHERE [Id] = @Id";
        }

        internal static string GenerateUpdateStatement<T>(UpdateValues values, List<Statement> statements)
        {           
            var updates = values.Select(x => $"[{x.Key}] = @{x.Key}");

            if (updates.Count() < 1)
                throw new InvalidOperationException("Values cannot be empty");

            var wheres = statements.Select(x => $"[{x.Name}] {GetOperator(x.Operator)} @{x.Name}");

            if (wheres.Count() < 1)
                throw new InvalidOperationException("Wheres cannot be empty");

            return $"UPDATE {GenerateTableName<T>()} SET {string.Join(",", updates)} WHERE {string.Join(" AND ", wheres)}";
        }

        internal static string GenerateSelectStatement<T>(object parameters)
        {
            var propertyInfos = parameters.GetType()
                .GetProperties()
                .Where(x => (x != null))
                .OrderBy(x => x.Name);

            var wheres = propertyInfos.Select(propInfo => string.Format("[{0}]=@{0}", propInfo.Name)).ToArray();

            var sql = $"SELECT * FROM {GenerateTableName<T>()}";

            if (wheres.Length > 0)
                sql += $" WHERE {string.Join(" AND ", wheres)}";

            return sql;
        }

        internal static string GenerateSelectStatement<T>(List<Statement> statements)
        {
            var wheres = statements.Select(x => $"[{x.Name}] {GetOperator(x.Operator)} @{x.Name}");

            var sql = $"SELECT * FROM {GenerateTableName<T>()}";

            if (wheres.Count() > 0)
                sql += $" WHERE {string.Join(" AND ", wheres)}";

            return sql;
        }

        internal static string GenerateDeleteStatement<T>(object parameters)
        {
            var propertyInfos = parameters.GetType()
                .GetProperties()
                .Where(x => (x != null))
                .OrderBy(x => x.Name);

            var wheres = propertyInfos
                .Select(propInfo => string.Format("[{0}]=@{0}", propInfo.Name))
                .ToArray();

            if (wheres.Length < 1)
                throw new InvalidOperationException("Wheres cannot be empty");

            return $"DELETE FROM {GenerateTableName<T>()} WHERE {string.Join(" AND ", wheres)}";
        }

        internal static string GenerateDeleteStatement<T>(List<Statement> statements)
        {
            var wheres = statements.Select(x => $"[{x.Name}] {GetOperator(x.Operator)} @{x.Name}");

            if (wheres.Count() < 1)
                throw new InvalidOperationException("Wheres cannot be empty");

            return $"DELETE FROM {GenerateTableName<T>()} WHERE {string.Join(" AND ", wheres)}";
        }

        internal static string GenerateTableName<T>()
        {
            var type = typeof(T);

            if (type == null)
                throw new InvalidOperationException("Type cannot be null");

            var attributes = type.GetTypeInfo().GetCustomAttributes<TableAttribute>();

            if (attributes.Count() > 0)
            {
                var tableAttr = attributes.First();

                if (string.IsNullOrEmpty(tableAttr.Schema) || string.IsNullOrEmpty(tableAttr.Name))
                    throw new InvalidOperationException("Both Schema and Name must be specified.");

                return $"[{tableAttr.Schema}].[{tableAttr.Name}]";
            }

            return $"[dbo].[{typeof(T).Name}]";
        }

        private static string GetOperator(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Equal:
                    return "=";
                case Operator.NotEqual:
                    return "<>";
                case Operator.GreaterThan:
                    return ">";
                case Operator.LessThan:
                    return "<";
                default:
                    throw new ArgumentNullException(nameof(@operator));
            }
        }
    }
}
