using predicate_builder_tests.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace predicate_builder_tests
{
    internal class ComplexQueriesPersonTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>()
        {
            new object[] { "Id = 10 AND Age = 30", Person.New(Id: 10, Age : 30), true },
            new object[] { "Id != 0 AND Age >= 18", Person.New(Id: 10, Age : 18), true },
            new object[] { "Id != 0 AND Age >= 18", Person.New(Id: 10, Age : 19), true },
            new object[] { "Id != 0 AND Age >= 18", Person.New(Id: 10, Age : 17), false },
            new object[] { "Age >= 18 AND Age <= 30", Person.New(Age : 18), true },
            new object[] { "Age >= 18 AND Age <= 30", Person.New(Age : 30), true },
            new object[] { "Age >= 18 AND Age <= 30", Person.New(Age : 17), false },
            new object[] { "Age >= 18 AND Age <= 30", Person.New(Age : 31), false },
            new object[] { "Birthdate.Year = 2000", Person.New(Birthdate: new DateTime(2000, 1,1)), true },
            new object[] { "Birthdate.Year <= 1989", Person.New(Birthdate: new DateTime(1989, 1,1)), true },
            new object[] { "Birthdate.Year <= 1989", Person.New(Birthdate: new DateTime(1988, 1,1)), true },
            new object[] { "Birthdate.Year <= 1989", Person.New(Birthdate: new DateTime(1990, 1,1)), false },
            new object[] { "Birthdate.Month = 8", Person.New(Birthdate: new DateTime(2000, 8 ,1)), true },
            new object[] { "Birthdate.Month = 8 AND Birthdate.Year = 2000", Person.New(Birthdate: new DateTime(2000, 8 ,1)), true },
            new object[] { "Birthdate.Month = 8 AND Birthdate.Year = 2000", Person.New(Birthdate: new DateTime(2001, 8 ,1)), false },
            new object[] { "Birthdate.Month = 8 AND Birthdate.Year = 2000 AND Age = 30", Person.New(Birthdate: new DateTime(2000, 8 ,1), Age: 30), true },
            new object[] { "Birthdate.Month = 8 AND Birthdate.Year = 2000 AND Age = 31", Person.New(Birthdate: new DateTime(2000, 8 ,1), Age: 30), false },

        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
