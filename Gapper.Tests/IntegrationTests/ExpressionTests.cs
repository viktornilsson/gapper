using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Expressions;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using System;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class ExpressionTests
    {
        protected static readonly bool IsAppVeyor = Environment.GetEnvironmentVariable("Appveyor")?.ToUpperInvariant() == "TRUE";
        
        [TestMethod]
        public void QueryTest()
        {
            int id = 1;
            string name = "Sten";

            string connString = IsAppVeyor ?
                "Server=(local)\\SQL2016;Database=master;User ID=sa;Password=Password12!" :
                "Server=(localdb)\\MSSQLLocalDB; Integrated Security = true; Database = dbTest;";

            CreateTable(connString);

            using (var conn = new SqlConnection(connString))
            {                
                var user = new User() { Id = 1, Name = name };
                var newId = conn.Insert(user);

                var selectUser = conn.Select<User>(x =>
                    x.Condition(nameof(name), Operator.Equal, name)
                );

                var values = new UpdateValues
                {
                    { nameof(name), "Kalle" }
                };

                conn.Update<User>(values, x => x.Condition(nameof(id), Operator.Equal, newId));

                conn.Delete<User>(q => q
                    .Condition(nameof(id), Operator.Equal, newId)
                    .And(nameof(name), Operator.Equal, "Kalle"));                                    

                Assert.IsTrue(newId > 0);
            }                        
        }
        
        [TestMethod]
        public async Task QueryAsyncTest()
        {
            int id = 1;
            string name = "Sten";

            string connString = IsAppVeyor ?
                "Server=(local)\\SQL2016;Database=master;User ID=sa;Password=Password12!" :
                "Server = (localdb)\\MSSQLLocalDB; Integrated Security = true; Database = dbTest;";

            CreateTable(connString);

            using (var conn = new SqlConnection(connString))
            {
                var user = new User() { Id = 1, Name = name };
                var newId = await conn.InsertAsync(user);

                Assert.IsTrue(newId > 0);

                var selectUser = await conn.SelectAsync<User>(x =>
                    x.Condition(nameof(name), Operator.Equal, name)
                );

                Assert.IsTrue(selectUser.Count > 0);

                var values = new UpdateValues
                {
                    { nameof(name), "Kalle" }
                };

                await conn.UpdateAsync<User>(values, x => x.Condition(nameof(id), Operator.Equal, newId));

                await conn.DeleteAsync<User>(q => q
                    .Condition(nameof(id), Operator.Equal, newId)
                    .And(nameof(name), Operator.Equal, "Kalle"));
            }
        }

        private void CreateTable(string connString)
        {
            using (var conn = new SqlConnection(connString))
            {
                var sql = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'User')
                BEGIN
                    CREATE TABLE [dbo].[User] (
	                        [Id]			INT				IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	                        [Name]			NVARCHAR(256)	NOT NULL,
	                        [CreatedDate]	DATETIME		NOT NULL DEFAULT GETDATE()
                    );
                END";

                conn.Execute(sql);
            }
        }

        private class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
