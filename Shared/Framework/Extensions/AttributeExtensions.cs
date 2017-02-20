using System.Linq;
using System.Reflection;

namespace Shared.Framework.Extensions
{
    public static class AttributeExtensions
    {
        public static bool HasAttribute<T>(this ICustomAttributeProvider member) where T : class
        {
            return member.GetAttributes<T>().FirstOrDefault() != null;
        }

        public static T[] GetAttributes<T>(this ICustomAttributeProvider member) where T : class
        {
            if (typeof(T) != typeof(object))
            {
                return (T[]) member.GetCustomAttributes(typeof (T), false);
            }

            return (T[]) member.GetCustomAttributes(false);
        }
    }
}