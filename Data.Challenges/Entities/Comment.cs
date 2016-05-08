using System;
using Data.Common;

namespace Data.Challenges.Entities
{
    public class Comment : Entity
    {
        public Guid UserId
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
