using Data.Common;

namespace Data.Challenges.Entities
{
    public class Solver : Entity
    {
        public string UserId
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
    }
}
