using System;
using Autofac;
using Business.Challenges.Private;
using Business.Challenges.Private.SearchStrategies;
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
            RegisterSearchStrategies(builder);

            base.Load(builder);
        }

        private void RegisterSearchStrategies(ContainerBuilder builder)
        {
            Type[] searchStrategies = {
                typeof(TitleSearchStrategy),
                typeof(PreviewTextSearchStrategy),
                typeof(SectionSearchStrategy),
            };

            foreach (var searchStrategy in searchStrategies)
            {
                builder.RegisterType(searchStrategy).As<ISearchStrategy>();
            }
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