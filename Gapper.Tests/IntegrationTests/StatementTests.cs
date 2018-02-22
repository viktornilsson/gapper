using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gapper.Helpers;

namespace Gapper.Tests.IntegrationTests
{
    [TestClass]
    public class StatementTests
    {
        [TestMethod]
        public void InsertStatement()
        {
            var user = new User()
            {
                Id = 1,
                Name = "John Doe"
            };

            var statement = StatementHelper.GenerateInsertStatement<User>(user);

            var equalTo = "INSERT INTO [dbo].[User] ([Name]) VALUES (@Name) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            Assert.AreEqual(statement, equalTo);
        }

        [TestMethod]
        public void SelectStatement()
        {
            int id = 1;

            var statement = StatementHelper.GenerateSelectStatement<User>(new { id });

            var equalTo = "SELECT * FROM [dbo].[User] WHERE [id]=@id";

            Assert.AreEqual(statement, equalTo);
        }

        [TestMethod]
        public void UpdateStatement()
        {
            var user = new User()
            {
                Id = 1,
                Name = "John Doe"
            };

            var statement = StatementHelper.GenerateUpdateStatement<User>(user);

            var equalTo = "UPDATE [dbo].[User] SET [Name]=@Name WHERE [Id] = @Id";

            Assert.AreEqual(statement, equalTo);
        }

        [TestMethod]
        public void UpdateStatementWithoutId()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                StatementHelper.GenerateUpdateStatement<User>(new { Name = "John Doe" })
            );
        }

        [TestMethod]
        public void DeleteStatement()
        {
            var statement = StatementHelper.GenerateDeleteStatement<User>(new { Id = 1 });

            var equalTo = "DELETE FROM [dbo].[User] WHERE [Id]=@Id";

            Assert.AreEqual(statement, equalTo);
        }

        [TestMethod]
        public void DeleteStatementWithoutWhere()
        {
            Assert.ThrowsException<InvalidOperationException>(() =>
                StatementHelper.GenerateDeleteStatement<User>(new {})
            );
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
