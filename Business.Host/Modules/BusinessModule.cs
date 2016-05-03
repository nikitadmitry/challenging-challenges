using Autofac;
using Business.Achievements;
using Business.Challenges;
using Business.Identity;
using Business.SearchIndex;

namespace Business.Host.Modules
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ChallengesService)).As(typeof(IChallengesService))/*.InstancePerRequest()*/;
            builder.RegisterType(typeof(IdentityService)).As(typeof(IIdentityService))/*.InstancePerRequest()*/;
            builder.RegisterType(typeof(AchievementsService)).As(typeof(IAchievementsService))/*.InstancePerRequest()*/;
            builder.RegisterType(typeof(SearchIndexService)).As(typeof(ISearchIndexService))/*.InstancePerRequest()*/;

            base.Load(builder);
        }
    }
}