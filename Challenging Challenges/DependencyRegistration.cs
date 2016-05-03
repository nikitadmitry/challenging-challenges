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
using Business.SearchIndex;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Identity;
using Challenging_Challenges.Infrastructure;
using Microsoft.AspNet.Identity;

namespace Challenging_Challenges
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

            builder.RegisterType(typeof(UserStore)).As(typeof(IUserStore<IdentityUser, Guid>));
            builder.RegisterType(typeof(ApplicationUserManager)).As(typeof(UserManager<IdentityUser, Guid>));
            builder.RegisterType(typeof(AchievementsSignalRProvider)).As(typeof(IAchievementsSignalRProvider));
            builder.RegisterType(typeof(ComplexViewModelsProvider)).As(typeof(IComplexViewModelsProvider));

            RegisterService<IChallengesService>(builder, "ChallengesService");
            RegisterService<IIdentityService>(builder, "IdentityService");
            RegisterService<IAchievementsService>(builder, "AchievementsService");
            RegisterService<ISearchIndexService>(builder, "SearchIndexService");

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }

        private static void RegisterService<T>(ContainerBuilder builder, string serviceName)
        {
            var businessAddress = ConfigurationValuesProvider.Get<string>("BusinessAddress");

            builder
                .Register(c => new ChannelFactory<T>(
                    new BasicHttpBinding(),
                    new EndpointAddress($"{businessAddress}{serviceName}.svc")))
                .SingleInstance();

            builder
                .Register(c => c.Resolve<ChannelFactory<T>>().CreateChannel())
                .As<T>()
                .UseWcfSafeRelease();
        }

        private static void RegisterChallengesService(ContainerBuilder builder, string businessAddress)
        {
            builder
                .Register(c => new ChannelFactory<IChallengesService>(
                    new BasicHttpBinding(),
                    new EndpointAddress($"{businessAddress}ChallengesService.svc")))
                .SingleInstance();

            builder
                .Register(c => c.Resolve<ChannelFactory<IChallengesService>>().CreateChannel())
                .As<IChallengesService>()
                .UseWcfSafeRelease();
        }

        private static void RegisterIdentityService(ContainerBuilder builder, string businessAddress)
        {
            builder
                .Register(c => new ChannelFactory<IIdentityService>(
                    new BasicHttpBinding(),
                    new EndpointAddress($"{businessAddress}IdentityService.svc")))
                .SingleInstance();

            builder
                .Register(c => c.Resolve<ChannelFactory<IIdentityService>>().CreateChannel())
                .As<IIdentityService>()
                .UseWcfSafeRelease();
        }

        private static void RegisterAchievementsService(ContainerBuilder builder, string businessAddress)
        {
            builder
                .Register(c => new ChannelFactory<IAchievementsService>(
                    new BasicHttpBinding(),
                    new EndpointAddress($"{businessAddress}AchievementsService.svc")))
                .SingleInstance();

            builder
                .Register(c => c.Resolve<ChannelFactory<IAchievementsService>>().CreateChannel())
                .As<IAchievementsService>()
                .UseWcfSafeRelease();
        }
    }
}