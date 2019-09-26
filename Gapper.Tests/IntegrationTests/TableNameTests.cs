using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Attributes;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class TableNameTests
    {
        [TestMethod]
        public void CheckTableNames()
        {
            var nameUser = Gapper.GetTableName<TestUser>();
            Assert.IsTrue(nameUser == "[users].[tblUser]");

            var nameCustomer = Gapper.GetTableName<TestCustomer>();

            Assert.IsTrue(nameCustomer == "[dbo].[TestCustomer]");
        }

        [Table(Name = "tblUser", Schema = "users")]
        public class TestUser
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class TestCustomer
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}