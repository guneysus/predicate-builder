using System;
using System.Collections.Generic;
using System.Linq;
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

      var tokens = Tokens(command);

      Expression<Func<T, bool>> expression = CreateLambda<T>(command);
      Func<T, bool> func = expression.Compile();
      return func;
    }

    public static Expression<Func<T, bool>> CreateExpression<T>(string command)
    {
      Expression<Func<T, bool>> expression = CreateLambda<T>(command);
      return expression;
    }


    public static Expression<Func<T, bool>> CreateLambda<T>(string command)
    {
      ParameterExpression param = Expression.Parameter(typeof(T), "x");
      ConstantExpression right = Expression.Constant(3);
      BinaryExpression exp = Expression.LessThan(param, right);

      Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(exp, param);
      return lambda;
    }

    public static Expression<Func<T, bool>> SimpleLambda<T>(IEnumerable<string> tokens) {
      
      if(tokens.Count() != 3) {
        throw new IndexOutOfRangeException(nameof(tokens));
      }

      Type type = typeof(T);

      var paramName = tokens.First();
      var operatorValue = tokens.Skip(1).First();
      var rightValue = tokens.Skip(2).First();

      ParameterExpression param = Expression.Parameter(type, paramName);
      ConstantExpression right = Expression.Constant(rightValue, type);

      BinaryExpression exp = Expression.LessThan(param, right); // TODO Implement a method with Reflection

      Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(exp, param);
      return lambda;      
    }

    static IEnumerable<string> Tokens(string command) {
      return command.Split(' ');
    }
  }

}
