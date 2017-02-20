using System;
using Business.Challenges.ViewModels;
using Data.Challenges.Entities;
using Shared.Framework.Dependency;

namespace Business.Challenges.Private
{
    public interface IChallengeSolvingStrategy : IDependency
    {
        ChallengeSolveResult Solve(Challenge challenge, Guid userId, string answer);
    }
}