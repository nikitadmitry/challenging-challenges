using System;
using System.ServiceModel;
using Business.Achievements.ViewModels;

namespace Business.Achievements
{
    [ServiceContract]
    public interface IAchievementsService
    {
        [OperationContract]
        AchievementType? ChallengePosted(Guid userId);

        [OperationContract]
        void ChallengeRemoved(Guid userId);
    }
}