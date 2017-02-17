using System.Collections.Generic;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Helpers;

namespace Presentation.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ChallengesController : Controller
    {
        private readonly IChallengesService challengesService;

        public ChallengesController(IChallengesService challengesService)
        {
            this.challengesService = challengesService;
        }

        [HttpPost]
        public IEnumerable<ChallengesDescriptionViewModel> GetChallenges([FromBody]SortedPageRule sortedPageRule)
        {
            return challengesService.GetChallenges(sortedPageRule);
        }

        [HttpGet]
        public int GetChallengesCount()
        {
            return challengesService.GetChallengesCount();
        }

        [HttpPost]
        [ValidateModel]
        public List<ChallengeInfoViewModel> Search([FromBody]ChallengesPageRule pageRule)
        {
            return challengesService.SearchByRule(pageRule);
        }
    }
}
