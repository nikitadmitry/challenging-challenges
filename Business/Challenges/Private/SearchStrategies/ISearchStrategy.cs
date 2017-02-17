using System.Collections.Generic;
using Business.Challenges.ViewModels;

namespace Business.Challenges.Private.SearchStrategies
{
    public interface ISearchStrategy
    {
        bool IsApplicable(ChallengeSearchType searchType);

        List<ChallengeInfoViewModel> Search(ChallengesPageRule pageRule);
    }
}