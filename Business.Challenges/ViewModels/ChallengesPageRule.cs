using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shared.Framework.DataSource;

namespace Business.Challenges.ViewModels
{
    public class ChallengesPageRule : PageRule
    {
        [StringLength(100)]
        public string Keyword { get; set; }

        public IEnumerable<ChallengeSearchType> SearchTypes { get; set; }
    }
}