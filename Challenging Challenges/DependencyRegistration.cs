using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Business.Challenges;

namespace Challenging_Challenges
{
    public class DependencyRegistration
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterFilterProvider();

            builder.RegisterSource(new ViewRegistrationSource());

            builder
              .Register(c => new ChannelFactory<IChallengesService>(
                new BasicHttpBinding(),
                new EndpointAddress("http://localhost:64242/ChallengesService.svc")))
              .SingleInstance();

            builder
              .Register(c => c.Resolve<ChannelFactory<IChallengesService>>().CreateChannel())
              .As<IChallengesService>()
              .UseWcfSafeRelease();


            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static IEnumerable<Assembly> BusinessAssemblies()
        {
            yield return typeof(Business.Challenges.ChallengesService).Assembly;
        }
    }
}