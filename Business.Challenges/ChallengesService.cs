using System;
using System.Linq;
using System.ServiceModel;
using AutoMapper;
using Business.Challenges.ViewModels;
using Business.Identity;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Shared.Framework.Resources;
using Shared.Framework.Validation;

namespace Business.Challenges
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true/*, InstanceContextMode = InstanceContextMode.PerCall*/)]
    public class ChallengesService: IChallengesService
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IIdentityService identityService;

        public ChallengesService(IChallengesUnitOfWork unitOfWork, IIdentityService identityService)
        {
            this.unitOfWork = unitOfWork;
            this.identityService = identityService;
        }

        public ChallengeViewModel AddChallenge(ChallengeViewModel challenge)
        {
            Contract.NotNull<ArgumentNullException>(challenge);

            var challengeEntity = Mapper.Map<Challenge>(challenge);

            var updatedChallenge = unitOfWork.InsertOrUpdate(challengeEntity);
            unitOfWork.Commit();

            return Mapper.Map<ChallengeViewModel>(updatedChallenge);
        }

        public void RemoveChallenge(Guid id)
        {
            var challenge = unitOfWork.Get<Challenge>(id);

            unitOfWork.Delete(challenge);

            unitOfWork.Commit();
        }

        public ChallengeViewModel GetChallengeViewModel(Guid id)
        {
            var challenge = unitOfWork.Get<Challenge>(id);

            return Mapper.Map<ChallengeViewModel>(challenge);
        }

        public Challenge GetChallenge(Guid id)
        {
            return unitOfWork.Get<Challenge>(id);
        }

        public void AddSolver(Guid challengeId, Guid userId)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);

            if (challenge.Solvers.Any(x => x.UserId == userId))
            {
                return;
            }

            challenge.Solvers.Add(Solver.Create(userId));
            //todo test this
            unitOfWork.InsertOrUpdate(challenge);

            unitOfWork.Commit();
        }

        public void AddComment(Guid challengeId, Guid userId, string message)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var userName = identityService.GetUserNameById(userId);

            challenge.Comments.Add(new Comment
            {
                UserName = userName,
                Value = message
            });

            //todo test
            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.Commit();
        }

        public void RemoveComment(Guid challengeId, Guid commentId, Guid userId)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(commentId, "Commend id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var userName = identityService.GetUserNameById(userId);

            var commentToRemove = challenge.Comments.First(x => x.Id == commentId && x.UserName == userName);

            challenge.Comments.Remove(commentToRemove);

            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.Commit();
        }

        public void RateChallenge(Guid challengeId, Guid userId, int rating)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            if (challenge.AuthorId == userId)
            {
                throw new InvalidOperationException(Localization.YouCantRateYourChallenge);
            }

            var solver = challenge.Solvers.SingleOrDefault(x => x.UserId == userId);

            if (solver == null || !solver.HasSolved)
            {
                throw new InvalidOperationException(Localization.YouHaveToSolveFirst);
            }

            if (solver.HasRated)
            {
                throw new InvalidOperationException(Localization.YouHaveRated);
            }

            challenge.Rating += (rating - challenge.Rating) / challenge.NumberOfVotes;
            challenge.NumberOfVotes++;

            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.Commit();

            identityService.AddRatingToUser(challenge.AuthorId, (rating - challenge.Rating) / 10);
        }

        public void AddSolveAttempt(Guid challengeId, Guid userId)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var solver = challenge.Solvers.Single(x => x.UserId == userId);

            solver.NumberOfTries++;

            unitOfWork.InsertOrUpdate(solver);
            unitOfWork.Commit();
        }

        public bool TryToSolve(Guid challengeId, Guid userId, string answer)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var solver = challenge.Solvers.Single(x => x.UserId == userId);

            if (!challenge.Answers.Select(x => x.Value).Any(x => x.Equals(answer.ToLower())))
            {
                return false;
            }

            solver.HasSolved = true;
            challenge.TimesSolved++;
            //todo test
            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.InsertOrUpdate(solver);
            unitOfWork.Commit();

            return true;
        }

        public ChallengeViewModel UpdateChallenge(ChallengeViewModel challenge)
        {
            Contract.NotNull<ArgumentNullException>(challenge);

            var challengeEntity = unitOfWork.Get<Challenge>(challenge.Id);

            Mapper.Map(challenge, challengeEntity);

            var updatedChallenge = unitOfWork.InsertOrUpdate(challengeEntity);

            unitOfWork.Commit();

            return Mapper.Map<ChallengeViewModel>(updatedChallenge);
        }
    }
}