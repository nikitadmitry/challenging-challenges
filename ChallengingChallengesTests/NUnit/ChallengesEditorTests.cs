using System;
using System.Collections.Generic;
using System.Data.Entity;
using Challenging_Challenges.Controllers;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Context;
using Challenging_Challenges.Models.Entities;
using Challenging_Challenges.Resources;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChallengingChallengesTests.NUnit
{
    [TestFixture]
    public class ChallengesEditorTests
    {
        private Mock<ChallengesEditor> subjectMock;
        private readonly string userId = "1";

        private ChallengesEditor subject => subjectMock.Object;

        [Test]
        public void RateChallenge_UserIsAuthor_ErrorReturned()
        {
            var challenge = GetChallenge(userId);

            CreateChallengesEditor(challenge);

            var result = subject.RateChallenge(3);

            result.Should().Be(Localization.YouCantRateYourChallenge);
        }

        [Test]
        public void RateChallenge_ChallengeNotSolved_ErrorReturned()
        {
            var challenge = GetChallenge(Guid.NewGuid().ToString());

            CreateChallengesEditor(challenge);

            var result = subject.RateChallenge(3);

            result.Should().Be(Localization.YouHaveToSolveFirst);
        }

        [Test]
        public void RateChallenge_UserHaveAlreadyRated_ErrorReturned()
        {
            var challenge = GetChallenge(Guid.NewGuid().ToString(),
                hasRated: true, hasSolved: true);

            CreateChallengesEditor(challenge);

            var result = subject.RateChallenge(3);

            result.Should().Be(Localization.YouHaveRated);
        }

        [Test]
        public void RateChallenge_RequestIsValid_ChallengeRated()
        {
            var challenge = GetChallenge(Guid.NewGuid().ToString(),
                hasRated: false, hasSolved: true);

            challenge.Answers = new List<Answer>();

            CreateChallengesEditor(challenge);

            var result = subject.RateChallenge(3);

            result.Should().Be(Localization.SuccessfullyRated);
        }

        private void CreateChallengesEditor(Challenge challenge)
        {
            subjectMock = new Mock<ChallengesEditor>(challenge, GetChallengesContextStub(challenge),
                userId, GetAccountControllerStub(), GetApplicationDbContextStub());

            subjectMock.Setup(
                x => x.SetEntityStateModified(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationDbContext>()));
        }

        private ChallengesContext GetChallengesContextStub(Challenge challenge)
        {
            var challengesContext = new Mock<ChallengesContext>();
            var challengesDbSet = new Mock<DbSet<Challenge>>();

            challengesDbSet
                .Setup(x => x.Attach(It.IsAny<Challenge>()))
                .Returns(challenge);

            challengesContext.SetupGet(x => x.Challenges).Returns(challengesDbSet.Object);

            challengesContext.Setup(x => x.SaveChanges()).Returns(1);

            return challengesContext.Object;
        }

        private AccountController GetAccountControllerStub()
        {
            var accountController = new Mock<AccountController>();

            var user = new ApplicationUser();

            accountController
                .Setup(x => x.GetApplicationUser(It.IsAny<string>(), It.IsAny<ApplicationDbContext>()))
                .Returns(user);

            return accountController.Object;
        }

        private ApplicationDbContext GetApplicationDbContextStub()
        {
            var context = new Mock<ApplicationDbContext>();

            var usersDbSet = new Mock<DbSet<ApplicationUser>>();
            usersDbSet.Setup(x => x.Attach(It.IsAny<ApplicationUser>()))
                .Returns(new ApplicationUser());
            context.Setup(x => x.Users).Returns(usersDbSet.Object);

            return context.Object;
        }

        private Challenge GetChallenge(string authorId = "", bool hasSolved = false, bool hasRated = false)
        {
            var challenge = new ChallengeGenerator().GetChallenge(authorId);

            challenge.Solvers.Add(new Solver
            {
                UserId = userId,
                HasRated = hasRated,
                HasSolved = hasSolved
            });

            return challenge;
        }
    }
}