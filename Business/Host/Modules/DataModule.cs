using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Util;
using Data.Challenges.Context;
using Data.Common;
using Module = Autofac.Module;

namespace Business.Host.Modules
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAssemblies = Assemblies.Data().ToArray();

            RegisterRepositories(builder, dataAssemblies);
            RegisterUnitOfWorks(builder, dataAssemblies);
            RegisterContexts(builder, dataAssemblies);

            base.Load(builder);
        }

        private void RegisterContexts(ContainerBuilder builder, Assembly[] dataAssemblies)
        {
            builder.RegisterAssemblyTypes(dataAssemblies)
                .Where(t => t.Name.EndsWith("Context", StringComparison.Ordinal))
                .AsSelf()
                .InstancePerLifetimeScope();
        }

        private void RegisterRepositories(ContainerBuilder builder, Assembly[] dataAssemblies)
        {
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

            builder.RegisterAssemblyTypes(dataAssemblies)
                .Where(t => t.Name.EndsWith("Repository", StringComparison.Ordinal))
                .AsImplementedInterfaces();
        }

        private void RegisterUnitOfWorks(ContainerBuilder builder, Assembly[] dataAssemblies)
        {
            dataAssemblies.SelectMany( a => a.GetLoadableTypes())
                .Where(t => t.IsAssignableTo<IUnitOfWork>()).ToList()
                .ForEach(t => RegisterUnitOfWork(t, builder));

            builder.RegisterType<ChallengesUnitOfWork>()
                .WithParameter(new ResolvedParameter(
                   (pi, ctx) => pi.ParameterType == typeof(ChallengesContext),
                   (pi, ctx) => ctx.Resolve<FullTextIndexedChallengesContext>()))
                .As(typeof(IFullTextIndexedChallengesUnitOfWork));
        }

        private void RegisterUnitOfWork(Type type, ContainerBuilder builder)
        {
            builder.RegisterTypes(type)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}