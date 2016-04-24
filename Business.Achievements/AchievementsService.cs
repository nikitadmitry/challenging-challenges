using System;
using System.Linq;
using System.ServiceModel;
using Business.Achievements.ViewModels;
using Data.Challenges.Context;
using Data.Identity.Context;
using Data.Identity.Entities;
using Data.Identity.Enums;

namespace Business.Achievements
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class AchievementsService : IAchievementsService
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IIdentityUnitOfWork identityUnitOfWork;

        public AchievementsService(IChallengesUnitOfWork unitOfWork,
            IIdentityUnitOfWork identityUnitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.identityUnitOfWork = identityUnitOfWork;
        }

        public AchievementType? ChallengePosted(Guid userId)
        {
            var user = identityUnitOfWork.Get<User>(userId);

            user.PostedTasksQuantity++;

            var result = GetAddedBadge(user, $"Posted{user.PostedTasksQuantity}");

            identityUnitOfWork.InsertOrUpdate(user);
            identityUnitOfWork.Commit();

            return result;
        }

        public void ChallengeRemoved(Guid userId)
        {
            var user = identityUnitOfWork.Get<User>(userId);

            user.PostedTasksQuantity--;

            identityUnitOfWork.InsertOrUpdate(user);
            identityUnitOfWork.Commit();
        }

        private AchievementType? GetAddedBadge(User user, string achievementString)
        {
            AchievementType achievement;
            if (Enum.TryParse(achievementString, out achievement))
            {
                return AddAchievement(user, achievement);
            }

            return null;
        }

        private AchievementType? AddAchievement(User user, AchievementType achievement)
        {
            if (user.Achievements.Any(x => x.AchievementEnum == (DataAchievementType)achievement))
            {
                return null;
            }

            user.Achievements.Add(Achievement.Create(achievement.ToString()));

            return achievement;
        }
    }
}