using System;
using System.Linq.Expressions;

namespace predicate.builder.net
{
  public class PredicateBuilder
  {
    public static Func<T, bool> Create<T>(string command)
    {

      /*
      x < 5
      */
      ParameterExpression param = Expression.Parameter(typeof(T), "x");
      ConstantExpression right = Expression.Constant(3);
      BinaryExpression exp = Expression.LessThan(param, right);

      Expression<Func<T, bool>> le = Expression.Lambda<Func<T, bool>>(exp, param);
      return le.Compile();

      throw new NotImplementedException();
    }
  }
}
