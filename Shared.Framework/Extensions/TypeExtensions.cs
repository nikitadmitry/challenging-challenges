using System;
using System.Linq;

namespace Shared.Framework.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks whether <paramref name="type"/> has injection of <paramref name="injectedType"/>.
        /// 'Injection' means this type has property or parameter in constructor
        /// typeof <paramref name="injectedType"/> (or generic with typeof <paramref name="injectedType"/>).
        /// </summary>
        public static bool HasInjectionOf(this Type type, Type injectedType)
        {
            return type.HasConstructorInjectionOf(injectedType) || type.HasPropertyInjectionOf(injectedType);
        }

        /// <summary>
        /// Checks whether <paramref name="type"/> has injection of <paramref name="injectedType"/>
        /// or any of <paramref name="injectedType"/> implementation.
        /// 'Injection' means this type has property or parameter in constructor
        /// typeof <paramref name="injectedType"/> (or generic with typeof <paramref name="injectedType"/>).
        /// </summary>
        public static bool HasInjectionOfTypeOrItsImplementation(this Type type, Type injectedType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(injectedType.IsAssignableFrom)
                .Any(type.HasInjectionOf);
        }

        private static bool HasConstructorInjectionOf(this Type type, Type injectedType)
        {
            return type.GetConstructors()
                .Any(constr => constr.GetParameters()
                    .Any(parameter => IsTypeOrGenericTypeWithParameterTypeOf(parameter.ParameterType, injectedType)));
        }

        private static bool HasPropertyInjectionOf(this Type type, Type injectedType)
        {
            return type.GetProperties()
                .Any(prop => IsTypeOrGenericTypeWithParameterTypeOf(prop.PropertyType, injectedType));
        }

        private static bool IsTypeOrGenericTypeWithParameterTypeOf(this Type type, Type injectedType)
        {
            return type == injectedType || type.IsGenericType && type.GetGenericArguments().Any(x => x == injectedType);
        }
    }
}