using System.Linq;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Business.Identity;
using Business.SearchIndex;
using Presentation.Legacy.Helpers;
using Presentation.Legacy.Models.ViewModels;

namespace Presentation.Legacy.Infrastructure
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

            var pageRule = new SortedPageRule()
            {
                Count = count,
                Start = 0
            };

            pageRule.SortingType = SortingType.Latest;
            viewModel.LatestChallenges =
                PagedListBuilder<ChallengesDescriptionViewModel>.Build(
                    challengesService.GetChallenges(pageRule), 1, count, totalCount);

            pageRule.SortingType = SortingType.Popular;
            viewModel.PopularChallenges =
                PagedListBuilder<ChallengesDescriptionViewModel>.Build(
                    challengesService.GetChallenges(pageRule), 1, count, totalCount);

            pageRule.SortingType = SortingType.Unsolved;
            viewModel.UnsolvedChallenges = 
                PagedListBuilder<ChallengesDescriptionViewModel>.Build(
                    challengesService.GetChallenges(pageRule), 1, count, totalCount);

            viewModel.TopUsers = identityService.GetTopUsers();

            viewModel.Tags = searchIndexService.GetTags(50).ToList();

            return viewModel;
        }
    }
}