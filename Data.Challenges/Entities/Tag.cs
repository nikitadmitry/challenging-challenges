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
        public virtual ICollection<Challenge> Challenges
        {
            get;
            set;
        }
    }
}
