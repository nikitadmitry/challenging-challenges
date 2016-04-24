using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Challenging_Challenges.Hubs;
using Data.Challenges.Entities;
using Data.Identity.Context;
using Data.Identity.Entities;
using Data.Identity.Enums;
using Shared.Framework.Resources;

namespace Challenging_Challenges.Infrastructure
{
    public class StatisticsWorkerFactory
    {
        public virtual IStatisticsWorker GetWorker(IdentityContext usersDb, ApplicationUser user)
        {
            return new StatisticsWorker(usersDb, user);
        }
    }

    public interface IStatisticsWorker
    {
        void ChallengeSolved(Challenge challenge);
        void ChallengePosted(bool isAdded);
        void RatingChanged();
        void BecameTopOne();
    }

    public class StatisticsWorker : IStatisticsWorker
    {
        private IdentityContext db;
        private ApplicationUser user;

        public StatisticsWorker(IdentityContext usersDb, ApplicationUser user)
        {
            db = usersDb;
            this.user = user;
        }

        public void ChallengeSolved(Challenge challenge)
        {
            db.Users.Attach(user);
            user.SolvedTasksQuantity++;
            if (challenge.TimesSolved == 1) AddBadge("First");
            AddBadge($"Solved{user.SolvedTasksQuantity}");
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void ChallengePosted(bool isAdded)
        {
            db.Users.Attach(user);
            user.PostedTasksQuantity += isAdded ? 1 : -1;
            if (isAdded) AddBadge($"Posted{user.PostedTasksQuantity}");
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void RatingChanged()
        {
            db.Users.Attach(user);
            AddBadge($"Rating{user.Rating.ToString("###")}");
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void BecameTopOne()
        {
            db.Users.Attach(user);
            AddBadge("TopOne");
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void AddBadge(string achievementString)
        {
            AchievementTypes achievement;
            if (Enum.TryParse(achievementString, out achievement))
                AddAchievement(achievement);
        }

        private void AddAchievement(AchievementTypes achievement)
        {
            if (user.Achievements.All(x => x.AchievementEnum != achievement))
            {
                user.Achievements.Add(new Achievement {Value = achievement.ToString()});
                ShowAhievement(achievement);
            }
        }

        private void ShowAhievement(AchievementTypes achievement)
        {
            var resourceSet = Achievements.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            foreach (var connectionId in NotificationHub.Connections.GetConnections(user.Id))
            {
                context.Clients.Client(connectionId).showAchievement(resourceSet.GetString(achievement.ToString()));
            }
        }
    }
}
