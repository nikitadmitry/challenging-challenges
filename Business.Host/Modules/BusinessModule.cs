using Autofac;
using Business.Challenges;

namespace Business.Host.Modules
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ChallengesService)).As(typeof(IChallengesService)).InstancePerRequest();

            base.Load(builder);
        }
    }
}