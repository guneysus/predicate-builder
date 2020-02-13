using System;
using System.Linq;
using System.Reflection;

namespace ReflectionHelpers
{
    public static class ReflectionHelpers
    {
        private const BindingFlags BINDING_FLAGS_PROPERTY_OR_FIELD = BindingFlags.Instance
                        | BindingFlags.Public
                        | BindingFlags.IgnoreCase
                        | BindingFlags.GetProperty
                        | BindingFlags.GetField
                        | BindingFlags.Static
            ;

        public static bool HasPropertyOrField<T>(this T obj, string memberName)
        {
            Type type = obj.GetType();

            PropertyInfo propertyInfo = type.GetProperty(memberName, BINDING_FLAGS_PROPERTY_OR_FIELD);
            if (propertyInfo == null)
            {
                var fieldInfo = type.GetField(memberName, BINDING_FLAGS_PROPERTY_OR_FIELD);
                return fieldInfo != null;
            }

            return propertyInfo != null;
        }

        public static bool HasPropertyOrField<T>(string memberName)
        {
            Type type = typeof(T);

            PropertyInfo propertyInfo = type.GetProperty(memberName, BINDING_FLAGS_PROPERTY_OR_FIELD);

            if (propertyInfo == null)
            {
                var fieldInfo = type.GetField(memberName, BINDING_FLAGS_PROPERTY_OR_FIELD);
                return fieldInfo != null;
            }

            return propertyInfo != null;
        }

        public static PropertyInfo GetPropertyInfo<T>(this T obj, string memberName)
        {
            return obj.GetType().GetProperty(memberName, BINDING_FLAGS_PROPERTY_OR_FIELD);
        }

        public static FieldInfo GetFieldInfo<T>(this T obj, string memberName)
        {
            return obj.GetType().GetField(memberName, BINDING_FLAGS_PROPERTY_OR_FIELD);
        }


        public static Func<TReturn> GetMethod<T, TReturn>(string member)
        {
            Predicate<MethodInfo> predicate = new Predicate<MethodInfo>(m => m.Name == member
                && m.ReturnType == typeof(TReturn)
                && !m.GetParameters().Select(p => p.ParameterType).Any()
                );

            MethodInfo methodInfo = typeof(T).GetMethods().FirstOrDefault(x => predicate(x));

            Func<TReturn> func = () =>
            {
                TReturn @return = (TReturn)methodInfo.Invoke(null, new object[] { });
                return @return;
            };

            return func;
        }

        // For static method
        public static Func<T1, TReturn> GetMethod<T, T1, TReturn>(string member)
        {
            Predicate<MethodInfo> predicate = new Predicate<MethodInfo>(m =>
                m.Name == member
                // && m.GetParameters().Select(parameters => parameters.ParameterType).ToArray() == new Type[] { typeof(T1) }.Select(t => t).ToArray()
                && m.ReturnType == typeof(TReturn)
            );


            MethodInfo methodInfo = typeof(T).GetMethods().FirstOrDefault(x => predicate(x));

            Func<T1, TReturn> func = (T1 t1) =>
            {
                TReturn @return = (TReturn)methodInfo.Invoke(null, new object[] { t1 });
                return @return;
            };

            return func;

        }

        public static Func<TReturn> GetMethod<T, TReturn>(this T obj, string member) where T : class
        {
            Predicate<MethodInfo> predicate = new Predicate<MethodInfo>(m => m.Name == member
                && m.ReturnType == typeof(TReturn)
                && !m.GetParameters().Select(p => p.ParameterType).Any()
                );

            MethodInfo methodInfo = typeof(T).GetMethods().FirstOrDefault(x => predicate(x));

            Func<TReturn> func = () =>
            {
                TReturn @return = (TReturn)methodInfo.Invoke(obj, new object[] { });
                return @return;
            };

            return func;
        }

        // For static method
        public static Func<T1, TReturn> ToMethod<T, T1, TReturn>(this T obj, string member) where T : class
        {
            Predicate<MethodInfo> predicate = new Predicate<MethodInfo>(m =>
                m.Name == member
                // && m.GetParameters().Select(parameters => parameters.ParameterType).ToArray() == new Type[] { typeof(T1) }.Select(t => t).ToArray()
                && m.ReturnType == typeof(TReturn)
            );


            MethodInfo methodInfo = typeof(T).GetMethods().FirstOrDefault(x => predicate(x));

            Func<T1, TReturn> func = (T1 t1) =>
            {
                TReturn @return = (TReturn)methodInfo.Invoke(obj, new object[] { t1 });
                return @return;
            };

            return func;

        }
    }
}
