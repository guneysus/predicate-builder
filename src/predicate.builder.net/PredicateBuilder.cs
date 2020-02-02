﻿using System;
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

      var leftValue = tokens.First();
      var operatorValue = tokens.Skip(1).First();
      var rightValue = tokens.Skip(2).First();


      Type exprType = typeof(Expression);

      // TODO Extract method
      var binaryExpMethod = exprType.GetMethod(operatorValue,
        BindingFlags.Public | BindingFlags.Static,
        null,
        CallingConventions.Any,
        new Type[] { exprType, exprType },
        null);

      if (!leftValue.Contains('.'))
      {
        T rightValueTyped = (T)Convert.ChangeType(rightValue, paramType);

        ParameterExpression parameterExpr = Expression.Parameter(paramType, leftValue);
        ConstantExpression constantExpr = Expression.Constant(rightValueTyped, paramType);

        BinaryExpression binaryExpr = (BinaryExpression)binaryExpMethod.Invoke(null, new object[] { parameterExpr, constantExpr }); // TODO DRY !

        Expression<Func<T, bool>> @delegate = Expression.Lambda<Func<T, bool>>(body: binaryExpr, parameters: parameterExpr); // TODO Extract method
        return @delegate;        
      }
      else
      {

        var leftValueTokens = leftValue.Split('.');

        if(leftValueTokens.Count() != 2) {
          throw new ArgumentOutOfRangeException(); // TODO
        }

        var paramName = leftValueTokens.First();  // v
        var paramPropName = leftValueTokens.Skip(1).First(); // Length like fields or Contains() like methods// TODO check if field or method like Contains(foo)

        int rightValueTyped = (int)Convert.ChangeType(rightValue, typeof(int)); // TODO hardcoded
        ParameterExpression parameterExpression = Expression.Parameter(paramType, paramName);

        MemberExpression memberExpr = Expression.Property(parameterExpression, typeof(string).GetProperty("Length"));  // TODO Hardcoded
        ConstantExpression constantExpr = Expression.Constant(rightValueTyped, typeof(int)); // TODO hardcoded

        BinaryExpression binaryExpr = (BinaryExpression)binaryExpMethod.Invoke(null, new object[] { memberExpr, constantExpr }); // TODO DRY !

        Expression<Func<T, bool>> @delegate = Expression.Lambda<Func<T, bool>>(body: binaryExpr, parameters: parameterExpression); // TODO Extract method

        return @delegate;
      }
    }

    static IEnumerable<string> Tokens(string command)
    {
      return command.Split(' ');
    }
  }

}
