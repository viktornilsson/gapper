using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Gapper.Tests.IntegrationTests.Helpers;
using Gapper.Tests.IntegrationTests.Models;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class CrudAsyncTests
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";
        
        [TestMethod]
        public async Task CrudTestAsync()
        {
            var connString = DatabaseHelper.GetConnectionString(IsAppVeyor);
            DatabaseHelper.CreateTable(connString);

            using (var sqlConnection = new SqlConnection(connString))
            {
                var insertUser = new User() { Id = 1, Name = "Sten", Age = 20 };

                var newId = await sqlConnection
                    .Insert(insertUser)
                    .ExecuteAsync();

                Assert.IsTrue(newId > 0);

                var user = await sqlConnection
                    .Select<User>()
                    .Where(nameof(User.Name)).EqualTo("Sten")
                    .And(nameof(User.Id)).EqualTo(newId)               
                    .FirstOrDefaultAsync();

                Assert.IsTrue(user != null);

                await sqlConnection
                    .Update<User>(new UpdateValues
                    {
                        { nameof(User.Name), "Pelle" }
                    })
                    .Where(nameof(User.Name)).EqualTo("Sten")
                    .ExecuteAsync();

                await sqlConnection
                    .Delete<User>()
                    .Where(nameof(User.Id)).EqualTo(newId)
                    .And(nameof(User.Name)).NotEqualTo("Sten")
                    .ExecuteAsync();

                var delUsers = await sqlConnection
                    .Select<User>()
                    .ToListAsync();

                Assert.IsTrue(delUsers.Count == 0);
            }                        
        }
        
    }
}
