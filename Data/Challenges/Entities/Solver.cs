using System;
using Data.Common;

namespace Data.Challenges.Entities
{
    public class Solver : Entity
    {
        public Guid UserId
        {
            get;
            set;
        }

        public bool HasSolved
        {
            get;
            set;
        }

        public bool HasRated
        {
            get;
            set;
        }

        public byte NumberOfTries
        {
            get;
            set;
        }

        public static Solver Create(Guid userId)
        {
            return new Solver
            {
                UserId = userId
            };
        }
    }
}
