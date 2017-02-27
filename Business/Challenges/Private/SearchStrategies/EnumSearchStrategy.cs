using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Business.Challenges.Exceptions;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Shared.Framework.DataSource;
using Shared.Framework.Utilities;

namespace Business.Challenges.Private.SearchStrategies
{
    public abstract class EnumSearchStrategy<TEnum> : SearchStrategyBase where TEnum : struct
    {
        protected EnumSearchStrategy(IChallengesUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected abstract IDictionary<TEnum, string[]> SearchStrings { get; }
        
        protected abstract Expression<Func<Challenge, TEnum>> PropertyExpression { get; }
        
        protected override void PopulateFilterSettings(FilterSettingsBuilder<Challenge> filterSettingsBuilder, string keyword)
        {
            var preparedKeyword = keyword.ToLower();

            if (SearchStrings.Any(SearchPredicate(preparedKeyword)))
            {
                var value = GetEnumFromKeyword(preparedKeyword);

                filterSettingsBuilder.AddFilterRule(PropertyExpression, FilterOperator.IsEqualTo, value);
            }
            else
            {
                throw new NoResultsException();
            }
        }

        private Func<KeyValuePair<TEnum, string[]>, bool> SearchPredicate(string keyword)
        {
            return x => x.Value.Any(key => key.Contains(keyword)) 
                || x.Value.Any(keyword.Contains);
        }

        private Func<KeyValuePair<TEnum, string[]>, bool> StrictSearchPredicate(string keyword)
        {
            return x => x.Value.Any(key => key.Equals(keyword)) 
                || x.Value.Any(keyword.Equals);
        }

        private TEnum GetEnumFromKeyword(string keyword)
        {
            var strictString = SearchStrings.FirstOrDefault(StrictSearchPredicate(keyword));
            return strictString.Value.IsNotNull()
                ? strictString.Key 
                : SearchStrings.First(SearchPredicate(keyword)).Key;
        }
    }
}