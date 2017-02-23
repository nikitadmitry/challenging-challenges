using System;
using System.Collections.Generic;
using System.Linq;
using Business.Identity;
using Business.Identity.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly Lazy<IIdentityService> identityService;

        public HomeController(Lazy<IIdentityService> identityService)
        {
            this.identityService = identityService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IEnumerable<UserTopViewModel> GetTopUsers()
        {
            return identityService.Value.GetTopUsers();
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPopularTags()
        {
            yield return "TAG1";
            yield return "TAG14";
            yield return "TAG22";
        }
    }
}
