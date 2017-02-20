using System;
using System.Linq.Expressions;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Shared.Framework.Dependency;

namespace Business.Challenges.Private.SearchStrategies
{
    [KeyedDependency(ChallengeSearchType.Condition)]
    public class ConditionSearchStrategy : IndexedSearchStrategyBase
    {
        public ConditionSearchStrategy(IFullTextIndexedChallengesUnitOfWork unitOfWork, IMapper mapper) 
            : base(unitOfWork, mapper)
        {
        }

        protected override ChallengeSearchType SearchType => ChallengeSearchType.Condition;

        protected override Expression<Func<Challenge, string>> PropertyExpression => x => x.Condition;
    }
}