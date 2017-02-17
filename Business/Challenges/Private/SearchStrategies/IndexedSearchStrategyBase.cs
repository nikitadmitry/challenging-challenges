using System;
using System.Linq.Expressions;
using AutoMapper;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Shared.Framework.DataSource;

namespace Business.Challenges.Private.SearchStrategies
{
    public abstract class IndexedSearchStrategyBase : SearchStrategyBase
    {
        protected IndexedSearchStrategyBase(IFullTextIndexedChallengesUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected abstract Expression<Func<Challenge, string>> PropertyExpression { get; }

        protected override void PopulateFilterSettings(FilterSettingsBuilder<Challenge> filterSettingsBuilder, string keyword)
        {
            filterSettingsBuilder.AddFilterRule(PropertyExpression, FilterOperator.FullText, keyword);
        }
    }
}