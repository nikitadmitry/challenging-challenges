using System;

namespace Business.Identity.ViewModels
{
    public class UserTopViewModel
    {
        public Guid UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public double Rating
        {
            get;
            set;
        }

        public int SolvedChallenges
        {
            get;
            set;
        }

        public int PostedChallenges
        {
            get;
            set;
        }
    }
}