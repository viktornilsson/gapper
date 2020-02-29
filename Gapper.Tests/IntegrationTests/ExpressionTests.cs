using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Helpers;
using Gapper.Tests.IntegrationTests.Models;
using System;
using System.Linq.Expressions;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void GetPropNameShouldReturnNullIfTheExpressionIsNotMemberExpression()
        {
            Expression<Func<User, string>> exp = p => "Constant";

            ThrowsException<InvalidOperationException>(() =>
                ExpressionHelper.GetPropName(exp)
            );
        }

        [TestMethod]
        public void GetPropNameShouldReturnTheCorrectPropName()
        {
            Expression<Func<User, string>> exp = p => p.Name;
            var result = ExpressionHelper.GetPropName(exp);

            AreEqual(result, "Name");
        }
    }
}
