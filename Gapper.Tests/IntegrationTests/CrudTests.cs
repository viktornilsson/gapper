using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Gapper.Tests.IntegrationTests.Helpers;
using Gapper.Tests.IntegrationTests.Models;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class CrudTests
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";

        [TestMethod]
        public void CrudTest()
        {
            var connString = DatabaseHelper.GetConnectionString(IsAppVeyor);
            DatabaseHelper.CreateTable(connString);
            DatabaseHelper.EmptyTable(connString);

            using (var sqlConnection = new SqlConnection(connString))
            {
                var insertUser = new User() { Id = 1, Name = "Sten", Age = 20 };

                var newId = sqlConnection
                    .Insert(insertUser)
                    .Execute();

                Assert.IsTrue(newId > 0);

                var user = sqlConnection
                    .Select<User>()
                    .Where(a => a.Name)
                    .EqualTo("Sten")
                    .And(a => a.Id).EqualTo(newId)
                    .FirstOrDefault();

                Assert.IsTrue(user != null);

                sqlConnection
                    .Update<User>()
                    .Set(x => x.Name, "Pelle")
                    .Set(x => x.Age, 1)
                    .Where(u => u.Name).EqualTo("Sten")
                    .Execute();

                sqlConnection
                    .Delete<User>()
                    .Where(u => u.Id).EqualTo(newId)
                    .And(u => u.Name).NotEqualTo("Sten")
                    .Execute();

                var delUsers = sqlConnection
                    .Select<User>()
                    .ToList();

                Assert.IsTrue(delUsers.Count == 0);
            }
        } 
    }
}
