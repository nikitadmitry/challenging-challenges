using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Shared.Framework.DataSource;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    public class ChallengesController : Controller
    {
        private readonly IChallengesService challengesService;

        public ChallengesController(IChallengesService challengesService)
        {
            this.challengesService = challengesService;
        }

        [HttpGet("[action]")]
        public IEnumerable<ChallengesDescriptionViewModel> GetLatestChallenges()
        {
            yield return new ChallengesDescriptionViewModel
            {
                Id = Guid.NewGuid(),
                Title = "title1",
                PreviewText = "превью 1"
            };
            //return challengesService.GetLatestChallenges(new PageRule
            //{
            //    Count = 10,
            //    Start = 0
            //});
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
