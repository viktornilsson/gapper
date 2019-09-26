using Gapper.Helpers;
using Gapper.Builders;
using System.Collections.Generic;
using System.Data;

namespace Gapper
{
    public class UpdateValues : Dictionary<string, object> { }

    public static class Gapper
    {
        public static ISelectBuilder<T> Select<T>(this IDbConnection connection)
        {
            return new SelectBuilder<T>(connection) as SelectBuilder<T>;
        }

        public static IInsertBuilder<T> Insert<T>(this IDbConnection connection, T obj)
        {
            return new InsertBuilder<T>(connection, obj) as InsertBuilder<T>;
        }

        public static IUpdateBuilder<T> Update<T>(this IDbConnection connection, UpdateValues values)
        {
            return new UpdateBuilder<T>(connection, values) as UpdateBuilder<T>;
        }

        public static IDeleteBuilder<T> Delete<T>(this IDbConnection connection)
        {
            return new DeleteBuilder<T>(connection) as DeleteBuilder<T>;
        }

        public static string GetTableName<T>()
        {
            return TableNameHelper.GenerateTableName<T>();
        }        
    }
}
