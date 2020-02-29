using Gapper.Helpers;
using Gapper.Builders;
using System.Data;

namespace Gapper
{
    public static class Gapper
    {
        public static ISelectBuilder<TClass> Select<TClass>(this IDbConnection connection)
        {
            return new SelectBuilder<TClass>(connection) as SelectBuilder<TClass>;
        }

        public static IInsertBuilder<TClass> Insert<TClass>(this IDbConnection connection, TClass obj)
        {
            return new InsertBuilder<TClass>(connection, obj) as InsertBuilder<TClass>;
        }

        public static IUpdateSetBuilder<TClass> Update<TClass>(this IDbConnection connection)
        {
            return new UpdateBuilder<TClass>(connection) as UpdateBuilder<TClass>;
        }

        public static IDeleteBuilder<TClass> Delete<TClass>(this IDbConnection connection)
        {
            return new DeleteBuilder<TClass>(connection) as DeleteBuilder<TClass>;
        }

        public static string GetTableName<TClass>()
        {
            return TableNameHelper.GenerateTableName<TClass>();
        }
    }

}
