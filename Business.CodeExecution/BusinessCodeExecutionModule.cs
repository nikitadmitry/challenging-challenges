using Autofac;

namespace Business.CodeExecution
{
    public class BusinessCodeExecutionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CodeExecutor>().As<ICodeExecutor>();

            base.Load(builder);
        }
    }
}