using Autofac;
using Autofac.Integration.Wcf;
using Business.Host.Modules;

namespace Business.Host
{
    public class DependencyRegistration
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new DataModule());
            builder.RegisterModule(new BusinessModule());
            AutoMapperConfig.RegisterMappings(builder);


            AutofacHostFactory.Container = builder.Build();
        }
    }
}