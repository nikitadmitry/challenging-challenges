using System;
using System.Linq;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;

namespace Business.Challenges.Private
{
    internal abstract class ChallengeSolvingStrategyBase : IChallengeSolvingStrategy
    {
        private readonly IChallengesUnitOfWork unitOfWork;

        protected ChallengeSolvingStrategyBase(IChallengesUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ChallengeSolveResult Solve(Challenge challenge, Guid userId, string answer)
        {
            var solveResult = ValidateAnswer(challenge, answer);

            if (solveResult.IsSolved)
            {
                var solver = challenge.Solvers.Single(x => x.UserId == userId);

                solver.HasSolved = true;
                challenge.TimesSolved++;

                unitOfWork.InsertOrUpdate(challenge);
                unitOfWork.InsertOrUpdate(solver);
                unitOfWork.Commit();
            }

            return solveResult;
        }

        protected abstract ChallengeSolveResult ValidateAnswer(Challenge challenge, string answer);
    }
}