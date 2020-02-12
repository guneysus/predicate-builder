using System.Collections.Generic;

namespace Northwind.Domain.Entities
{
    public class Category
    {
        public Category()
        {
            //Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        //public byte[] Picture { get; set; }

        //public ICollection<Product> Products { get; private set; }
    }
}
