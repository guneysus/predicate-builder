using System;
using Xunit;
using ReflectionHelpers;

namespace ReflectionHelpers.Tests
{
    public class ReflectionHelpersTests
    {
        [Fact]
        public void Simple_Tests()
        {
            var dt = new DateTime();
            Assert.True(dt.HasPropertyOrField("Year"));
            Assert.True(dt.HasPropertyOrField("year"));

            Assert.True(dt.HasPropertyOrField("DayOfWeek"));
            Assert.True(dt.HasPropertyOrField("dayofweek"));

            Assert.False(dt.HasPropertyOrField("NoneExistingField"));
            Assert.False(dt.HasPropertyOrField("noneexistingfield"));

            Assert.True(ReflectionHelpers.HasPropertyOrField<DateTime>("Year"));
            Assert.True(ReflectionHelpers.HasPropertyOrField<DateTime>("year"));

            Assert.True(ReflectionHelpers.HasPropertyOrField<DateTime>("DayOfWeek"));
            Assert.True(ReflectionHelpers.HasPropertyOrField<DateTime>("dayofweek"));

            Assert.False(ReflectionHelpers.HasPropertyOrField<DateTime>("NoneExistingField"));
            Assert.False(ReflectionHelpers.HasPropertyOrField<DateTime>("noneexistingfield"));

            Assert.True(ReflectionHelpers.HasPropertyOrField<int>("MaxValue"));
        }

        [Fact]
        public void Get_Prop_Info()
        {
            Assert.NotNull(DateTime.Now.GetPropertyInfo("Year"));
            Assert.Null(DateTime.Now.GetPropertyInfo("NoneExistingField"));

            Assert.NotNull(new Foo().GetPropertyInfo("PropInt"));
            Assert.NotNull(new Foo().GetFieldInfo("FieldInt"));

            Assert.Null(new Foo().GetFieldInfo("PropInt"));
            Assert.Null(new Foo().GetPropertyInfo("FieldInt"));
        }

        [Fact]
        public void Get_Method()
        {
            var parse = ReflectionHelpers.GetMethod<Foo, string, int>("StaticParse");
            var getMinValue = ReflectionHelpers.GetMethod<Foo, int>("StaticGetMinValue");
            Assert.Equal(1, parse("1"));
            Assert.Equal(int.MinValue, getMinValue());

            var instance = new Foo();
            var instanceInt = instance.GetMethod<Foo, int>("InstanceInt");

            Assert.Equal(instance.GetHashCode(), instanceInt());
        }
    }

    public class Foo
    {
        public int FieldInt;
        public int PropInt { get; set; }

        public static int StaticParse(string v) => int.Parse(v);
        public static int StaticGetMinValue() => int.MinValue;

        public int InstanceInt()
        {
            return this.GetHashCode();
        }

    }
}
