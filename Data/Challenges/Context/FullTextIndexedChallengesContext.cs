using System.Data.Entity.Infrastructure.Interception;
using Data.Common.FullTextSearch;

namespace Data.Challenges.Context
{
    public class FullTextIndexedChallengesContext : ChallengesContext
    {
        public FullTextIndexedChallengesContext()
        {
            DbInterception.Add(new FtsInterceptor());
        }
    }
}