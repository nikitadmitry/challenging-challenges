using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Challenging_Challenges;
using Challenging_Challenges.Controllers;
using Challenging_Challenges.Models.Context;
using Challenging_Challenges.Models.ViewModels;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;

namespace ChallengingChallengesTests.NUnit
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController subject;
        private Mock<ApplicationSignInManager> signInManager;
        private Mock<ApplicationUserManager> userManager;
        private Mock<UrlHelper> urlHelper;
        private string validUserName = "Test User", validPassword = "qWaszx-12";

        [OneTimeSetUp]
        public void Initialize()
        {
            urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(x => x.IsLocalUrl(It.IsAny<string>()))
                .Returns(false)
                .Verifiable("UrlHelper should be called");
        }

        [Test]
        public void Login_ValidCredentialsSpecified_SuccessfullyAuthenticated()
        {
            var route = new RouteValueDictionary
            {
                { "action", "Index" },
                { "controller", "Home" }
            };

            var result = PrepareAndRunSimpleLoginTest(validUserName, validPassword) as RedirectToRouteResult;

            result.Should().NotBeNull();
            result.RouteValues.ShouldBeEquivalentTo(route);
            signInManager.Verify(
                x => x.PasswordSignInAsync(
                    validUserName, validPassword, It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);
            urlHelper.Verify(x => x.IsLocalUrl(null));
        }

        [Test]
        public void Login_NotValidPasswordSpecified_AuthenticationFailed()
        {
            var password = "qWaszx-21";

            var result = PrepareAndRunSimpleLoginTest(validUserName, password) as ViewResult;

            result.Should().NotBeNull();

            var returnedModel = result.Model as LoginViewModel;

            returnedModel.Should().NotBeNull();
            returnedModel.UserName.Should().Be(validUserName);
            returnedModel.Password.Should().Be(password);
            signInManager.Verify(
                x => x.PasswordSignInAsync(
                    validUserName, password, It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Once);
        }

        [Test]
        public void Login_UserNameNotValid_ViewReturned()
        {
            var userName = "not valid name";

            PrepareSubject(userName, validPassword);
            PopulateModelState(new LoginViewModel
            {
                UserName = userName,
                Password = validPassword
            });

            var result = SimpleLoginTest(userName, validPassword) as ViewResult;

            result.Should().NotBeNull();

            var returnedModel = result.Model as LoginViewModel;
            returnedModel.Should().NotBeNull();
            returnedModel.UserName.Should().Be(userName);
            returnedModel.Password.Should().Be(validPassword);
            signInManager.Verify(
                x => x.PasswordSignInAsync(
                    userName, validPassword, It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        public void Register_ViewModelIsValid_RegistrationSuccessful()
        {
            var route = new RouteValueDictionary
            {
                { "action", "Index" },
                { "controller", "Home" }
            };

            var result = PrepareAndRunSimpleRegisterTest(validUserName, validPassword) as RedirectToRouteResult;

            result.Should().NotBeNull();
            result.RouteValues.ShouldBeEquivalentTo(route);
            userManager.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        public void Register_UserExists_RegisterPageReturned()
        {
            var userName = "Test Name";

            PrepareSubject(validUserName, validPassword);

            var result = SimpleRegisterTest(userName, validPassword) as ViewResult;
            result.Should().NotBeNull();

            var returnedModel = result.Model as RegisterViewModel;
            returnedModel.Should().NotBeNull();
            returnedModel.UserName.Should().Be(userName);
            returnedModel.Password.Should().Be(validPassword);
            signInManager.Verify(
                x => x.PasswordSignInAsync(
                    userName, validPassword, It.IsAny<bool>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Test]
        public void Register_ViewModelIsNotValid_RegisterPageReturned()
        {
            var password = "nonvalid password";

            PrepareSubject(validUserName, password);
            StubContext();
            PopulateModelState(new RegisterViewModel
            {
                UserName = validUserName,
                Password = password,
                ConfirmPassword = password,
                Email = "email@mail.com"
            });

            var result = SimpleRegisterTest(validUserName, password) as ViewResult;

            result.Should().NotBeNull();

            var returnedModel = result.Model as RegisterViewModel;
            returnedModel.UserName.Should().Be(validUserName);
            returnedModel.Password.Should().Be(password);

            userManager.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        private ActionResult PrepareAndRunSimpleLoginTest(string userName, string password)
        {
            PrepareSubject(validUserName, validPassword);

            return SimpleLoginTest(userName, password);
        }

        private ActionResult PrepareAndRunSimpleRegisterTest(string userName, string password)
        {
            PrepareSubject(validUserName, validPassword);

            StubContext();

            return SimpleRegisterTest(userName, password);
        }

        private void StubContext()
        {
            var context = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));
            subject.ControllerContext = new ControllerContext(new HttpContextWrapper(context), new RouteData(),
                new Mock<ControllerBase>().Object);
        }

        private ActionResult SimpleLoginTest(string userName, string password)
        {
            var loginViewModel = GetLoginViewModel(userName, password);

            var result = subject.Login(loginViewModel, null);

            return result.Result;
        }

        private ActionResult SimpleRegisterTest(string userName, string password)
        {
            var model = GetRegisterViewModel(userName, password);

            var result = subject.Register(model);

            return result.Result;
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

        private LoginViewModel GetLoginViewModel(string userName, string password)
        {
            return new LoginViewModel
            {
                UserName = userName,
                Password = password
            };
        }

        private RegisterViewModel GetRegisterViewModel(string userName, string password)
        {
            return new RegisterViewModel
            {
                UserName = userName,
                Password = password,
                ConfirmPassword = password,
                Email = "email"
            };
        }

        private void PrepareSubject(string userName, string password)
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            userManager = new Mock<ApplicationUserManager>(userStore.Object);

            StubUserManager(userName, password);

            var authenticationManager = new Mock<IAuthenticationManager>();
            signInManager = new Mock<ApplicationSignInManager>(userManager.Object, authenticationManager.Object);

            StubSignInManager(userName, password);

            subject = new AccountController(userManager.Object, signInManager.Object)
            {
                Url = urlHelper.Object
            };
        }

        private void StubSignInManager(string userName, string password)
        {
            signInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInStatus.Failure))
                .Verifiable("ApplicationSignInManager.PasswordSignInAsync should be called with a failed result");

            signInManager.Setup(
                x => x.PasswordSignInAsync(userName, password, It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInStatus.Success))
                .Verifiable("ApplicationSignInManager.PasswordSignInAsync should be called with a success result");

            signInManager.Setup(
                x => x.SignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(""))
                .Verifiable("ApplicationSignInManager.SignInAsync should be called");
        }

        private void StubUserManager(string userName, string password)
        {
            userManager.Setup(
                x => x.CreateAsync(It.IsAny<ApplicationUser>(), password))
                .Returns(Task.FromResult(IdentityResult.Failed()))
                .Verifiable("ApplicationUserManager.CreateAsync should be called");

            userManager.Setup(
                x => x.CreateAsync(It.Is<ApplicationUser>(u => u.UserName.Equals(userName)), password))
                .Returns(Task.FromResult(IdentityResult.Success))
                .Verifiable("ApplicationUserManager.CreateAsync should be called");

            userManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<string>()))
                .Returns(Task.FromResult("code"));
            userManager.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(""))
                .Verifiable("UserManager.SendEmailAsync should be called");
        }
    }
}