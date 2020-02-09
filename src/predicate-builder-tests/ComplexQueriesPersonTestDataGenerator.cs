using predicate_builder_tests.Models;
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

        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
