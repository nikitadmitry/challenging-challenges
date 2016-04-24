using System.Collections.Generic;
using System.Linq;
using Challenging_Challenges.Controllers;
using Challenging_Challenges.Enums;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;
using Data.Identity.Context;
using Newtonsoft.Json;
using PagedList;

namespace Challenging_Challenges.Models.ViewModels
{
    public class HomeChallengeViewModel
    {
        public IPagedList<SearchIndex> LatestChallenges { get; set; }
        public IPagedList<SearchIndex> UnsolvedChallenges { get; set; }
        public IPagedList<SearchIndex> PopularChallenges { get; set; }
        public List<string> Tags { get; set; }
        public List<TopUser> TopUsers { get; set; }

        public HomeChallengeViewModel()
        {
            SearchService searchService = new SearchService();
            LatestChallenges = searchService.GetPagedList(SortType.Latest);
            UnsolvedChallenges = searchService.GetPagedList(SortType.Unsolved);
            PopularChallenges = searchService.GetPagedList(SortType.Popular);
            Tags = (List<string>) JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new ChallengesController().TagSearch("", 50).Data), 
                typeof(List<string>));
            TopUsers = new IdentityContext().Users.OrderByDescending(x => x.Rating)
                        .Take(10)
                        .ToList()
                        .Select(x => new TopUser(x))
                        .ToList();
        }
    }

    
}
