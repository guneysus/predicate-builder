using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace predicate.builder.net
{
    public static class PredicateHelper
    {
        private static string PARAMETER_NAME = string.Intern("t");

        public static Func<T, bool> Create<T>(string command)
        {
            ParameterExpression parameters = Expression.Parameter(typeof(T), PARAMETER_NAME);
            IEnumerable<string> andBranches = AndBranches(command);
            IEnumerable<Expression> childExpressions = ChildExpressions<T>(andBranches, parameters);
            BinaryExpression combinedExpression = (BinaryExpression)childExpressions.Aggregate(Expression.AndAlso);
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(combinedExpression, parameters);
            return expression.Compile();
        }


        /// <summary>
        /// Split input into tokens
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static IEnumerable<string> Tokens(string command) => command.Split();

        static IEnumerable<Expression> ChildExpressions<T>(IEnumerable<string> commands, ParameterExpression t)
        {
            foreach (var subcommand in commands)
            {
                var tokens = Tokens(subcommand);
                yield return ExpressionFactory<T>(tokens, t);
            }
        }


        static Expression ExpressionFactory<T>(IEnumerable<string> tokens, ParameterExpression t)
        {
            // TODO Support for two tokens commands like `x IsNull` or `x IsNotNull`
            var (propName, @operator, rightValue) = new ValueTuple<string, string, string>(tokens.ElementAt(0),
                tokens.ElementAt(1), tokens.ElementAt(2));

            PropertyInfo prop = typeof(T).GetProperty(propName); // TODO GetMember, GetField or GetProperty
            object rightValueTyped = Convert.ChangeType(rightValue, prop.PropertyType);
            MemberExpression memberExpr = Expression.PropertyOrField(t, propName);
            ConstantExpression right = Expression.Constant(rightValueTyped);

            switch (@operator)
            {
                case "=": return Expression.Equal(memberExpr, right);
                case "!=": return Expression.NotEqual(memberExpr, right);
                case "<": return Expression.LessThan(memberExpr, right);
                case "<=": return Expression.LessThanOrEqual(memberExpr, right);
                case ">": return Expression.GreaterThan(memberExpr, right);
                case ">=": return Expression.GreaterThanOrEqual(memberExpr, right);
                default:
                    throw new NotImplementedException();
                    break;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Split AND commands
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static IEnumerable<string> AndBranches(string command)
        {
            string AND = string.Intern("AND");

            return command.Split(new string[] { AND }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
