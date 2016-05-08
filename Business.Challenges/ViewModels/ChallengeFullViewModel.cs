using System.Collections.Generic;

namespace Business.Challenges.ViewModels
{
    public class ChallengeFullViewModel : ChallengeViewModel
    {
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