using System;
using System.ServiceModel;
using Business.Challenges.ViewModels;
using Data.Challenges.Entities;

namespace Business.Challenges
{
    [ServiceContract]
    public interface IChallengesService
    {
        [OperationContract]
        ChallengeViewModel AddChallenge(ChallengeViewModel challenge);

        [OperationContract]
        ChallengeViewModel UpdateChallenge(ChallengeViewModel challenge);

        [OperationContract]
        void RemoveChallenge(Guid id);

        [OperationContract]
        ChallengeViewModel GetChallengeViewModel(Guid id);

        [OperationContract]
        Challenge GetChallenge(Guid id);

        [OperationContract]
        void AddSolver(Guid challengeId, Guid userId);

        [OperationContract]
        void AddComment(Guid challengeId, Guid userId, string message);

        [OperationContract]
        void RemoveComment(Guid challengeId, Guid commentId, Guid userId);

        [OperationContract]
        void RateChallenge(Guid challengeId, Guid userId, int rating);

        [OperationContract]
        void AddSolveAttempt(Guid challengeId, Guid userId);

        [OperationContract]
        bool TryToSolve(Guid challengeId, Guid userId, string answer);
    }
}