using AutoMapper;
using Business.Challenges.ViewModels;
using Business.SearchIndex;
using Data.Challenges.Context;

namespace Business.Challenges.Private.SearchStrategies
{
    public class ConditionSearchStrategy : IndexedSearchStrategyBase
    {
        public ConditionSearchStrategy(ISearchIndexService searchIndexService, IChallengesUnitOfWork unitOfWork, 
            IMapper mapper) : base(searchIndexService, unitOfWork, mapper)
        {
        }

        protected override ChallengeSearchType SearchType => ChallengeSearchType.Condition;
    }
}