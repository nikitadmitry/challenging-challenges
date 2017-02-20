using System;
using System.Linq.Expressions;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Shared.Framework.Dependency;

namespace Business.Challenges.Private.SearchStrategies
{
    [KeyedDependency(ChallengeSearchType.Title)]
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