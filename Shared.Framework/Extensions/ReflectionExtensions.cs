using System;

namespace Shared.Framework.Extensions
{
    /// <summary>
    /// Provides extension methods for types related to reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Checks whether the property exists in the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the property exists in the type; <c>false</c> otherwise.</returns>
        public static bool PropertyExists(this Type type, string propertyName)
        {
            if (type == null || propertyName == null)
            {
                return false;
            }
            return type.GetProperty(propertyName) != null;
        }
    }
}