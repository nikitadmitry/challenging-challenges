using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Business.Challenges;
using Business.Identity;
using Business.Identity.ViewModels;
using Challenging_Challenges.Identity;
using Microsoft.AspNet.Identity;

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

            builder.RegisterType(typeof(UserStore)).As(typeof(IUserStore<IdentityUser, Guid>));
            builder.RegisterType(typeof(ApplicationUserManager)).As(typeof(UserManager<IdentityUser, Guid>));

            RegisterChallengesService(builder);
            RegisterIdentityService(builder);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterChallengesService(ContainerBuilder builder)
        {
            builder
                .Register(c => new ChannelFactory<IChallengesService>(
                    new BasicHttpBinding(),
                    new EndpointAddress("http://localhost:64242/ChallengesService.svc")))
                .SingleInstance();

            builder
                .Register(c => c.Resolve<ChannelFactory<IChallengesService>>().CreateChannel())
                .As<IChallengesService>()
                .UseWcfSafeRelease();
        }

        private static void RegisterIdentityService(ContainerBuilder builder)
        {
            builder
                .Register(c => new ChannelFactory<IIdentityService>(
                    new BasicHttpBinding(),
                    new EndpointAddress("http://localhost:64242/IdentityService.svc")))
                .SingleInstance();

            builder
                .Register(c => c.Resolve<ChannelFactory<IIdentityService>>().CreateChannel())
                .As<IIdentityService>()
                .UseWcfSafeRelease();
        }
    }
}