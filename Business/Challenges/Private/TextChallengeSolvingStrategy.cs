using System;
using System.Linq;
using Business.Challenges.ViewModels;
using Business.Identity;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Shared.Framework.Dependency;
using Shared.Framework.Resources;

namespace Business.Challenges.Private
{
    [KeyedDependency(ChallengeType.TextAnswered)]
    internal class TextChallengeSolvingStrategy : ChallengeSolvingStrategyBase
    {
        public TextChallengeSolvingStrategy(IChallengesUnitOfWork unitOfWork, IIdentityService identityService)
            : base(unitOfWork, identityService)
        {
        }

        protected override ChallengeSolveResult ValidateAnswer(Challenge challenge, string answer)
        {
            var solveResult = new ChallengeSolveResult
            {
                IsSolved = challenge.Answers.Select(x => x.Value.ToLower())
                    .Any(x => x.Equals(answer.ToLower()))
            };

            if (!solveResult.IsSolved)
            {
                solveResult.ErrorMessage = Localization.AnswerNotValid;
            }

            return solveResult;
        }
    }
}