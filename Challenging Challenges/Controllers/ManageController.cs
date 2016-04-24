using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Business.Identity.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Challenging_Challenges.Models.ViewModels;
using Data.Identity.Entities;
using Microsoft.Owin.Security;

namespace Challenging_Challenges.Controllers
{
    [Authorize]
    public class ManageController : BaseController
    {
        private UserManager<IdentityUser, Guid> userManager;

        public ManageController(UserManager<IdentityUser, Guid> userManager)
        {
            this.userManager = userManager;
        }

        public void SetAbout(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            IdentityUser user = userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
            user.About = value;
            userManager.Update(user);
        }

        //
        // POST: /Manage/ChangeUserName
        [HttpPost]
        public async Task<ActionResult> ChangeUserName(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            Regex regex = new Regex("^([A-Z][a-z]+ [A-Z][a-z]+)$");
            if (!regex.IsMatch(value))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            if (userManager.FindByName(value) == null)
            {
                IdentityUser user = userManager.FindById(Guid.Parse(User.Identity.GetUserId()));
                user.UserName = value;
                userManager.Update(user);
                await SignInAsync(user, isPersistent: false);
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
            var result = await userManager.ChangePasswordAsync(Guid.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await userManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("UserProfile", "Account", new { userName = User.Identity.Name.Replace(' ', '_') });
            }
            AddErrors(result);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }

            base.Dispose(disposing);
        }

        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

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