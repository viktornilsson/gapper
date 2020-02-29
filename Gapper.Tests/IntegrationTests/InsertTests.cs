using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Tests.IntegrationTests.Models;
using System.Data.SqlClient;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class InsertTests
    {
        [TestMethod]
        public void InsertUser()
        {
            var user = new User
            {
                Name = "Pelle",
                Age = 50,
            };

            var sql = new SqlConnection("")
                .Insert(user)
                .ToSql();

            var expected = "INSERT INTO [dbo].[User] ([Age],[CreatedDate],[Name]) \nVALUES (@age_1,@createddate_2,@name_3)\nSELECT CAST(SCOPE_IDENTITY() AS INT)";

            Assert.AreEqual(expected, sql);
        }
    }
}
