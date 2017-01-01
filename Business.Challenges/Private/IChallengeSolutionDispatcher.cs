using System;
using Business.Challenges.ViewModels;

namespace Business.Challenges.Private
{
    public interface IChallengeSolutionDispatcher
    {
        ChallengeSolveResult Solve(Guid challengeId, Guid userId, string answer);
    }
}