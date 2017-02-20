using Autofac;
using Shared.Framework.Dependency;

namespace Business.Host
{
    public class ContainerConfig
    {
        public static ContainerBuilder BuildContainer(ContainerBuilder builder)
        {
            builder.RegisterDefaultDependencies(Assemblies.All());

            foreach (var assembly in Assemblies.Modules())
            {
                builder.RegisterAssemblyModules(assembly);
            }

            AutoMapperConfig.RegisterMappings(builder);
            
            return builder;
        }
    }
}