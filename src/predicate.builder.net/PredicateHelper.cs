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

            Type type = typeof(T);
            PropertyInfo prop = GetProperty(propName, type);
            object rightValueTyped = default(object);
            Expression leftExpression = default(Expression);

            if (propName == string.Intern("@"))
            {
                rightValueTyped = Convert.ChangeType(rightValue, type);
                leftExpression = t;
            }
            else
            {
                var nestedProps = propName.Split(new string[] {
                    string.Intern(".")
                }, StringSplitOptions.RemoveEmptyEntries);

                if (nestedProps.Count() > 1)
                {
                    leftExpression = t;
                    foreach (var member in nestedProps)
                    {
                        leftExpression = Expression.PropertyOrField(leftExpression, member);
                    }

                    Type lastMemberType = typeof(T);


                    for (int i = 0; i < nestedProps.Length; i++)
                    {
                        string nestedPropname = nestedProps.Skip(i).First();
                        lastMemberType = GetProperty(nestedPropname, lastMemberType).PropertyType;
                    }

                    rightValueTyped = Convert.ChangeType(rightValue, lastMemberType);
                }
                else
                {
                    if (rightValue == string.Intern("null"))
                    {
                        rightValueTyped = null;
                    }
                    else
                    {

                        rightValueTyped = Convert.ChangeType(rightValue, prop.PropertyType);

                    }

                    leftExpression = Expression.PropertyOrField(t, propName);
                }
            }

            ConstantExpression rightExpression = Expression.Constant(rightValueTyped);

            switch (@operator)
            {
                case "=": return Expression.Equal(leftExpression, rightExpression);
                case "!=": return Expression.NotEqual(leftExpression, rightExpression);
                case "<": return Expression.LessThan(leftExpression, rightExpression);
                case "<=": return Expression.LessThanOrEqual(leftExpression, rightExpression);
                case ">": return Expression.GreaterThan(leftExpression, rightExpression);
                case ">=": return Expression.GreaterThanOrEqual(leftExpression, rightExpression);
                default:
                    throw new NotImplementedException();
                    break;
            }

            throw new NotImplementedException();
        }

        static LambdaExpression CreateExpression(string propertyName, Type type)
        {
            var param = Expression.Parameter(type, "x");
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            return Expression.Lambda(body, param);
        }

        private static PropertyInfo GetProperty(string propName, Type type)
        {
            return type.GetProperty(propName);
        }

        /// <summary>
        /// Split AND commands
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static IEnumerable<string> AndBranches(string command)
        {
            string AND = string.Intern("AND");

            var result = command
                .Split(new string[] { AND }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());
            return result;
        }
    }
}
