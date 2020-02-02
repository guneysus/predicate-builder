using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

      Expression<Func<T, bool>> expression = BinaryExpressionFactory<T>(tokens);
      Func<T, bool> func = expression.Compile();
      return func;
    }

    static Expression<Func<T, bool>> ExpressionFactory<T>(string command)
    {
      var tokens = Tokens(command);
      Expression<Func<T, bool>> expression = BinaryExpressionFactory<T>(tokens);
      return expression;
    }

    static Expression<Func<T, bool>> BinaryExpressionFactory<T>(IEnumerable<string> tokens)
    {

      if (tokens.Count() != 3)
      {
        throw new IndexOutOfRangeException(nameof(tokens));
      }

      Type paramType = typeof(T);

      var paramName = tokens.First();
      var operatorValue = tokens.Skip(1).First(); // TODO use with reflection
      var rightValue = tokens.Skip(2).First();
      T rightValueTyped = (T)Convert.ChangeType(rightValue, paramType);

      Type exprType = typeof(Expression);

      var binaryExpMethod = exprType.GetMethod(operatorValue,
        BindingFlags.Public | BindingFlags.Static,
        null,
        CallingConventions.Any,
        new Type[] { exprType, exprType },
        null);


      ParameterExpression paramExpr = Expression.Parameter(paramType, paramName);

      ConstantExpression rightExpr = Expression.Constant(rightValueTyped, paramType); // TODO Use overload and cast/parse to correct type

      BinaryExpression binaryExpr = (BinaryExpression)binaryExpMethod.Invoke(null, new object[] { paramExpr, rightExpr }); // TODO Implement a method with Reflection

      Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(binaryExpr, paramExpr);
      return lambda;
    }

    static IEnumerable<string> Tokens(string command)
    {
      return command.Split(' ');
    }
  }

}
