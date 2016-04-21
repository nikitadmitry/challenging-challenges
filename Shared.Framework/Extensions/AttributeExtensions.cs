using System;
using System.Linq;
using System.Reflection;

namespace Shared.Framework.Extensions
{
    /// <summary>
    /// Provides extension methods to work with attributes.
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// Gets the attribute on type or assembly.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The target type.</param>
        /// <returns>
        /// The attribute of type <typeparamref name="TAttribute"/> on the <paramref name="type"/> if found;
        /// the attribute of type <typeparamref name="TAttribute"/> on the assembly containing <paramref name="type"/> otherwise.
        /// </returns>
        public static TAttribute GetAttributeOnTypeOrAssembly<TAttribute>(this Type type) where TAttribute : Attribute
        {
            var attribute = type.First<TAttribute>();
            if (attribute == null)
            {
                attribute = type.Assembly.First<TAttribute>();
            }
            return attribute;
        }

        /// <summary>
        /// Get the first instance of <typeparamref name="TAttribute"/> attribute from the specified attribute provider.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="attributeProvider">The attribute provider.</param>
        /// <returns>The first instance of <typeparamref name="TAttribute"/> attribute if found; <c>null</c> otherwise.</returns>
        public static TAttribute First<TAttribute>(this ICustomAttributeProvider attributeProvider)
            where TAttribute : Attribute
        {
            return attributeProvider.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
        }
    }
}