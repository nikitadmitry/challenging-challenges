using Autofac;
using Data.Challenges.Context;
using Data.Common;
using Data.Identity.Context;

namespace Business.Host.Modules
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork));/*.InstancePerRequest();*/

            builder.RegisterType(typeof(ChallengesUnitOfWork)).As(typeof(IChallengesUnitOfWork))/*.InstancePerRequest()*/;
            builder.RegisterType(typeof(IdentityUnitOfWork)).As(typeof(IIdentityUnitOfWork))/*.InstancePerRequest()*/;

            base.Load(builder);
        }
    }
}