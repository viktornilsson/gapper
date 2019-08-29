using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Gapper.Tests.IntegrationTests.Helpers;
using Gapper.Tests.IntegrationTests.Models;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class CrudTests : GapperService
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";
        private static string connString = DatabaseHelper.GetConnectionString(IsAppVeyor);

        public CrudTests() : base(connString)
        {

        }

        [TestMethod]
        public void CrudTest()
        {
            DatabaseHelper.CreateTable(connString);

            var user = new User() { Id = 1, Name = "Sten", Age = 20 };
            var newId = Insert<User>(user);

            Assert.IsTrue(newId > 0);

            var users = Select<User>(new { });

            Assert.IsTrue(users.Count > 0);

            Update<User>(new { Id = newId, Name = "Kalle" });

            Delete<User>(new { Name = "Kalle" });

            var delUsers = Select<User>(new { });
            Assert.IsTrue(delUsers.Count == 0);
        }
    }
}
