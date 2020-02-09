using predicate.builder.net;
using predicate_builder_tests.Models;
using System;
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

        [Fact]
        public void Simple_Queries()
        {
            Assert.True(PredicateHelper.Create<Person>("Id = 10")(Person.New(Id: 10)));
            Assert.True(PredicateHelper.Create<Person>("Id = 9")(Person.New(Id: 9)));
            Assert.False(PredicateHelper.Create<Person>("Id = 10")(Person.New(Id: 11)));

            Assert.True(PredicateHelper.Create<Person>("Id < 10")(Person.New(Id: 9)));
            Assert.True(PredicateHelper.Create<Person>("Id < 10")(Person.New(Id: 8)));
            Assert.False(PredicateHelper.Create<Person>("Id < 10")(Person.New(Id: 11)));

            Assert.True(PredicateHelper.Create<Person>("Id > 10")(Person.New(Id: 11)));
            Assert.True(PredicateHelper.Create<Person>("Id > 10")(Person.New(Id: 12)));
            Assert.False(PredicateHelper.Create<Person>("Id > 10")(Person.New(Id: 9)));

            Assert.True(PredicateHelper.Create<Person>("Id <= 10")(Person.New(Id: 10)));
            Assert.True(PredicateHelper.Create<Person>("Id <= 10")(Person.New(Id: 9)));
            Assert.False(PredicateHelper.Create<Person>("Id <= 10")(Person.New(Id: 11)));

            Assert.True(PredicateHelper.Create<Person>("Id >= 10")(Person.New(Id: 10)));
            Assert.True(PredicateHelper.Create<Person>("Id >= 10")(Person.New(Id: 11)));
            Assert.False(PredicateHelper.Create<Person>("Id >= 10")(Person.New(Id: 9)));

            Assert.True(PredicateHelper.Create<Person>("Id != 10")(Person.New(Id: 11)));
            Assert.True(PredicateHelper.Create<Person>("Id != 10")(Person.New(Id: 9)));
            Assert.False(PredicateHelper.Create<Person>("Id != 10")(Person.New(Id: 10)));

            Assert.True(PredicateHelper.Create<string>("Length = 0")(string.Empty));
        }
    }
}
