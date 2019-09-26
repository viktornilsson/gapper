using Gapper.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Gapper.Helpers
{
    public class TableNameHelper
    {
        public static string GenerateTableName<T>()
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
    }
}
