using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Expressions;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using Gapper.Tests.IntegrationTests.Helpers;
using Gapper.Tests.IntegrationTests.Models;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class ExpressionTests
    {
        private static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";
        
        [TestMethod]
        public void QueryTest()
        {
            int id = 1;
            string name = "Sten";

            var connString = DatabaseHelper.GetConnectionString(IsAppVeyor);
            DatabaseHelper.CreateTable(connString);

            using (var conn = new SqlConnection(connString))
            {                
                var user = new User() { Id = 1, Name = name, Age = 20 };
                var newId = conn.Insert(user);

                var selectUser = conn.Select<User>(x => x
                    .Condition(nameof(name), Operator.Eq, name)
                );

                var values = new UpdateValues
                {
                    { nameof(name), "Kalle" }
                };

                conn.Update<User>(values, x => x.Condition(nameof(id), Operator.Eq, newId));

                conn.Delete<User>(q => q
                    .Condition(nameof(id), Operator.Eq, newId)
                    .And(nameof(name), Operator.Eq, "Kalle"));

                var delUsers = conn.Select<User>(x => { });

                Assert.IsTrue(delUsers.Count == 0);
            }                        
        }
        
        [TestMethod]
        public async Task QueryAsyncTest()
        {            
            string name = "Sten";

            var connString = DatabaseHelper.GetConnectionString(IsAppVeyor);
            DatabaseHelper.CreateTable(connString);

            using (var conn = new SqlConnection(connString))
            {
                var user = new User() { Id = 1, Name = name, Age = 20 };
                var newId = await conn.InsertAsync(user);

                Assert.IsTrue(newId > 0);

                var selectUser = await conn.SelectAsync<User>(x =>
                    x.Condition(nameof(name), Operator.Eq, name)
                );

                Assert.IsTrue(selectUser.Count > 0);

                var values = new UpdateValues
                {
                    { nameof(name), "Kalle" }
                };

                await conn.UpdateAsync<User>(values, x => x.Condition("Id", Operator.Eq, newId));

                await conn.DeleteAsync<User>(q => q
                    .Condition("Id", Operator.Eq, newId)
                    .And(nameof(name), Operator.Eq, "Kalle"));

                var delUsers = await conn.SelectAsync<User>(x => { });

                Assert.IsTrue(delUsers.Count == 0);
            }
        }

        [TestMethod]
        public void LessThanTest()
        {
            var connString = DatabaseHelper.GetConnectionString(IsAppVeyor);
            DatabaseHelper.CreateTable(connString);

            using (var conn = new SqlConnection(connString))
            {
                var now = DateTime.UtcNow;
                var user = new User() { Id = 1, Name = "Sten", Age = 20, CreatedDate = now };
                var newId = conn.Insert(user);

                Assert.IsTrue(newId > 0);

                var noUsers = conn.Select<User>(x => 
                    x.Condition("CreatedDate", Operator.Lt, now));

                Assert.IsTrue(noUsers.Count == 0);

                var users = conn.Select<User>(x =>
                    x.Condition("CreatedDate", Operator.Gte, now));

                Assert.IsTrue(users.Count > 0);

                conn.Delete<User>(x => x.Condition("Id", Operator.Eq, newId));

                var delUsers = conn.Select<User>(x => { });
                Assert.IsTrue(delUsers.Count == 0);
            }
        }
    }
}
