using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Challenging_Challenges.Controllers;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Resources;
using Data.Challenges.Entities;
using Data.Challenges.Repositories;
using Data.Identity.Entities;
using Data.Identity.Repositories;
using WebGrease.Css.Extensions;

namespace Challenging_Challenges.Infrastructure
{
    public class ChallengesEditorFactory
    {
        public virtual IChallengesEditor GetEditor(Challenge challenge, ChallengesContext db, string userId)
        {
            return new ChallengesEditor(challenge, db, userId);
        }
    }

    public interface IChallengesEditor
    {
        void AddSolveAttempt();
        bool TryToSolve(string answer);
        Solver AddSolver();
        void EditChallenge(Challenge newChallenge);
        void AddChallenge(string tags);
        void RemoveChallenge();
        string RateChallenge(int rating);
        void AddComment(string userName, string message);
        void RemoveComment(Comment comment);
    }

    public class ChallengesEditor : IChallengesEditor
    {
        private readonly Challenge challenge;
        private readonly string userId;
        private readonly ChallengesContext db;
        private readonly AccountController accountController;
        private readonly IdentityContext identityContext;

        public ChallengesEditor(Challenge challenge, ChallengesContext db,
            string userId, AccountController accountController,
            IdentityContext identityContext)
        {
            this.userId = userId;
            this.challenge = challenge;
            this.db = db;
            this.accountController = accountController;
            this.identityContext = identityContext;
        }

        public ChallengesEditor(Challenge challenge, ChallengesContext db, 
            string userId)
        {
            this.userId = userId;
            this.challenge = challenge;
            this.db = db;
            identityContext = new IdentityContext();
            accountController = new AccountController();
        }

        public void AddSolveAttempt()
        {
            var solver = AddSolver();
            db.Challenges.Attach(challenge);
            solver.NumberOfTries++;
            db.Entry(challenge).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool TryToSolve(string answer)
        {
            if (!challenge.Answers.Select(x => x.Value).Any(x => x.Equals(answer.ToLower()))) return false;
            var user = challenge.Solvers.FirstOrDefault(x => x.UserId.Equals(userId));
            db.Challenges.Attach(challenge);
            if (user != null) user.HasSolved = true;
            challenge.TimesSolved++;
            db.Entry(challenge).State = EntityState.Modified;
            db.SaveChanges();
            AddChallengeStats();
            return true;
        }

        public Solver AddSolver()
        {
            var solver = challenge.Solvers.FirstOrDefault(x => x.UserId == userId);
            if (solver == null)
            {
                solver = new Solver {UserId = userId};
                db.Challenges.Attach(challenge);
                challenge.Solvers.Add(solver);
                db.SaveChanges();
            }
            return solver;
        }

        public void EditChallenge(Challenge newChallenge)
        {
            db.Challenges.Attach(challenge);
            foreach (var answer in challenge.Answers.ToList())
                db.Entry(answer).State = EntityState.Deleted;
            challenge.BindChanges(newChallenge);
            LowerChallengeAnswers();
            db.Entry(challenge).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void AddChallenge(string tags)
        {
            LowerChallengeAnswers();
            LowerChallengeTags();
            db.Challenges.Add(challenge);
            db.SaveChanges();
            TagChallenge(tags);
        }

        public void RemoveChallenge()
        {
            List<Tag> tagsToDelete = challenge.Tags.Where(tag => tag.Challenges.Count == 1).ToList();
            db.Challenges.Remove(challenge);
            db.Tags.RemoveRange(tagsToDelete);
            db.SaveChanges();
            LuceneRegistry.StagedToDelete.Enqueue(challenge.Id.ToString());
        }

        public string RateChallenge(int rating)
        {
            if (challenge.AuthorId.Equals(userId)) return Localization.YouCantRateYourChallenge;
            var solver = challenge.Solvers.FirstOrDefault(x => x.UserId.Equals(userId));
            if (solver == null || !solver.HasSolved) return Localization.YouHaveToSolveFirst;
            if (solver.HasRated) return Localization.YouHaveRated;
            ChangeChallengeRating((rating - challenge.Rating) / challenge.NumberOfVotes, solver);
            ApplicationUser author = accountController.GetApplicationUser(challenge.AuthorId, identityContext);
            AddUserRating(author, (rating - challenge.Rating) / 10, identityContext);
            return Localization.SuccessfullyRated;
        }

        public void AddComment(string userName, string message)
        {
            db.Challenges.Attach(challenge);
            challenge.Comments.Add(new Comment { UserName = userName, Value = message });
            db.Entry(challenge).State = EntityState.Modified;
            db.SaveChanges();  
        }

        public void RemoveComment(Comment comment)
        {
            db.Challenges.Attach(challenge);
            challenge.Comments.Remove(comment);
            db.Entry(comment).State = EntityState.Deleted;
            db.Entry(challenge).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void LowerChallengeAnswers()
        {
            challenge.Answers.ForEach(x => x.Value = x.Value.ToLower());
        }

        private void LowerChallengeTags()
        {
            challenge.Tags.ForEach(x => x.Value = x.Value.ToLower());
        }

        private void TagChallenge(string tags)
        {
            if (string.IsNullOrEmpty(tags)) return;
            var service = new TagService(db);
            foreach (string tag in tags.Split(' ', ','))
                service.Tag(challenge, tag);
        }

        private void AddChallengeStats()
        {
            ApplicationUser user = accountController.GetApplicationUser(userId, identityContext);
            ApplicationUser author = accountController.GetApplicationUser(challenge.AuthorId, identityContext);
            float rating = challenge.Rating * challenge.Difficulty / 10 / GetNumberOfTries();
            AddUserRating(user, rating, identityContext);
            AddUserRating(author, rating / 5, identityContext);
            identityContext.SaveChanges();
            var sw = new StatisticsWorker(identityContext, user);
            sw.RatingChanged();
            sw.ChallengeSolved(challenge);
            new StatisticsWorker(identityContext, author).RatingChanged();
        }

        private byte GetNumberOfTries()
        {
            Solver solver = challenge.Solvers.FirstOrDefault(x => x.UserId.Equals(userId));
            return solver?.NumberOfTries ?? 0;
        }

        private void AddUserRating(ApplicationUser user, float rating, IdentityContext usersDb)
        {
            usersDb.Users.Attach(user);
            if (!user.EmailConfirmed) rating /= 3;
            user.Rating += rating;
            SetEntityStateModified(user, usersDb);
        }

        public virtual void SetEntityStateModified(ApplicationUser user, IdentityContext usersDb)
        {
            usersDb.Entry(user).State = EntityState.Modified;
        }

        private void ChangeChallengeRating(float rating, Solver solver)
        {
            db.Challenges.Attach(challenge);
            solver.HasRated = true;
            challenge.NumberOfVotes++;
            challenge.Rating += rating;
            db.Entry(challenge).State = EntityState.Modified;
            db.SaveChanges();
        }

    }
}
