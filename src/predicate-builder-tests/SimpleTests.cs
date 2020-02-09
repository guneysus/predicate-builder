using predicate.builder.net;
using predicate_builder_tests.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace predicate_builder_tests
{
    public class LinqLikeQueries
    {
        private readonly ITestOutputHelper _output;

        public LinqLikeQueries(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(SimpleQueriesPersonTestDataGenerator))]
        public void Simple_Queries(string command, Person person, bool expected)
        {
            Assert.Equal(expected, PredicateHelper.Create<Person>(command)(person));

        }

        [Theory]
        [ClassData(typeof(ComplexQueriesPersonTestDataGenerator))]
        public void Complex_Queries(string command, Person person, bool expected)
        {
            Assert.Equal(expected, PredicateHelper.Create<Person>(command)(person));
        }

    }

    internal class ComplexQueriesPersonTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> data = new List<object[]>()
        {
            new object[] { "Id = 10 AND Age = 30", Person.New(Id: 10, Age : 30), true },
            new object[] { "Id != 0 AND Age >= 18", Person.New(Id: 10, Age : 18), true },
            new object[] { "Id != 0 AND Age >= 18", Person.New(Id: 10, Age : 19), true },
            new object[] { "Id != 0 AND Age >= 18", Person.New(Id: 10, Age : 17), false },

        };

        public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
