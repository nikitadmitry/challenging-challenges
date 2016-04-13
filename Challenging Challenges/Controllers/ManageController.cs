using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Challenging_Challenges.Models.ViewModels;
using Data.Identity.Entities;

namespace Challenging_Challenges.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public void SetAbout(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            user.About = value;
            UserManager.Update(user);
        }

        //
        // POST: /Manage/ChangeUserName
        [HttpPost]
        public ActionResult ChangeUserName(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Regex regex = new Regex("^([A-Z][a-z]+ [A-Z][a-z]+)$");
            if (!regex.IsMatch(value))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            if (UserManager.FindByName(value) == null)
            {
                ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
                user.UserName = value;
                UserManager.Update(user);
                SignInManager.SignIn(user, false, false);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return null;
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("UserProfile", "Account", new { userName = User.Identity.Name.Replace(' ', '_') });
            }
            AddErrors(result);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}