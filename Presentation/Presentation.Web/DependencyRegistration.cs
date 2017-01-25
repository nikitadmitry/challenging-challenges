﻿using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using Business.Achievements;
using Business.Challenges;
using Business.Identity;
using Business.SearchIndex;
using Microsoft.Extensions.Configuration;

namespace Presentation.Web
{
    public class DependencyRegistration
    {
        public static void ConfigureContainer(ContainerBuilder builder, IConfiguration configuration)
        {
            RegisterService<IChallengesService>(builder, configuration, "ChallengesService");
            RegisterService<IIdentityService>(builder, configuration, "IdentityService");
            RegisterService<IAchievementsService>(builder, configuration, "AchievementsService");
            RegisterService<ISearchIndexService>(builder, configuration, "SearchIndexService");
        }

        private static void RegisterService<T>(ContainerBuilder builder, IConfiguration configuration, string serviceName)
        {
            var businessAddress = configuration["BusinessAddress"];

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