using System;
using System.Linq;
using Xunit;

namespace NorthwindApp.Tests
{
    public class UnitTest1
    {
        private readonly NorthwindDbContext db;

        public UnitTest1()
        {
            this.db = new NorthwindDbContext();
        }

        [Fact]
        public void Test1()
        {
            var p1 = db.Products.First();

        }
    }
}
