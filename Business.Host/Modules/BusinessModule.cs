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
            builder.RegisterType(typeof(ChallengesService)).As(typeof(IChallengesService))/*.InstancePerRequest()*/;
            builder.RegisterType(typeof(IdentityService)).As(typeof(IIdentityService))/*.InstancePerRequest()*/;
            builder.RegisterType(typeof(AchievementsService)).As(typeof(IAchievementsService))/*.InstancePerRequest()*/;

            base.Load(builder);
        }
    }
}