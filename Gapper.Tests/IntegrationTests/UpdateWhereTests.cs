using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Gapper.Tests.IntegrationTests.Models;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class UpdateWhereTests
    { 
        [TestMethod]
        public void EqualTo()
        {
            var sql = new SqlConnection("")
                .Update<User>(new UpdateValues
                {
                    { nameof(User.Name), "Kalle" }
                })
                .Where(nameof(User.Id)).EqualTo(1)
                .ToSql();

            var expected = "UPDATE [dbo].[User] SET [Name] = @name_1\nWHERE [Id] = @id_2";
          
            Assert.AreEqual(expected, sql);
        }
    }
}
