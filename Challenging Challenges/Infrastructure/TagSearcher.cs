using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;
using WebGrease.Css.Extensions;

namespace Challenging_Challenges.Infrastructure
{
    public class TagSearcher
    {
        public static List<string> Search(string term = "", int limit = 10)
        {
            if (term.Length > 180)
            {
                return null;
            }
            term = term.Trim().Split(' ').Last().ToLower();
            List<string> tags = new List<string>();
            LuceneSearch.Search(Sort.RELEVANCE, term, "Tags", 0, limit).ForEach(index => index.Tags.Split(' ')
                .Where(tag => tag.StartsWith(term) && !tag.Equals(string.Empty)).ForEach(tag => tags.Add(tag)));
            tags = tags.Distinct().ToList();
            return tags;
        }
    }
}