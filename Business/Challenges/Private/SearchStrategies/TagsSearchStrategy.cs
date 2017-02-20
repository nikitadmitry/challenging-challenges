using System;
using System.Collections.Generic;
using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Data.Common.Query.QueryParameters;
using Shared.Framework.DataSource;
using Shared.Framework.Dependency;
using Shared.Framework.Validation;

namespace Business.Challenges.Private.SearchStrategies
{
    [KeyedDependency(ChallengeSearchType.Tags)]
    public class TagsSearchStrategy : ISearchStrategy
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TagsSearchStrategy(IChallengesUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        protected ChallengeSearchType SearchType => ChallengeSearchType.Tags;

        public bool IsApplicable(ChallengeSearchType searchType)
        {
            return searchType == SearchType;
        }

        public List<ChallengeInfoViewModel> Search(ChallengesPageRule pageRule)
        {
            Contract.Assert<InvalidOperationException>(pageRule.IsValid);

            var queryParameters = new QueryParameters
            {
                PageRule = pageRule
            };

            //todo add view.
            if (!pageRule.Keyword.IsNullOrEmpty())
            {
                queryParameters.FilterSettings = FilterSettingsBuilder<Tag>.Create()
                    .AddFilterRule(x => x.Value, FilterOperator.IsEqualTo, pageRule.Keyword)
                    .GetSettings();
            }

            var challenges = unitOfWork.GetAll<Tag>(queryParameters);

            return mapper.Map<List<ChallengeInfoViewModel>>(challenges);
        }
    }
}