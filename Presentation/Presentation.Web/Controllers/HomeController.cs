using System;
using System.Collections.Generic;
using Business.Identity;
using Business.Identity.ViewModels;
using Business.SearchIndex;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly Lazy<IIdentityService> identityService;
        private readonly Lazy<ISearchIndexService> searchIndexService;

        public HomeController(Lazy<IIdentityService> identityService,
            Lazy<ISearchIndexService> searchIndexService)
        {
            this.identityService = identityService;
            this.searchIndexService = searchIndexService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IEnumerable<UserTopViewModel> GetTopUsers()
        {
            return identityService.Value.GetTopUsers();
        }

        public IEnumerable<string> GetTags()
        {
            const int numberOfTagsFetched = 50;

            return searchIndexService.Value.GetTags(numberOfTagsFetched);
        }
    }
}
