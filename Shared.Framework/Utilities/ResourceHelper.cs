using System;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Represents resource helper
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Returns resource key by type and property name
        /// </summary>
        /// <param name="containerType">Type</param>
        /// <param name="propertyName">Property name</param>
        /// <returns></returns>
        public static string GetResourceKey(Type containerType, string propertyName)
        {
            return containerType.Name + "_" + propertyName;
        }
    }
}
