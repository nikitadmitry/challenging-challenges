using System;
using System.ServiceModel;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Business.Achievements;
using Business.Challenges;
using Business.Identity;
using Business.Identity.ViewModels;
using Microsoft.AspNet.Identity;
using Presentation.Legacy.Helpers;
using Presentation.Legacy.Identity;
using Presentation.Legacy.Infrastructure;

namespace Presentation.Legacy
{
    public class DependencyRegistration
    {
        public static IContainer Container
        {
            get;
            set;
        }

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterFilterProvider();

            builder.RegisterSource(new ViewRegistrationSource());

            builder.RegisterType(typeof(IdentityStore))
                .As(typeof(IUserStore<IdentityUser, Guid>))
                .As(typeof(IUserRoleStore<IdentityUser, Guid>));
            builder.RegisterType(typeof(ApplicationUserManager)).As(typeof(UserManager<IdentityUser, Guid>));
            builder.RegisterType(typeof(AchievementsSignalRProvider)).As(typeof(IAchievementsSignalRProvider));
            builder.RegisterType(typeof(ComplexViewModelsProvider)).As(typeof(IComplexViewModelsProvider));

            RegisterService<IChallengesService>(builder, "ChallengesService");
            RegisterService<IIdentityService>(builder, "IdentityService");
            RegisterService<IAchievementsService>(builder, "AchievementsService");

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }

        private static void RegisterService<T>(ContainerBuilder builder, string serviceName)
        {
            var businessAddress = ConfigurationValuesProvider.Get<string>("BusinessAddress");

            builder.Register(c => new ChannelFactory<T>(
                    new BasicHttpBinding(),
                    new EndpointAddress($"{businessAddress}{serviceName}.svc")))
                .SingleInstance();

            builder.Register(c => c.Resolve<ChannelFactory<T>>().CreateChannel())
                .As<T>()
                .UseWcfSafeRelease();
        }
    }
}