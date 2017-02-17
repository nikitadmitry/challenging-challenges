using System;
using System.Linq.Expressions;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;

namespace Business.Challenges.Private.SearchStrategies
{
    public class TitleSearchStrategy : IndexedSearchStrategyBase
    {
        public TitleSearchStrategy(IFullTextIndexedChallengesUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork, mapper)
        {
        }

        protected override ChallengeSearchType SearchType => ChallengeSearchType.Title;

        protected override Expression<Func<Challenge, string>> PropertyExpression => x => x.Title;
    }
}