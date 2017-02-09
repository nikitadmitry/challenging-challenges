﻿using System.Collections.Generic;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

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
    }
}
