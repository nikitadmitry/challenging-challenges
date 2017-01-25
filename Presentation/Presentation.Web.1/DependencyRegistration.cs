using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using Business.Achievements;
using Business.Challenges;
using Business.Identity;
using Business.SearchIndex;
using Presentation.Web.Helpers;
using Presentation.Web.Services;

namespace Presentation.Web
{
    public class DependencyRegistration
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            // Add application services.
            builder.RegisterType<AuthMessageSender>().As<ISmsSender>().As<IEmailSender>();


            RegisterService<IChallengesService>(builder, "ChallengesService");
            RegisterService<IIdentityService>(builder, "IdentityService");
            RegisterService<IAchievementsService>(builder, "AchievementsService");
            RegisterService<ISearchIndexService>(builder, "SearchIndexService");
        }

        private static void RegisterService<T>(ContainerBuilder builder, string serviceName)
        {
            var businessAddress = ConfigurationValuesProvider.Get<string>("BusinessAddress");

            builder.Register(c => new ChannelFactory<T>(
                    new WSHttpBinding(),
                    new EndpointAddress($"{businessAddress}{serviceName}.svc")))
                .SingleInstance();

            builder.Register(c => c.Resolve<ChannelFactory<T>>().CreateChannel())
                .As<T>()
                .UseWcfSafeRelease();
        }
    }
}