using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Shared.Framework.Utilities
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns collection of possible values of enum.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        public static IEnumerable<T> GetValues<T>() 
            where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Gets description of the enum value
        /// http://stackoverflow.com/questions/12022557/get-a-liststring-of-my-enum-attributes-with-a-generic-method
        /// </summary>
        /// <param name="val">Enum value</param>
        /// <returns>Description</returns>
        public static string GetDescription(Enum val)
        {
            if (val.GetAttribute<DescriptionAttribute>() != null)
                return val.GetAttribute<DescriptionAttribute>().Description;

            return val.ToString();
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}