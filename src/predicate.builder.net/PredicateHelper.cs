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
            Expression combinedExpression;

            if (childExpressions.Any())
            {
                combinedExpression = childExpressions.Aggregate(Expression.AndAlso);
            }
            else
            {
                combinedExpression = Expression.Constant(true, typeof(bool));
            }

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


        static Expression ExpressionFactory<T>(IEnumerable<string> tokens, ParameterExpression parameter)
        {
            // TODO Support for two tokens commands like `x IsNull` or `x IsNotNull`
            var (propName, @operator, rightValue) = new ValueTuple<string, string, string>(tokens.ElementAt(0),
                tokens.ElementAt(1), tokens.ElementAt(2));

            Type parameterType = typeof(T);
            object rightValueTyped = default(object);
            Type rightValueMemberType = parameterType;

            Expression leftExpression = default(Expression);

            if (propName == string.Intern("@"))
            {
                rightValueTyped = Convert.ChangeType(rightValue, rightValueMemberType);
                leftExpression = parameter;
            }
            else
            {
                var nestedProps = propName.Split(new string[] {
                    string.Intern(".")
                }, StringSplitOptions.RemoveEmptyEntries);

                if (nestedProps.Count() > 1)
                {
                    leftExpression = parameter;
                    Type leftExpressionType = parameterType;
                    foreach (string member in nestedProps)
                    {                       
                        leftExpression = Expression.PropertyOrField(leftExpression, member);
                    }

                    for (int i = 0; i < nestedProps.Length; i++)
                    {
                        string nestedPropname = nestedProps.Skip(i).First();
                        rightValueMemberType = GetProperty(nestedPropname, rightValueMemberType).PropertyType;
                    }

                    rightValueTyped = Convert.ChangeType(rightValue, rightValueMemberType);
                }
                else
                {
                    rightValueMemberType = GetProperty(propName, parameterType).PropertyType;

                    if (rightValue == string.Intern("null"))
                    {
                        rightValueTyped = null;
                    }
                    else
                    {
                        rightValueTyped = Convert.ChangeType(rightValue, rightValueMemberType);
                    }

                    leftExpression = Expression.PropertyOrField(parameter, propName);
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

        private static LambdaExpression CreateExpression(string propertyName, Type type)
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

        private static bool HasProperty(string propName, Type type)
        {
            return GetProperty(propName, type) != null;
        }

        private static bool HasProperty<T>(string propName)
        {
            return GetProperty(propName, typeof(T)) != null;
        }

        private static bool IsNullable(Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }


        private static bool IsNullable<T>()
        {
            return typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Split AND commands
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static IEnumerable<string> AndBranches(string command)
        {
            string AND = string.Intern("AND");
            command = command ?? string.Empty;

            var result = command
                .Split(new string[] { AND }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim());
            return result;
        }
    }
}
