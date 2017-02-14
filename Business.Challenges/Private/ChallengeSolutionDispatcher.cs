using System;
using Autofac.Features.Indexed;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Shared.Framework.Validation;

namespace Business.Challenges.Private
{
    internal class ChallengeSolutionDispatcher : IChallengeSolutionDispatcher
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly Lazy<IIndex<ChallengeType, IChallengeSolvingStrategy>> challengeSolvingStrategies;

        public ChallengeSolutionDispatcher(
            IChallengesUnitOfWork unitOfWork,
            Lazy<IIndex<ChallengeType, IChallengeSolvingStrategy>> challengeSolvingStrategies)
        {
            this.unitOfWork = unitOfWork;
            this.challengeSolvingStrategies = challengeSolvingStrategies;
        }

        public ChallengeSolveResult Solve(Guid challengeId, Guid userId, string answer)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var challengeType = (ChallengeType) challenge.ChallengeType;

            return challengeSolvingStrategies.Value[challengeType].Solve(challenge, userId, answer);
        }
    }
}