using System.Linq;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Business.Identity;
using Business.SearchIndex;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Models.ViewModels;
using Shared.Framework.DataSource;

namespace Challenging_Challenges.Infrastructure
{
    public class ComplexViewModelsProvider : IComplexViewModelsProvider
    {
        private readonly IChallengesService challengesService;
        private readonly IIdentityService identityService;
        private readonly ISearchIndexService searchIndexService;

        public ComplexViewModelsProvider(IChallengesService challengesService, 
            IIdentityService identityService,
            ISearchIndexService searchIndexService)
        {
            this.challengesService = challengesService;
            this.identityService = identityService;
            this.searchIndexService = searchIndexService;
        }

        public HomeChallengeViewModel GetHomeChallengeViewModel()
        {
            var viewModel = new HomeChallengeViewModel();

            var count = ConfigurationValuesProvider.Get<int>("DefaultMainPageCount");

            var totalCount = challengesService.GetChallengesCount();

            var pageRule = new PageRule
            {
                Count = count,
                Start = 0
            };

            viewModel.LatestChallenges =
                PagedListBuilder<ChallengesDescriptionViewModel>.Build(challengesService.GetLatestChallenges(pageRule),
                    1, count, totalCount);

            viewModel.PopularChallenges =
                PagedListBuilder<ChallengesDescriptionViewModel>.Build(challengesService.GetPopularChallenges(pageRule),
                    1, count, totalCount);

            viewModel.UnsolvedChallenges = 
                PagedListBuilder<ChallengesDescriptionViewModel>.Build(challengesService.GetUnsolvedChallenges(pageRule),
                   1, count, totalCount);

            viewModel.TopUsers = identityService.GetTopUsers();

            viewModel.Tags = searchIndexService.GetTags(50).ToList();

            return viewModel;
        }
    }
}