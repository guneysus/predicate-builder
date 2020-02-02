using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using predicate.builder.net;
using System.Linq;

namespace predicate.builder.net.tests
{

  public class BasicTests
  {
    ITestOutputHelper output;

    public BasicTests(ITestOutputHelper output)
    {
      this.output = output;
      /*
      var list = new List<int>() { 1, 2, 3, 4, 5 };

      var command = "x => x < 3";

      var predicate = PredicateBuilder.Create<int>(command);

      IQueryable<int> queryable = list.AsQueryable();

      var queried = queryable.Where(predicate);

      Expression<Func<int, bool>> expression = PredicateBuilder.ExpressionFactory<int>(command);
      var q = queryable.Provider.CreateQuery(expression);

      foreach (var item in q)
      {
        output.WriteLine(item.ToString());
      }
      */
    }

    [Theory]
    [InlineData("x Equal 3", 3, 0)]
    [InlineData("x Equal 5", 5, 0)]

    [InlineData("x NotEqual 3", 0, 3)]
    [InlineData("x NotEqual 5", 0, 5)]

    [InlineData("x GreaterThan 3", 4, 0)]
    [InlineData("x GreaterThan 5", 6, 0)]

    [InlineData("x GreaterThanOrEqual 3", 3, 0)]
    [InlineData("x GreaterThanOrEqual 5", 5, 0)]

    [InlineData("x LessThan 3", 2, 99)]
    [InlineData("x LessThan 5", 4, 99)]

    [InlineData("x LessThanOrEqual 3", 3, 99)]
    [InlineData("x LessThanOrEqual 5", 5, 99)]
    public void Int_Equal_Tests(string command, int trueValue, int falseValue)
    {
      var predicate = PredicateBuilder.Create<int>(command);

      Assert.True(predicate(trueValue));
      Assert.False(predicate(falseValue));
    }
  }

}
