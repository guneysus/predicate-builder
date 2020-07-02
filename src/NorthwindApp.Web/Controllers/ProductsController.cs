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

        public void JoinTest()
        {
            #region MyRegion

            List<Person> people;
            List<Pet> pets;
            FillSampleDb(out people, out pets);

            // Create a list of Person-Pet pairs where
            // each element is an anonymous type that contains a
            // Pet's name and the name of the Person that owns the Pet.

            var __ =
                people.Join(pets,
                            person => person.Id,
                            pet => pet.OwnerId,
                            (person, pet) =>
                                new { OwnerName = person.Name, Pet = pet.Name }
                                // new Dictionary<string, object>()
                            );

            #endregion
        }

        private static void FillSampleDb(out List<Person> people, out List<Pet> pets)
        {
            Person magnus = new Person { Name = "Hedlund, Magnus", Id = 1 };
            Person terry = new Person { Name = "Adams, Terry", Id = 2 };
            Person charlotte = new Person { Name = "Weiss, Charlotte", Id = 3 };

            Pet barley = new Pet { Name = "Barley", Owner = terry };
            Pet boots = new Pet { Name = "Boots", Owner = terry };
            Pet whiskers = new Pet { Name = "Whiskers", Owner = charlotte };
            Pet daisy = new Pet { Name = "Daisy", Owner = magnus };

            people = new List<Person> { magnus, terry, charlotte };
            pets = new List<Pet> { barley, boots, whiskers, daisy };
        }
    }
}

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Pet
{
    public int OwnerId => Owner.Id;

    public string Name { get; set; }
    public Person Owner { get; set; }
}

