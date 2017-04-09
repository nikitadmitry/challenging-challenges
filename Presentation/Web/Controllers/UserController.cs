using System;
using Business.Identity;
using Business.Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IIdentityService identityService;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(IIdentityService identityService, UserManager<IdentityUser> userManager)
        {
            this.identityService = identityService;
            this.userManager = userManager;
        }

        public IActionResult GetUser(Guid userId)
        {
            var userModel = identityService.GetUser(userId);

            var currentUserId = userManager.GetUserId(User);
            userModel.IsReadOnly = userModel.Id.ToString().Equals(currentUserId);

            return Json(userModel);
        }
    }
}