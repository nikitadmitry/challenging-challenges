using System;
using System.ServiceModel;
using Business.Common.ViewModels;

namespace Business.Achievements
{
    [ServiceContract]
    public interface IAchievementsService
    {
        [OperationContract]
        AchievementType? ChallengePosted(Guid userId);

        [OperationContract]
        void ChallengeRemoved(Guid userId);

        [OperationContract]
        AchievementType? ChallengeSolved(Guid challengeId, Guid userId);

        [OperationContract]
        void BecameTopOne(Guid userId);

        [OperationContract]
        AchievementType? RatingChanged(Guid userId);

        [OperationContract]
        void UpdateTopOne();
    }
}