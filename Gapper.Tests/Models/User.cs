using System;

namespace Gapper.Tests.IntegrationTests.Models
{
    public class User
    {
        public User()
        {
            Id = -1;
            CreatedDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
