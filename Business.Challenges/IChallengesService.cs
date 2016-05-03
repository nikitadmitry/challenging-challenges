﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using Business.Challenges.ViewModels;
using Shared.Framework.DataSource;

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

        [OperationContract]
        int GetChallengeTimesSolved(Guid challengeId);

        [OperationContract]
        List<ChallengesDescriptionViewModel> GetLatestChallenges(PageRule pageRule);

        [OperationContract]
        List<ChallengesDescriptionViewModel> GetPopularChallenges(PageRule pageRule);

        [OperationContract]
        List<ChallengesDescriptionViewModel> GetUnsolvedChallenges(PageRule pageRule);

        [OperationContract]
        int GetChallengesCount();

        [OperationContract]
        string GetTagsAsStringByChallengeId(Guid challengeId);

        [OperationContract]
        List<ChallengeInfoViewModel> GetByProperty(string keyword, string property, PageRule pageRule);

        [OperationContract]
        Guid GetChallengeAuthor(Guid challengeId);
    }
}