using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
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
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork));

            builder.RegisterType(typeof(ChallengesContext)).AsSelf().InstancePerLifetimeScope();
            builder.RegisterType(typeof(FullTextIndexedChallengesContext)).AsSelf().InstancePerLifetimeScope();

            builder.RegisterType(typeof(ChallengesUnitOfWork)).As(typeof(IChallengesUnitOfWork))
                .InstancePerLifetimeScope();
            builder.RegisterType<ChallengesUnitOfWork>()
                .WithParameter(new ResolvedParameter(
                   (pi, ctx) => pi.ParameterType == typeof(ChallengesContext),
                   (pi, ctx) => ctx.Resolve<FullTextIndexedChallengesContext>()))
                .As(typeof(IFullTextIndexedChallengesUnitOfWork))
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof(IdentityUnitOfWork)).As(typeof(IIdentityUnitOfWork))
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}