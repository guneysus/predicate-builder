using Northwind.Domain.Entities;
using NorthwindApp;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace dql.tests
{
    public class BasicQueries
    {
        public BasicQueries()
        {
            this._db = new NorthwindDbContext(@"Data Source=..\..\..\..\..\data\Northwind_small.sqlite;");
        }

        public NorthwindDbContext _db { get; }

        [Fact]
        public void SimpleSelect()
        {
            var pwd = Environment.CurrentDirectory;
            var query = "select * from Products;";
            var parser = new dql.Dql();

            var (context, str) = parser.Parse(query);
            var tableName = context.selectAllFromTable().TABLE_NAME().GetText();

            try
            {
                var expected = _db.Set<Product>().Where(x => true).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
