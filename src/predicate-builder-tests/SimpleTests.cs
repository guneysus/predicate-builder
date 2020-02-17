using predicate.builder.net;
using predicate_builder_tests.Models;
using System;
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
            Func<Person, bool> predicate = PredicateHelper.Create<Person>(command);
            Assert.Equal(expected, predicate(person));
        }


        [Fact(DisplayName = "Null queries")]
        public void Null_Queries()
        {
            Assert.True(PredicateHelper.Create<Person>("Name = null")(Person.New(Name: null)));
            Assert.True(PredicateHelper.Create<Person>("Name != null")(Person.New(Name: "Ahmed")));
            Assert.False(PredicateHelper.Create<Person>("Name != null")(Person.New(Name: null)));
        }

        [Fact(DisplayName = "String Tests")]
        public void String_Tests()
        {
            Assert.True(PredicateHelper.Create<Person>("Name = Ahmed Þeref")(Person.New(Name: "Ahmed Þeref")));
        }

        [Fact]
        public void Defense_Null_And_Empty ()
        {
            Assert.True(PredicateHelper.Create<Person>(null)(Person.New()));
            Assert.True(PredicateHelper.Create<Person>("")(Person.New()));
        }

    }
}
