using Autofac;
using Business.Achievements;
using Business.Challenges;
using Business.Identity;

namespace Business.Host.Modules
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(IdentityService)).As(typeof(IIdentityService));
            builder.RegisterType(typeof(AchievementsService)).As(typeof(IAchievementsService));
            
            builder.RegisterModule(new BusinessChallengesModule());

            base.Load(builder);
        }
    }
}