using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Class that add useful methods on objects
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Yields the list out of the single item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static IEnumerable<T> YieldEnumerable<T>(this T item)
        {
            yield return item;
        }

        /// <summary>
        /// Checks whether the value equals one of the speciefied.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static bool In<T>(this T value, params T[] values)
        {
            return values.Contains(value);
        }

        /// <summary>
        /// Creates a dictionary from an object using the PropertyName as a key and PropertyValue as a value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj is IDictionary<string, object>)
            {
                return (IDictionary<string, object>) obj;
            }

            if (obj is NameValueCollection)
            {
                var result = new Dictionary<string, object>();
                var nameValues = (NameValueCollection)obj;
                foreach (string nameValue in nameValues)
                {
                    result.Add(nameValue, nameValues[nameValue]);
                }
                return result;
            }


            return TypeDescriptor.GetProperties(obj)
                                 .OfType<PropertyDescriptor>()
                                 .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj));
        }

        public static IList<T> ConvertToList<T>(this T source)
        {
            var list = new List<T>();

            if (!Equals(source, null))
            {
                list.Add(source);
            }

            return list;
        }

        /// <summary>
        /// Creates <see cref="List{T}"/> and adds <paramref name="source"/> as the first element.
        /// If <paramref name="source"/> is null returns null.
        /// </summary>
        public static IEnumerable<T> YieldEnumerableOrNull<T>(this T? source) where T : struct
        {
            if (!source.HasValue)
            {
                return null;
            }
            return YieldEnumerable(source.Value);
        }

        /// <summary>
        /// Helper method to use either the current value of the type or provide new one.
        /// </summary>
        /// <typeparam name="T">Type of the value to use.</typeparam>
        /// <param name="obj">Current value.</param>
        /// <param name="newValue">New value</param>
        /// <returns>
        /// Existing value of <paramref name="obj"/> if it set 
        /// (i.e. does NOT equal default(<typeparamref name="T"/>)),
        /// otherwise <paramref name="newValue"/>.
        /// </returns>
        public static T NewIfNotSet<T>(this T obj, T newValue)
        {
            return Equals(obj, default(T)) ? newValue : obj;
        }

        /// <summary>
        /// Helper method to use either the current value of the type or provide new one.
        /// New value is created only if existing value is not set.
        /// </summary>
        /// <typeparam name="T">Type of the value to use.</typeparam>
        /// <param name="obj">Current value.</param>
        /// <param name="newValueCreator">
        /// A function which will be evaluated to provide new value.
        /// </param>
        /// <returns>
        /// Existing value of <paramref name="obj"/> if it set 
        /// (i.e. does NOT equal default(<typeparamref name="T"/>)),
        /// otherwise a result of the <paramref name="newValueCreator"/>.
        /// </returns>
        public static T NewIfNotSet<T>(this T obj, Func<T> newValueCreator)
        {
            return Equals(obj, default(T)) ? newValueCreator() : obj;
        }

        /// <summary>
        /// Helper method to find out if <paramref name="nullable"/>
        /// has value and the value does not equal default value for 
        /// the type.
        /// </summary>
        /// <typeparam name="T">Type of the <paramref name="nullable"/></typeparam>
        /// <returns>
        /// <c>True</c> if <paramref name="nullable"/>
        /// has value and the value does not equal default value for 
        /// type <typeparamref name="T"/>,
        /// otherwise <c>False</c>
        /// </returns>
        public static bool HasValueAndNotDefault<T>(this T? nullable)
            where T: struct
        {
            return nullable.HasValue && !Equals(nullable.Value, default(T));
        }

        /// <summary>
        /// Converts object to ExpandoObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<String, object> ToExpando<T>(this T obj)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;
            obj.ToDictionary().ToList().ForEach(item => expando[item.Key] = item.Value);
            return expando;
        }

        /// <summary>
        /// Goes through elements in <paramref name="items"/>, casts each element to <typeparamref name="TDestination"/>
        /// and returns collection of <typeparamref name="TDestination"/>.
        /// </summary>
        public static IEnumerable<TDestination> CastEach<TDestination>(this IEnumerable items)
        {
            if (items == null)
            {
                return null;
            }

            return from object item in items select (TDestination)item;
        }

        public static TProperty GetValue<TObject, TProperty>(
            this TObject obj, Expression<Func<TObject, TProperty>> memberExpression)
        {
            var property = (PropertyInfo) GetMemberInfo(memberExpression);

            return (TProperty) property.GetValue(obj);
        }

        public static void SetValue<TObject, TProperty>(
            this TObject obj, Expression<Func<TObject, TProperty>> memberExpression, TProperty value)
        {
            var property = (PropertyInfo) GetMemberInfo(memberExpression);

            property.SetValue(obj, value);
        }

        public static string GetMemberName(
            this LambdaExpression memberExpression)
        {
            return GetMemberInfo(memberExpression).Name;
        }

        private static MemberInfo GetMemberInfo(LambdaExpression memberExpression)
        {
            var memberSelectorExpression =
                memberExpression.Body is UnaryExpression
                    ? ((UnaryExpression) memberExpression.Body).Operand
                    : memberExpression.Body;

            return ((MemberExpression) memberSelectorExpression).Member;
        }

        /// <summary>
        /// Checks whether object is not null.
        /// </summary>
        public static bool IsNotNull(this Object obj)
        {
            return !IsNull(obj);
        }

        /// <summary>
        /// Checks whether object is null.
        /// </summary>
        public static bool IsNull(this Object obj)
        {
            return obj == null;
        }
    }
}