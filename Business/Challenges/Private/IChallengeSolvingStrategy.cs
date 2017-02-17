using System;
using Business.Challenges.ViewModels;
using Data.Challenges.Entities;

namespace Business.Challenges.Private
{
    public interface IChallengeSolvingStrategy
    {
        ChallengeSolveResult Solve(Challenge challenge, Guid userId, string answer);
    }
}