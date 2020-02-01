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
    }

    [Fact]
    public void Simple_Predicate_Experiments()
    {
      var list = new List<int>() { 1, 2, 3, 4, 5 };

      var command = "x => x < 3";

      var predicate = PredicateBuilder.Create<int>(command);

      IQueryable<int> queryable = list.AsQueryable();

      var queried = queryable.Where(predicate);

      foreach (var item in queried)
      {
        output.WriteLine(item.ToString());
      }

      Assert.Equal(0, queried.Except(new int[] { 1, 2 }).Count());
    }
  }

}
