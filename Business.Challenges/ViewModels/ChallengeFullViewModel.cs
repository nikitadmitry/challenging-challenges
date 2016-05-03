using System.Collections.Generic;

namespace Business.Challenges.ViewModels
{
    public class ChallengeFullViewModel : ChallengeViewModel
    {
        public List<SolverViewModel> Solvers
        {
            get;
            set;
        }

        public List<CommentViewModel> Comments
        {
            get;
            set;
        }

        public double Rating
        {
            get;
            set;
        }

        public int TimesSolved
        {
            get;
            set;
        }
    }
}