using System.Collections.Generic;
using Business.Challenges.ViewModels;
using Business.Identity.ViewModels;
using PagedList;

namespace Presentation.Legacy.Models.ViewModels
{
    public class HomeChallengeViewModel
    {
        public IPagedList<ChallengesDescriptionViewModel> LatestChallenges
        {
            get;
            set;
        }

        public IPagedList<ChallengesDescriptionViewModel> UnsolvedChallenges
        {
            get;
            set;
        }

        public IPagedList<ChallengesDescriptionViewModel> PopularChallenges
        {
            get;
            set;
        }

        public List<string> Tags
        {
            get;
            set;
        }

        public IList<UserTopViewModel> TopUsers
        {
            get;
            set;
        }
    }
}
