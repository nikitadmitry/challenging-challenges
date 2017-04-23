namespace Business.Challenges.ViewModels
{
    public class ChallengeSolveResult
    {
        public bool IsSolved
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public double RatingObtained
        {
            get;
            set;
        }
    }
}