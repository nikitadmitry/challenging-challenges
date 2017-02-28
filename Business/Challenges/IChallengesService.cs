using System;
using System.Collections.Generic;
using System.ServiceModel;
using Business.Challenges.ViewModels;

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
        ChallengeFullViewModel GetChallengeFullViewModel(Guid id);

        [OperationContract]
        void AddComment(Guid challengeId, Guid userId, string message);

        [OperationContract]
        void RemoveComment(Guid challengeId, Guid commentId, Guid userId);

        [OperationContract]
        void RateChallenge(Guid challengeId, Guid userId, int rating);

        [OperationContract]
        void AddSolveAttempt(Guid challengeId, Guid userId);

        [OperationContract]
        ChallengeSolveResult TryToSolve(Guid challengeId, Guid userId, string answer);

        [OperationContract]
        int GetChallengeTimesSolved(Guid challengeId);

        [OperationContract]
        List<ChallengesDescriptionViewModel> GetChallenges(SortedPageRule sortedPageRule);

        [OperationContract]
        int GetChallengesCount();

        [OperationContract]
        string GetTagsAsStringByChallengeId(Guid challengeId);

        [OperationContract]
        List<ChallengeInfoViewModel> SearchByRule(ChallengesSearchOptions searchOptions);

        [OperationContract]
        Guid GetChallengeAuthor(Guid challengeId);

        [OperationContract]
        bool CheckIfSolved(Guid challengeId, Guid userId);

        [OperationContract]
        ChallengeDetailsModel GetChallenge(Guid challengeId, Guid userId);
    }
}