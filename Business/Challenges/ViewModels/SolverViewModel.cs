using System;

namespace Business.Challenges.ViewModels
{
    public class SolverViewModel
    {
        public Guid UserId
        {
            get;
            set;
        }

        public Guid ChallengeId
        {
            get;
            set;
        }

        public bool HasSolved
        {
            get;
            set;
        }
    }
}