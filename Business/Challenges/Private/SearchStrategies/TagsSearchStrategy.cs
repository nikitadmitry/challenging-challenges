using AutoMapper;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Shared.Framework.DataSource;

namespace Business.Challenges.Private.SearchStrategies
{
    public class TagsSearchStrategy : SearchStrategyBase
    {
        public TagsSearchStrategy(IChallengesUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override ChallengeSearchType SearchType => ChallengeSearchType.Tags;

        protected override void PopulateFilterSettings(FilterSettingsBuilder<Challenge> filterSettingsBuilder, string keyword)
        {
            filterSettingsBuilder.AddFilterRule(x => x.Tags, FilterOperator.Contains, keyword);
        }
    }
}