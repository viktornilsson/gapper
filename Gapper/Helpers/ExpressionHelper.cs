using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Gapper.Helpers
{
    public static class ExpressionHelper
    {
        public static string GetPropName<TClass, TProp>(Expression<Func<TClass, TProp>> expression)
        {
            if (!(expression.Body is MemberExpression memberAccess))
                throw new InvalidOperationException("Expression must be a member access expression");

            var propertyInfo = memberAccess?.Member as PropertyInfo;

            return propertyInfo?.Name;
        }
    }
}
