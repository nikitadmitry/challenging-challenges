using System;
using System.Collections.Generic;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Data.Common.Query.QueryParameters;
using Shared.Framework.Validation;

namespace Business.Challenges.Private.SearchStrategies
{
    public abstract class SearchStrategyBase : ISearchStrategy
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        protected abstract ChallengeSearchType SearchType { get; }

        protected SearchStrategyBase(IChallengesUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public List<ChallengeInfoViewModel> Search(ChallengesPageRule pageRule)
        {
            Contract.Assert<InvalidOperationException>(pageRule.IsValid);

            var queryParameters = new QueryParameters
            {
                PageRule = pageRule
            };

            var filterSettingsBuilder = FilterSettingsBuilder<Challenge>.Create();

            if (!pageRule.Keyword.IsNullOrEmpty())
            {
                PopulateFilterSettings(filterSettingsBuilder, pageRule.Keyword);
            }

            queryParameters.FilterSettings = filterSettingsBuilder.GetSettings();

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);

            return mapper.Map<List<ChallengeInfoViewModel>>(challenges);
        }

        protected abstract void PopulateFilterSettings(FilterSettingsBuilder<Challenge> filterSettingsBuilder,
            string keyword);

        public bool IsApplicable(ChallengeSearchType searchType)
        {
            return searchType == SearchType;
        }
    }
}