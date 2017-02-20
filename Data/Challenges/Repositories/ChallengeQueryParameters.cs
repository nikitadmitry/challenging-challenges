using System.Collections.Generic;
using Data.Common.Query.QueryParameters;

namespace Data.Challenges.Repositories
{
    public class ChallengeQueryParameters : QueryParameters
    {
        public IList<string> Tags { get; set; }

        public bool IncludeTags { get; set; }
    }
}