using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using Autofac.Util;
using Shared.Framework.Extensions;

namespace Shared.Framework.Dependency
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterDefaultDependencies(this ContainerBuilder builder, params Assembly[] assemblies)
        {
            builder.RegisterDefaultDependencies((IEnumerable<Assembly>)assemblies);
        }

        public static void RegisterDefaultDependencies(this ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            foreach (var type in assemblies.SelectMany(a => a.GetLoadableTypes()))
            {
                RegisterDefaultDependency(type, builder);
            }
        }

        private static void RegisterDefaultDependency(Type type, ContainerBuilder builder)
        {
            if (!type.IsAssignableTo<IDependency>())
            {
                return;
            }
            
            RegisterAsInterfaceImplementation(type, builder);

            RegisterAsSelf(type, builder);
        }

        private static void RegisterAsSelf(Type type, ContainerBuilder builder)
        {
            var registrationBuilder = builder.RegisterTypes(type);

            registrationBuilder.AsSelf();
        }

        private static void RegisterAsInterfaceImplementation(Type type, ContainerBuilder builder)
        {
            var registrationBuilder = builder.RegisterTypes(type);

            if (type.HasAttribute<KeyedDependencyAttribute>())
            {
                RegisterAsNamed(type, registrationBuilder);
            }
            else
            {
                registrationBuilder.AsImplementedInterfaces();                
            }
        }

        private static void RegisterAsNamed(Type type, 
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registrationBuilder)
        {
            foreach (var attribute in type.GetAttributes<KeyedDependencyAttribute>())
            {
                foreach (var registeredInterface in type.GetInterfaces())
                {
                    registrationBuilder.Keyed(attribute.Key, registeredInterface);
                }
            }
        }
    }
}