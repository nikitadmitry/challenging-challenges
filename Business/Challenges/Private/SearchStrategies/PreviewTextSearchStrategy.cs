using System;
using System.Linq.Expressions;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;

namespace Business.Challenges.Private.SearchStrategies
{
    public class PreviewTextSearchStrategy : IndexedSearchStrategyBase
    {
        public PreviewTextSearchStrategy(IFullTextIndexedChallengesUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork, mapper)
        {
        }

        protected override Expression<Func<Challenge, string>> PropertyExpression => x => x.PreviewText;

        protected override ChallengeSearchType SearchType => ChallengeSearchType.PreviewText;
    }
}