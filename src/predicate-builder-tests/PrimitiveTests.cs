using predicate.builder.net;
using System;
using Xunit;

namespace predicate_builder_tests
{
    public class PrimitiveTests
    {

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


        [Fact]
        public void Datetime_Tests()
        {
            var foo = new Foo()
            {
                DateTime = new DateTime(2000, 12, 31),
            };

            Assert.True(PredicateHelper.Create<Foo>("DateTime.Year = 2000")(foo));
            Assert.True(PredicateHelper.Create<Foo>("NullableDateTimeNull = null")(foo));
            Assert.True(PredicateHelper.Create<Foo>("NullableDateTimeHasValue.Year = 2000")(foo));
        }

        class Foo
        {
            public DateTime DateTime { get; set; }
            public DateTime? NullableDateTimeNull => null;
            public DateTime? NullableDateTimeHasValue => DateTime;
        }

    }
}
