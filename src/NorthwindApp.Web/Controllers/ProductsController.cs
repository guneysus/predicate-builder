using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Northwind.Domain.Entities;
using predicate.builder.net;

namespace NorthwindApp.Web.Controllers
{
    public class ProductController : BaseController<Product> { }
    public class CategoryController : BaseController<Category> { }

    public class CustomerController : BaseController<Customer> { }

    public class EmployeeController : BaseController<Employee> { }
    public class EmployeeTerritoryController : BaseController<EmployeeTerritory> { }
    public class OrderController : BaseController<Order> { }
    public class OrderDetailController : BaseController<OrderDetail> { }
    public class RegionController : BaseController<Region> { }
    public class ShipperController : BaseController<Shipper> { }
    public class SupplierController : BaseController<Supplier> { }
    public class TerritoryController : BaseController<Territory> { }


    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<T> : ControllerBase where T : class
    {
        // GET api/values
        [HttpGet]
        public virtual IEnumerable<T> Get(string q)
        {
            var query = PredicateHelper.Create<T>(q);

            var db = new NorthwindDbContext();
            var result = db.Set<T>().Where(query).ToList();
            return result;
        }
    }
}
