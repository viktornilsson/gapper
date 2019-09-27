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

            using (var sqlConnection = new SqlConnection(connString))
            {
                var insertUser = new User() { Id = 1, Name = "Sten", Age = 20 };

                var newId = sqlConnection
                    .Insert(insertUser)
                    .Execute();

                Assert.IsTrue(newId > 0);

                var user = sqlConnection
                    .Select<User>()
                    .Where(nameof(User.Name)).EqualTo("Sten")
                    .And(nameof(User.Id)).EqualTo(newId)               
                    .FirstOrDefault();

                Assert.IsTrue(user != null);

                sqlConnection
                    .Update<User>(new UpdateValues
                    {
                        { nameof(User.Name), "Pelle" }
                    })
                    .Where(nameof(User.Name)).EqualTo("Sten")
                    .Execute();

                sqlConnection
                    .Delete<User>()
                    .Where(nameof(User.Id)).EqualTo(newId)
                    .And(nameof(User.Name)).NotEqualTo("Sten")
                    .Execute();

                var delUsers = sqlConnection
                    .Select<User>()
                    .ToList();

                Assert.IsTrue(delUsers.Count == 0);
            }                        
        }
        
    }
}
