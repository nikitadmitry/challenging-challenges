using System;
using Autofac.Features.Indexed;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Shared.Framework.Dependency;
using Shared.Framework.Validation;

namespace Business.Challenges.Private
{
    internal class ChallengeSolutionDispatcher : IChallengeSolutionDispatcher, IDependency
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IIndex<ChallengeType, Lazy<IChallengeSolvingStrategy>> challengeSolvingStrategies;

        public ChallengeSolutionDispatcher(
            IChallengesUnitOfWork unitOfWork,
            IIndex<ChallengeType, Lazy<IChallengeSolvingStrategy>> challengeSolvingStrategies)
        {
            this.unitOfWork = unitOfWork;
            this.challengeSolvingStrategies = challengeSolvingStrategies;
        }

        public ChallengeSolveResult Solve(Guid challengeId, Guid userId, string answer)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var challengeType = (ChallengeType) challenge.ChallengeType;

            return challengeSolvingStrategies[challengeType].Value.Solve(challenge, userId, answer);
        }
    }
}