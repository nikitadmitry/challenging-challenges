using System;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Challenging_Challenges.Controllers;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Context;
using Challenging_Challenges.Models.Entities;
using Challenging_Challenges.Models.ViewModels;
using Challenging_Challenges.Resources;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ChallengingChallengesTests.NUnit
{
    [TestFixture]
    public class ChallengesControllerTests
    {
        private ChallengesController subject;
        private readonly string userId = Guid.NewGuid().ToString();
        private Mock<StatisticsWorkerFactory> statisticsWorkerFactory;
        private Mock<ChallengesEditorFactory> challengesEditorFactory;

        [OneTimeSetUp]
        public void Initialize()
        {
            subject = new ChallengesController();
        }

        [Test]
        public void Solve_IdNotSpecified_BadRequestHttpErrorReturned()
        {
            var result = subject.Solve(null, "answer") as HttpStatusCodeResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public void Solve_ChallengeNotExists_HttpNotFoundErrorReturned()
        {
            StubChallengeObtaining(challenge: null);

            var result = subject.Solve(1, "answer") as HttpNotFoundResult;

            result.Should().NotBeNull();
        }

        [Test]
        public void Solve_UserIsAuthor_RedirectToIndexReturned()
        {
            StubChallengeObtaining(GetChallenge(userId));
            StubUserIdentity();
            var route = new RouteValueDictionary
            {
                { "action", "Index" },
            };

            var result = subject.Solve(1, "answer") as RedirectToRouteResult;

            result.Should().NotBeNull();
            result.RouteValues.ShouldBeEquivalentTo(route);
        }

        [Test]
        public void Solve_DataIsValid_ChallengeSolved()
        {
            var authorId = Guid.NewGuid().ToString();

            StubChallengeObtaining(GetChallenge(authorId));
            StubUserIdentity();

            var result = subject.Solve(1, "answer") as ViewResult;

            result.Should().NotBeNull();
            var returnedModel = result.Model as Challenge;

            returnedModel.Should().NotBeNull();
            returnedModel.AuthorId.Should().Be(authorId);
        }

        [Test]
        public void Create_ModelIsNotValid_ViewReturned()
        {
            var viewModel = new ChallengeViewModel(
                GetChallenge(Guid.NewGuid().ToString()))
            {
                Condition = "short"
            };

            PopulateModelState(viewModel);

            var result = subject.Create(viewModel) as ViewResult;

            result.Should().NotBeNull();
            var returnedModel = result.Model as ChallengeViewModel;

            returnedModel.Should().NotBeNull();
            returnedModel.ShouldBeEquivalentTo(viewModel);
        }

        [Test]
        public void Create_ModelIsValid_ChallengeCreated()
        {
            var challenge = GetChallenge(Guid.NewGuid().ToString());

            var viewModel = new ChallengeViewModel(challenge);

            StubChallengeObtaining(challenge);
            StubUserIdentity();

            var result = subject.Create(viewModel) as RedirectToRouteResult;

            result.Should().NotBeNull();
            statisticsWorkerFactory.Verify(x => x.GetWorker(It.IsAny<ApplicationDbContext>(),
                It.IsAny<ApplicationUser>()));
        }

        [Test]
        public void Rate_ChallengeIdNotProvided_ErrorReturned()
        {
            var result = subject.Rate(null);

            result.Should().Be(Localization.Error);
        }

        [Test]
        public void Rate_ChallengeDoesntExists_ErrorReturned()
        {
            StubChallengeObtaining(null);

            var result = subject.Rate(1);

            result.Should().Be(Localization.Error);
        }

        [Test]
        public void Rate_DataIsValid_ChallengeRated()
        {
            StubChallengeObtaining(GetChallenge(Guid.NewGuid().ToString()));
            StubUserIdentity();

            var result = subject.Rate(1);

            result.Should().Be(Localization.SuccessfullyRated);
            challengesEditorFactory.Verify(x => x.GetEditor(It.IsAny<Challenge>(),
                It.IsAny<ChallengesContext>(), It.IsAny<string>()));
        }

        private void StubChallengeObtaining(Challenge challenge)
        {
            var challengesContext = new Mock<ChallengesContext>();
            var challengesDbSet = new Mock<DbSet<Challenge>>();
            challengesDbSet.Setup(x => x.Find(It.IsAny<int>()))
                .Returns(challenge);
            challengesContext.SetupGet(x => x.Challenges).Returns(challengesDbSet.Object);

            challengesEditorFactory = new Mock<ChallengesEditorFactory>();
            challengesEditorFactory
                .Setup(x => x.GetEditor(It.IsAny<Challenge>(), It.IsAny<ChallengesContext>(),
                    It.IsAny<string>()))
                .Returns(new ChallengesEditorStub());

            statisticsWorkerFactory = new Mock<StatisticsWorkerFactory>();
            statisticsWorkerFactory
                .Setup(x => x.GetWorker(It.IsAny<ApplicationDbContext>(), It.IsAny<ApplicationUser>()))
                .Returns(new StatisticsWorkerStub())
                .Verifiable("StatisticsWorkerFactory.GetWorker should be called");

            var accountController = new Mock<AccountController>();
            accountController
                .Setup(x => x.GetApplicationUser(It.IsAny<string>(), It.IsAny<ApplicationDbContext>()))
                .Returns(new ApplicationUser());

            subject = new ChallengesController(challengesContext.Object, challengesEditorFactory.Object,
                statisticsWorkerFactory.Object, accountController.Object);
        }

        private void StubUserIdentity()
        {
            var context = new Mock<HttpContextBase>();
            var identity = new GenericIdentity("useridentity");
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId));
            var principal = new GenericPrincipal(identity, new[] { "user" });
            context.Setup(s => s.User).Returns(principal);

            subject.ControllerContext = new ControllerContext(
                context.Object, new RouteData(),
                new Mock<ControllerBase>().Object);
        }

        private Challenge GetChallenge(string authorId = "")
        {
            var challenge = new ChallengeGenerator().GetChallenge(authorId);

            return challenge;
        }

        private void PopulateModelState(object model)
        {
            var modelBinder = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
                      () => model, model.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(
                        new NameValueCollection(), CultureInfo.InvariantCulture)
            };

            new DefaultModelBinder().BindModel(new ControllerContext(), modelBinder);

            subject.ModelState.Clear();
            subject.ModelState.Merge(modelBinder.ModelState);
        }
    }
}