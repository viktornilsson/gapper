using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Gapper.Tests.IntegrationTests.Models;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class SelectWhereTests
    { 
        [TestMethod]
        public void GreaterThanOrLessThan()
        {
            var sql = new SqlConnection("")
                .Select<User>()
                .Where("Id").GreaterThan(0)               
                .Or("Id").LessThan(100)
                .ToSql();

            var expected = "SELECT * FROM [dbo].[User]\nWHERE [Id] > @id_1\nOR [Id] < @id_2";
          
            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void Like()
        {
            var sql = new SqlConnection("")
                .Select<User>()
                .Where("Name").Like("%FOO%")                
                .ToSql();

            var expected = "SELECT * FROM [dbo].[User]\nWHERE [Name] LIKE @name_1";

            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void NotEqualTo()
        {
            var sql = new SqlConnection("")
                .Select<User>()
                .Where("Name").NotEqualTo("Foo")
                .ToSql();

            var expected = "SELECT * FROM [dbo].[User]\nWHERE [Name] != @name_1";

            Assert.AreEqual(expected, sql);
        }
    }
}
