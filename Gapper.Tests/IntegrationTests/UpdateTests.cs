using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Tests.IntegrationTests.Models;
using System.Data.SqlClient;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class UpdateTests
    {
        [TestMethod]
        public void EqualTo()
        {
            var sql = new SqlConnection("")
                .Update<User>()
                .Set(x => x.Name, "Kalle")
                .Where(u => u.Id).EqualTo(1)
                .ToSql();

            var expected = "UPDATE [dbo].[User] SET \n[Name] = @name_1\nWHERE [Id] = @id_2";

            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void SetTwoPropsEqualTo()
        {
            var sql = new SqlConnection("")
                .Update<User>()
                .Set(x => x.Name, "Kalle")
                .Set(x => x.Age, 11)
                .Where(u => u.Id).EqualTo(1)
                .ToSql();

            var expected = "UPDATE [dbo].[User] SET \n[Name] = @name_1\n,[Age] = @age_2\nWHERE [Id] = @id_3";

            Assert.AreEqual(expected, sql);
        }

        [TestMethod]
        public void SetThreePropsEqualTo()
        {
            var sql = new SqlConnection("")
                .Update<User>()
                .Set(x => x.Name, "Kalle")
                .Set(x => x.Age, 11)
                .Set(x => x.Age, 22)
                .Where(u => u.Id).EqualTo(1)
                .ToSql();

            var expected = "UPDATE [dbo].[User] SET \n[Name] = @name_1\n,[Age] = @age_2\n,[Age] = @age_3\nWHERE [Id] = @id_4";

            Assert.AreEqual(expected, sql);
        }
    }
}

