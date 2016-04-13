using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;
using Challenging_Challenges.Resources;

namespace ChallengingChallengesTests
{
    public class ChallengesEditorStub: IChallengesEditor
    {
        public void AddSolveAttempt()
        {
        }

        public bool TryToSolve(string answer)
        {
            return !answer.Equals("false");
        }

        public Solver AddSolver()
        {
            return new Solver();
        }

        public void EditChallenge(Challenge newChallenge)
        {
        }

        public void AddChallenge(string tags)
        {
        }

        public void RemoveChallenge()
        {
        }

        public string RateChallenge(int rating)
        {
            return Localization.SuccessfullyRated;
        }

        public void AddComment(string userName, string message)
        {
        }

        public void RemoveComment(Comment comment)
        {
        }
    }
}