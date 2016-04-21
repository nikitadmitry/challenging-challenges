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
        /// Modify properties (data) of objects. 
        /// This method used if necessary change some fields/properties (include data) for objects.
        /// </summary>
        /// <param name="targetObject">Object which properties (data) will be change</param>
        /// <param name="predicate">Condition for selection properties</param>
        /// <param name="action">Custom action for properties which modify it</param>
        /// <param name="inherit">Allows check and modify inherited objects</param>
        public static void ModifyProperties(object targetObject, Func<PropertyInfo, bool> predicate, Action<object, PropertyInfo> action, bool inherit = false)
        {
            if (targetObject == null)
            {
                return;
            }

            if (CheckIEnumerableInterface(targetObject.GetType()))
            {
                foreach (var item in (IEnumerable) targetObject)
                {
                    ModifyProperties(item, predicate, action, inherit);
                }

                return;
            }

            var objectProperties = targetObject.GetType().GetProperties().ToList();

            var properties = objectProperties.Where(predicate).ToList();

            properties.ForEach(property => action(targetObject, property));

            var innerProperties = objectProperties.Except(properties).Where(property => !property.PropertyType.IsPrimitive &&
                                 property.PropertyType != typeof(string) && 
                                 (property.PropertyType.IsClass || property.PropertyType.IsGenericType || property.PropertyType.IsArray)
                                 ).Select(property => property.GetValue(targetObject, null)).ToList();

            innerProperties.ForEach(propertyValue => ModifyProperties(propertyValue, predicate, action, inherit));
        }
        
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
