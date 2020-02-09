using predicate.builder.net;
using predicate_builder_tests.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace predicate_builder_tests
{
    public class SimpleTests
    {
        private readonly ITestOutputHelper _output;

        public SimpleTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(SimpleQueriesPersonTestDataGenerator))]
        public void Simple_Queries(string command, Person person, bool expected)
        {
            Assert.Equal(expected, PredicateHelper.Create<Person>(command)(person));

        }

        [Fact]
        public void String_Queries()
        {
            Assert.True(PredicateHelper.Create<string>("Length = 0")(string.Empty));
            Assert.False(PredicateHelper.Create<string>("@ = Ahmed")("ahmed"));
            Assert.True(PredicateHelper.Create<string>("@ = Ahmed")("Ahmed"));
        }


        [Theory]
        [InlineData("@ = 0", 0, true)]
        [InlineData("@ = 3", 3, true)]
        [InlineData("@ = 5", 1, false)]

        [InlineData("@ != 5", 6, true)]
        [InlineData("@ != 10", 11, true)]
        [InlineData("@ != 5", 5, false)]

        [InlineData("@ < 10", 9, true)]
        [InlineData("@ < 0", -1, true)]
        [InlineData("@ < 99", 100, false)]


        [InlineData("@ <= 10", 10, true)]
        [InlineData("@ <= 0", 0, true)]
        [InlineData("@ <= 99", 100, false)]

        [InlineData("@ > 10", 11, true)]
        [InlineData("@ > 0", 1, true)]
        [InlineData("@ > 99", 98, false)]

        [InlineData("@ >= 10", 10, true)]
        [InlineData("@ >= 0", 0, true)]
        [InlineData("@ >= 99", 98, false)]
        public void Integer_Queries(string q, int val, bool expected)
        {
            Assert.Equal(expected, PredicateHelper.Create<int>(q)(val));
        }
    }

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

    internal class TestData<T>
    {
        public bool Expected { get; set; }
        public string Command { get; set; }
        public T Item { get; set; }

        public static TestData<T> New(string command, T item, bool expected)
        {
            return new TestData<T>()
            {
                Command = command,
                Item = item,
                Expected = expected
            };
        }
    }
}
