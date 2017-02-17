using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Custom tranformer for objects.
    /// Use if we need change contents of the object.
    /// </summary>
    public static class ObjectTransformer
    {
        /// <summary>
        /// Check object type is implements IEnumerable interface
        /// </summary>
        /// <param name="type">Type of property or field</param>
        public static bool CheckIEnumerableInterface(Type type)
        {
            if (type.IsGenericType)
            {
                return type != typeof(string) && type.GetInterfaces().Any(t => t == typeof(IEnumerable) || t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            }

            return type != typeof(string) && type.GetInterfaces().Any(t => t == typeof(IEnumerable));
        }
    }
}
