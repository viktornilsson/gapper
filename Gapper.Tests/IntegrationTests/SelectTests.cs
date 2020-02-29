using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Tests.IntegrationTests.Models;
using System.Data.SqlClient;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class SelectTests
    {
        [TestMethod]
        public void SelectAll()
        {
            var sql = new SqlConnection("")
                .Select<User>()
                .ToSql();

            var expected = "SELECT * FROM [dbo].[User]";

            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void GreaterThanOrLessThan()
        {
            var sql = new SqlConnection("")
                .Select<User>()
                .Where(u => u.Id).GreaterThan(0)
                .Or(u => u.Id).LessThan(100)
                .ToSql();

            var expected = "SELECT * FROM [dbo].[User]\nWHERE [Id] > @id_1\nOR [Id] < @id_2";

            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void Like()
        {
            var sql = new SqlConnection("")
                .Select<User>()
                .Where(u => u.Name).Like("%FOO%")
                .ToSql();

            var expected = "SELECT * FROM [dbo].[User]\nWHERE [Name] LIKE @name_1";

            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void NotEqualTo()
        {
            var sql = new SqlConnection("")
                .Select<User>()
                .Where(u => u.Name).NotEqualTo("Foo")
                .ToSql();

            var expected = "SELECT * FROM [dbo].[User]\nWHERE [Name] != @name_1";

            Assert.AreEqual(expected, sql);
        }
    }
}
