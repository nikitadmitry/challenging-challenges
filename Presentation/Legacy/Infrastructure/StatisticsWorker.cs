using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Business.Achievements.ViewModels;
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
        public virtual IStatisticsWorker GetWorker(IdentityContext usersDb, User user)
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
        private User user;

        public StatisticsWorker(IdentityContext usersDb, User user)
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
    }
}
