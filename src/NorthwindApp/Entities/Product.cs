﻿using System.Collections.Generic;

namespace Northwind.Domain.Entities
{
    public class Product
    {
        public Product()
        {
            //OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        //public Category Category { get; set; }
        //public Supplier Supplier { get; set; }
        //public ICollection<OrderDetail> OrderDetails { get; private set; }
    }
}
