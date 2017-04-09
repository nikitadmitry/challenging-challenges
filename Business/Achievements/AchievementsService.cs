using System;
using System.Linq;
using System.ServiceModel;
using Business.Challenges;
using Business.Common.ViewModels;
using Data.Common.Query.Builder;
using Data.Common.Query.QueryParameters;
using Data.Identity.Context;
using Data.Identity.Entities;
using Data.Identity.Enums;
using Shared.Framework.Dependency;

namespace Business.Achievements
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AchievementsService : IAchievementsService, IDependency
    {
        private readonly IChallengesService challengesService;
        private readonly IIdentityUnitOfWork identityUnitOfWork;

        public AchievementsService(IChallengesService challengesService,
            IIdentityUnitOfWork identityUnitOfWork)
        {
            this.challengesService = challengesService;
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

        public AchievementType? ChallengeSolved(Guid challengeId, Guid userId)
        {
            var user = identityUnitOfWork.Get<User>(userId);

            user.SolvedTasksQuantity++;

            var timesSolved = challengesService.GetChallengeTimesSolved(challengeId);

            var addedBadge = GetAddedBadge(user, $"Solved{user.SolvedTasksQuantity}");

            if (timesSolved == 1)
            {
                addedBadge = GetAddedBadge(user, "First");
            }

            identityUnitOfWork.InsertOrUpdate(user);
            identityUnitOfWork.Commit();

            return addedBadge;
        }

        public void BecameTopOne(Guid userId)
        {
            var user = identityUnitOfWork.Get<User>(userId);

            GetAddedBadge(user, "TopOne");
        }

        public AchievementType? RatingChanged(Guid userId)
        {
            var user = identityUnitOfWork.Get<User>(userId);

            return GetAddedBadge(user, $"Rating{user.Rating.ToString("###")}");
        }

        public void UpdateTopOne()
        {
            var queryParameters = new QueryParameters
            {
                SortSettings = SortSettingsBuilder<User>
                    .Create()
                    .DescendingBy("Rating")
                    .GetSettings()
            };

            var user = identityUnitOfWork.GetFirstOrDefault<User>(queryParameters);

            BecameTopOne(user.Id);
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