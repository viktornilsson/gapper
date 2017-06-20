using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Attributes;
using Gapper;
using System.Linq;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class TableNameTests
    {
        [TestMethod]
        public void CheckTableNames()
        {
            var service = new TestService();

            var nameUser = service.GetUserTableName();
            Assert.IsTrue(nameUser == "[users].[tblUser]");

            var nameCustomer = service.GetCustomerTableName();

            Assert.IsTrue(nameCustomer == "[dbo].[Customer]");
        }
    }

    [Table(Name = "tblUser", Schema = "users")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestService : DapperService
    {
        private static string connectionString;

        public TestService() : base(connectionString)
        {
            connectionString = string.Empty;
        }

        public string GetUserTableName()
        {
            return GetTableName<User>();
        }

        public string GetCustomerTableName()
        {
            return GetTableName<Customer>();
        }
    }
}
