using Dapper;
using System.Data.SqlClient;

namespace Gapper.Tests.IntegrationTests.Helpers
{
    internal static class DatabaseHelper
    {
        internal static string GetConnectionString(bool isAppVeyor)
        {
            return isAppVeyor ?
                "Server=(local)\\SQL2016;Database=master;User ID=sa;Password=Password12!" :
                "Server = (localdb)\\MSSQLLocalDB; Integrated Security = true; Database = dbTest;";
        }

        internal static void CreateTable(string connString)
        {
            using (var conn = new SqlConnection(connString))
            {
                var sql = @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'User')
                BEGIN
                    CREATE TABLE [dbo].[User] (
	                        [Id]			INT				IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	                        [Name]			NVARCHAR(256)	NOT NULL,
                            [Age]			INT	            NOT NULL,
	                        [CreatedDate]	DATETIME		NOT NULL DEFAULT GETDATE()
                    );
                END";

                conn.Execute(sql);
            }
        }
    }
}
