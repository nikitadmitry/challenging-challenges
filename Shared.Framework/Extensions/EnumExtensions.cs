using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Shared.Framework.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Enum"/>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Checks if the value contains the provided type.
        /// </summary>
        public static bool Has<T>(this T type, T value) where T : struct, IConvertible
        {
            Contract.Requires<ArgumentException>(typeof(T).IsEnum);
            bool result = (((int)(object)type & (int)(object)value) == (int)(object)value);
            return result;
        }

        /// <summary>
        /// Checks if the value is only the provided type
        /// </summary>
        public static bool Is<T>(this T type, T value) where T : struct, IConvertible
        {
            Contract.Requires<ArgumentException>(typeof(T).IsEnum);
            bool result = (int)(object)type == (int)(object)value;
            return result;
        }

        /// <summary>
        /// Appends a flag to enum.
        /// </summary>
        public static T Add<T>(this T type, T value) where T : struct, IConvertible
        {
            Contract.Requires<ArgumentException>(typeof(T).IsEnum);
            var result = (T)(object)(((int)(object)type | (int)(object)value));
            return result;
        }


        /// <summary>
        /// Completely removes the value.
        /// </summary>
        public static T Remove<T>(this T type, T value) where T : struct, IConvertible
        {
            Contract.Requires<ArgumentException>(typeof(T).IsEnum);
            var result = (T)(object)(((int)(object)type & ~(int)(object)value));
            return result;
        }

        /// <summary>
        /// Combines separate flags into one enum value.
        /// </summary>
        public static T Combine<T>(this IEnumerable<T> values) where T : struct, IConvertible
        {
            Contract.Requires<ArgumentException>(typeof(T).IsEnum);
            var result = values.Aggregate((T)(object)0, (current, resourceAction) => current.Add(resourceAction));
            return result;
        }

        /// <summary>
        /// Checks enum collection and typing prior to parsing enum
        /// </summary>
        public static T ConvertToEnum<T>(this string value) where T : struct, IConvertible
        {
            Contract.Requires<ArgumentException>(typeof(T).IsEnum);
            Contract.Requires<ArgumentException>(value != null);
            Contract.Requires<ArgumentException>(Enum.IsDefined(typeof(T), value));
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Gets the value description of the Description attribute from Enum member.
        /// </summary>
        public static string GetDescription<T>(this T enumValue) where T : struct
        {
            Contract.Requires<ArgumentException>(typeof(T).IsEnum);

            var descriptionAttribute = enumValue.GetType().GetField(enumValue.ToString()).First<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description ?? enumValue.ToString();
            }
            return enumValue.ToString();
        }

        public static string ResolveEnumDescriptionAttributeValue(int intValue, Type enumType)
        {
            foreach (var enumValue in Enum.GetValues(enumType))
            {
                if (intValue == (int)enumValue)
                {
                    var descriptionAttribute = enumType.GetField(enumValue.ToString()).First<DescriptionAttribute>();
                    return descriptionAttribute == null ? string.Empty : descriptionAttribute.Description;
                }
            }
            return string.Empty;
        }
    }
}