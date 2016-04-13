using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;

namespace ChallengingChallengesTests
{
    public class StatisticsWorkerStub: IStatisticsWorker
    {
        public void ChallengeSolved(Challenge challenge)
        {
        }

        public void ChallengePosted(bool isAdded)
        {
        }

        public void RatingChanged()
        {
        }

        public void BecameTopOne()
        {
        }
    }
}