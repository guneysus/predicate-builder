using System;
using System.Collections.Generic;
using System.Linq;

namespace predicate_builder_tests.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }

        public decimal Salary { get; set; }
        
        public static Person New(
            int Id = default(int), 
            string Name = default(string), 
            int Age = default(int), 
            DateTime BirthDate =default(DateTime), 
            decimal Salary = default(decimal))
        {
            return new Person()
            {
              Id = Id,
              Age = Age,
              BirthDate = BirthDate,
              Name = Name,
              Salary = Salary
            };
        }
    }

    public static class Db {
        public static IQueryable<Person> People { get; set; } = Db.PeopleList.AsQueryable();
        public static List<Person> PeopleList { get; set; } = new List<Person>();

        static Db() {

        }
    }
}
