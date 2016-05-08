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