using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Tests.IntegrationTests.Models;
using System.Data.SqlClient;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class DeleteTests
    {
        [TestMethod]
        public void DeleteAll()
        {
            var sql = new SqlConnection("")
                .Delete<User>()
                .ToSql();

            var expected = "DELETE FROM [dbo].[User]";

            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void DeleteWhere()
        {
            var sql = new SqlConnection("")
                .Delete<User>()
                .Where(u => u.Id).GreaterThan(0)
                .ToSql();

            var expected = "DELETE FROM [dbo].[User]\nWHERE [Id] > @id_1";

            Assert.AreEqual(expected, sql);
        }
    }
}
