//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using Presentation.Legacy.Infrastructure;
//using Presentation.Legacy.Models.Mobile;
//using Presentation.Legacy.Models.ViewModels;
//using Presentation.Legacy.Providers;
//using Data.Challenges.Context;
//using Data.Identity.Entities;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OAuth;

//todo: uncomment if mobile auth is on
//namespace Presentation.Legacy.Controllers
//{
//    [Authorize]
//    [RoutePrefix("api/Mobile")]
//    public class MobileController : ApiController
//    {
//        private ApplicationUserManager _userManager;
//        private readonly ChallengesContext _db = new ChallengesContext();

//        public MobileController()
//        {
//        }

//        public MobileController(ApplicationUserManager userManager,
//            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
//        {
//            UserManager = userManager;
//            AccessTokenFormat = accessTokenFormat;
//        }

//        public ApplicationUserManager UserManager
//        {
//            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
//            private set { _userManager = value; }
//        }

//        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

//        // GET api/Mobile/GetTasks
//        [HttpGet]
//        public IEnumerable<MobileChallenge> GetTasks()
//        {
//            var userId = User.Identity.GetUserId();
//            IEnumerable<MobileChallenge> result = _db.Challenges.Where(x => !x.AuthorId.Equals(userId) && x.Solvers.Count(y => y.UserId.Equals(userId) && y.HasSolved) == 0).Take(30).ToList().Select(x => new MobileChallenge(x));
//            return result;
//        }

//        // POST api/Mobile/SolveChallenge
//        [HttpPost]
//        public bool SolveChallenge(string answer, Guid id)
//        {
//            if (string.IsNullOrEmpty(answer)) return false;
//            var challenge = _db.Challenges.Find(id);
//            if (challenge == null) return false;
//            var userId = User.Identity.GetUserId();
//            if (challenge.AuthorId.Equals(userId)) return false;
//            var ce = new ChallengesEditor(challenge, _db, userId);
//            ce.AddSolveAttempt();
//            return ce.TryToSolve(answer);
//        }

//        //// GET api/Account/ExternalLogin
//        //[OverrideAuthentication]
//        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
//        //[AllowAnonymous]
//        //[Route("ExternalLogin", Name = "ExternalLogin")]
//        //public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
//        //{
//        //    if (error != null)
//        //    {
//        //        return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
//        //    }

//        //    if (!User.Identity.IsAuthenticated)
//        //    {
//        //        return new ChallengeResult(provider, this);
//        //    }

//        //    var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

//        //    if (externalLogin == null)
//        //    {
//        //        return InternalServerError();
//        //    }

//        //    if (externalLogin.LoginProvider != provider)
//        //    {
//        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
//        //        return new ChallengeResult(provider, this);
//        //    }

//        //    var user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
//        //        externalLogin.ProviderKey));

//        //    var hasRegistered = user != null;

//        //    if (hasRegistered)
//        //    {
//        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

//        //        var oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
//        //            OAuthDefaults.AuthenticationType);
//        //        var cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
//        //            CookieAuthenticationDefaults.AuthenticationType);

//        //        var properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
//        //        Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
//        //    }
//        //    else
//        //    {
//        //        IEnumerable<Claim> claims = externalLogin.GetClaims();
//        //        var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
//        //        Authentication.SignIn(identity);
//        //    }

//        //    return Ok();
//        //}

//        //// POST api/Account/Register
//        //[AllowAnonymous]
//        //[Route("Register")]
//        //public async Task<IHttpActionResult> Register(RegisterViewModel model)
//        //{
//        //    if (!ModelState.IsValid)
//        //    {
//        //        return BadRequest(ModelState);
//        //    }

//        //    var user = new User {UserName = model.UserName, Email = model.Email};

//        //    var result = await UserManager.CreateAsync(user, model.Password);

//        //    if (!result.Succeeded)
//        //    {
//        //        return GetErrorResult(result);
//        //    }

//        //    return Ok();
//        //}

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                UserManager.Dispose();
//            }

//            base.Dispose(disposing);
//        }

//        #region Helpers

//        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

//        private IHttpActionResult GetErrorResult(IdentityResult result)
//        {
//            if (result == null)
//            {
//                return InternalServerError();
//            }

//            if (!result.Succeeded)
//            {
//                if (result.Errors != null)
//                {
//                    foreach (var error in result.Errors)
//                    {
//                        ModelState.AddModelError("", error);
//                    }
//                }

//                if (ModelState.IsValid)
//                {
//                    return BadRequest();
//                }

//                return BadRequest(ModelState);
//            }

//            return null;
//        }

//        private class ExternalLoginData
//        {
//            public string LoginProvider { get; private set; }
//            public string ProviderKey { get; private set; }
//            private string UserName { get; set; }

//            public IList<Claim> GetClaims()
//            {
//                IList<Claim> claims = new List<Claim>();
//                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

//                if (UserName != null)
//                {
//                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
//                }

//                return claims;
//            }

//            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
//            {
//                var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

//                if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
//                {
//                    return null;
//                }

//                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
//                {
//                    return null;
//                }

//                return new ExternalLoginData
//                {
//                    LoginProvider = providerKeyClaim.Issuer,
//                    ProviderKey = providerKeyClaim.Value,
//                    UserName = identity.FindFirstValue(ClaimTypes.Name)
//                };
//            }
//        }

//        public class ChallengeResult : IHttpActionResult
//        {
//            public ChallengeResult(string loginProvider, ApiController controller)
//            {
//                LoginProvider = loginProvider;
//                Request = controller.Request;
//            }

//            public string LoginProvider { get; set; }
//            public HttpRequestMessage Request { get; set; }

//            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//            {
//                Request.GetOwinContext().Authentication.Challenge(LoginProvider);

//                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized) {RequestMessage = Request};
//                return Task.FromResult(response);
//            }
//        }

//        #endregion
//    }
//}