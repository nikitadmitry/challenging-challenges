using System;
using System.Linq;
using Business.Challenges.ViewModels;
using Business.Identity;
using Data.Challenges.Context;
using Data.Challenges.Entities;

namespace Business.Challenges.Private
{
    internal abstract class ChallengeSolvingStrategyBase : IChallengeSolvingStrategy
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IIdentityService identityService;

        protected ChallengeSolvingStrategyBase(IChallengesUnitOfWork unitOfWork, IIdentityService identityService)
        {
            this.unitOfWork = unitOfWork;
            this.identityService = identityService;
        }

        public ChallengeSolveResult Solve(Challenge challenge, Guid userId, string answer)
        {
            var solveResult = ValidateAnswer(challenge, answer);

            if (solveResult.IsSolved)
            {
                var solver = challenge.Solvers.Single(x => x.UserId == userId);

                solver.HasSolved = true;
                challenge.TimesSolved++;
                var rating = (double)((int)challenge.Difficulty + 1) / challenge.TimesSolved;
                solveResult.RatingObtained = rating;

                identityService.AddRatingToUser(userId, rating);

                unitOfWork.InsertOrUpdate(challenge);
                unitOfWork.InsertOrUpdate(solver);
                unitOfWork.Commit();
            }

            return solveResult;
        }

        protected abstract ChallengeSolveResult ValidateAnswer(Challenge challenge, string answer);
    }
}