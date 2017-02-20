using System.Collections.Generic;
using Business.Challenges.ViewModels;
using Shared.Framework.Dependency;

namespace Business.Challenges.Private.SearchStrategies
{
    public interface ISearchStrategy : IDependency
    {
        List<ChallengeInfoViewModel> Search(ChallengesPageRule pageRule);
    }
}