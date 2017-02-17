using System.Collections.Generic;
using Data.Common;

namespace Data.Challenges.Entities
{
    public class Tag : Entity
    {
        public string Value
        {
            get;
            set;
        }

        private List<Challenge> challenges;

        public virtual IList<Challenge> Challenges
        {
            get
            {
                return challenges ?? (challenges = new List<Challenge>());
            }
        }
    }
}
