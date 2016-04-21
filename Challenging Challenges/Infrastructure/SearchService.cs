using System.Collections.Generic;
using System.Linq;
using Challenging_Challenges.Enums;
using Challenging_Challenges.Models.Entities;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using PagedList;

namespace Challenging_Challenges.Infrastructure
{
    public class SearchService
    {
        public IPagedList<SearchIndex> GetPagedList(SortType sortType, int page = 1, int pageSize = 9)
        {
            return new StaticPagedList<SearchIndex>(GetSortedList(sortType, page - 1, pageSize), page, pageSize, 900);
        }

        private IEnumerable<SearchIndex> GetSortedList(SortType sortType, int page = 0, int pageSize = 9)
        {
            ChallengesContext db = new ChallengesContext();
            return db.Challenges.CustomSort(sortType)
                   .Skip(page * pageSize)
                   .Take(pageSize)
                   .ToList()
                   .Select(x => new SearchIndex(x));
        }
    }

    public static class ExtentionMethods
    {
        public static IOrderedQueryable<Challenge> CustomSort(this IQueryable<Challenge> source, SortType sortType)
        {
            if (sortType == SortType.Popular)
                return source.OrderByDescending(x => x.TimesSolved).ThenByDescending(x => x.Id);
            if (sortType == SortType.Unsolved)
                return source.OrderBy(x => x.TimesSolved).ThenBy(x => x.Id);
            return source.OrderByDescending(x => x.Id);
        }
    }

}
