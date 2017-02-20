using Autofac;
using Autofac.Integration.Wcf;

namespace Business.Host
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            InitializeRootContainer();
        }

        private void InitializeRootContainer()
        {
            var containerBuilder = ContainerConfig.BuildContainer(new ContainerBuilder());
 
            IContainer container = containerBuilder.Build();

            AutofacHostFactory.Container = container;
        }
    }
}
