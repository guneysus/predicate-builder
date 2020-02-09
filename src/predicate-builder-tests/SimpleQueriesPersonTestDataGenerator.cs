using predicate_builder_tests.Models;
using System.Collections;
using System.Collections.Generic;

namespace predicate_builder_tests
{
    internal class SimpleQueriesPersonTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>()
        {
            new object[] { "Id = 10", Person.New(Id: 10), true },
            new object[] { "Id = 9", Person.New(Id: 9), true },
            new object[] { "Id = 10", Person.New(Id: 11), false },

            new object[] { "Id != 10", Person.New(Id: 11), true },
            new object[] { "Id != 10", Person.New(Id: 9), true },
            new object[] { "Id != 10", Person.New(Id: 10), false },

            new object[] { "Id < 10", Person.New(Id: 9), true },
            new object[] { "Id < 10", Person.New(Id: 8), true },
            new object[] { "Id < 10", Person.New(Id: 11), false },

            new object[] { "Id <= 10", Person.New(Id: 10), true },
            new object[] { "Id <= 10", Person.New(Id: 9), true },
            new object[] { "Id <= 10", Person.New(Id: 11), false },

            new object[] { "Id > 10", Person.New(Id: 11), true },
            new object[] { "Id > 10", Person.New(Id: 12), true },
            new object[] { "Id > 10", Person.New(Id: 9), false },

            new object[] { "Id >= 10", Person.New(Id: 10), true },
            new object[] { "Id >= 10", Person.New(Id: 11), true },
            new object[] { "Id >= 10", Person.New(Id: 9), false }

        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
