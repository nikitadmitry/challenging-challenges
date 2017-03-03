using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Business.Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Web.Helpers;

namespace Presentation.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ChallengesController : Controller
    {
        private readonly IChallengesService challengesService;
        private readonly Lazy<UserManager<IdentityUser>> userManager;

        public ChallengesController(IChallengesService challengesService,
            Lazy<UserManager<IdentityUser>> userManager)
        {
            this.challengesService = challengesService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<ChallengeDetailsModel> GetChallenge(Guid challengeId)
        {
            var user = await userManager.Value.GetUserAsync(User);

            return challengesService.GetChallenge(challengeId, user.Id);
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
        public List<ChallengeInfoViewModel> Search([FromBody]ChallengesSearchOptions searchOptions)
        {
            return challengesService.SearchByRule(searchOptions);
        }

        [HttpPost]
        [Authorize]
        public async Task<ChallengeSolveResult> Solve(Guid challengeId, [FromBody]string answer)
        {
            var user = await userManager.Value.GetUserAsync(User);

            return challengesService.TryToSolve(challengeId, user.Id, answer);
        }

        [HttpGet]
        public string GetSourceCodeTemplate(BusinessSection section)
        {
            return challengesService.GetSourceCodeTemplate(section);
        }
    }
}
