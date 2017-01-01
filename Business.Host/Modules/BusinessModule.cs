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
            builder.RegisterType(typeof(IdentityService)).As(typeof(IIdentityService));
            builder.RegisterType(typeof(AchievementsService)).As(typeof(IAchievementsService));
            builder.RegisterType(typeof(SearchIndexService)).As(typeof(ISearchIndexService));
            
            builder.RegisterModule(new BusinessChallengesModule());

            base.Load(builder);
        }
    }
}