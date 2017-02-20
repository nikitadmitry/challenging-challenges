using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Data.Challenges.Entities;
using Data.Common;
using Data.Common.Query.QueryParameters;

namespace Data.Challenges.Repositories
{
    public class ChallengesRepository : Repository<Challenge>
    {
        public ChallengesRepository(DbContext context) : base(context)
        {
        }

        protected override IQueryable<Challenge> ApplyListQueryParametersTypeSpecific(
            IQueryable<Challenge> queryable, QueryParameters parameters)
        {
            queryable = ApplyChallengeQueryParameters(queryable, parameters as ChallengeQueryParameters);

            return base.ApplyListQueryParametersTypeSpecific(queryable, parameters);
        }

        private IQueryable<Challenge> ApplyChallengeQueryParameters(IQueryable<Challenge> queryable,
            ChallengeQueryParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.IncludeTags)
                {
                    queryable = queryable.Include(x => x.Tags);
                }

                if (!parameters.Tags.IsNullOrEmpty())
                {
                    queryable = queryable.Where(
                        x => x.Tags.Any(t => parameters.Tags.Any(p => p.Equals(t.Value))));
                }
            }

            return queryable;
        }
    }
}