using System;
using System.Collections.Generic;
using AutoMapper;
using Business.Challenges.Exceptions;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Challenges.Repositories;
using Data.Common.Query.Builder;

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

        public List<ChallengeInfoViewModel> Search(ChallengesSearchOptions searchOptions)
        {
            var queryParameters = new ChallengeQueryParameters
            {
                PageRule = searchOptions.PageRule,
                IncludeTags = true
            };

            var filterSettingsBuilder = FilterSettingsBuilder<Challenge>.Create();

            if (!searchOptions.Keyword.IsNullOrEmpty())
            {
                try
                {
                    PopulateFilterSettings(filterSettingsBuilder, searchOptions.Keyword);
                }
                catch (NoResultsException)
                {
                    return new List<ChallengeInfoViewModel>();
                }
            }

            queryParameters.FilterSettings = filterSettingsBuilder.GetSettings();

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);

            return mapper.Map<List<ChallengeInfoViewModel>>(challenges);
        }

        protected abstract void PopulateFilterSettings(FilterSettingsBuilder<Challenge> filterSettingsBuilder,
            string keyword);
    }
}