using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Achievements;
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
        private readonly IAchievementsService achievementsService;

        public ChallengesController(IChallengesService challengesService,
            Lazy<UserManager<IdentityUser>> userManager,
            IAchievementsService achievementsService)
        {
            this.challengesService = challengesService;
            this.userManager = userManager;
            this.achievementsService = achievementsService;
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
        public async Task<JsonResult> Solve(Guid challengeId, [FromBody]string answer)
        {
            var user = await userManager.Value.GetUserAsync(User);

            var solveResult = challengesService.TryToSolve(challengeId, user.Id, answer);

            if (solveResult.IsSolved)
            {
                achievementsService.ChallengeSolved(challengeId, user.Id);
            }

            return Json(solveResult);
        }

        [HttpGet]
        public string GetSourceCodeTemplate(BusinessSection section)
        {
            return challengesService.GetSourceCodeTemplate(section);
        }

        [HttpPost]
        [Authorize]
        [ValidateModel]
        public JsonResult SaveChallenge([FromBody]EditChallengeViewModel challenge)
        {
            challenge.AuthorId = userManager.Value.GetUserId(User).ToGuid();

            var model = challengesService.SaveChallenge(challenge);

            return Json(model);
        }
    }
}
