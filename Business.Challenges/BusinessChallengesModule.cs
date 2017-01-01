using Autofac;
using Business.Challenges.Private;
using Business.Challenges.ViewModels;
using Business.CodeExecution;

namespace Business.Challenges
{
    public class BusinessChallengesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChallengesService>().As<IChallengesService>();
            RegisterChallengeSolvingStrategies(builder);
            builder.RegisterModule(new BusinessCodeExecutionModule());
            builder.RegisterType<ChallengeSolutionDispatcher>().As<IChallengeSolutionDispatcher>();

            base.Load(builder);
        }

        private void RegisterChallengeSolvingStrategies(ContainerBuilder builder)
        {
            builder.RegisterType<TextChallengeSolvingStrategy>()
                .Keyed<IChallengeSolvingStrategy>(ChallengeType.TextAnswered);

            builder.RegisterType<TestCaseChallengeSolvingStrategy>()
                .Keyed<IChallengeSolvingStrategy>(ChallengeType.CodeAnswered);
        }
    }
}