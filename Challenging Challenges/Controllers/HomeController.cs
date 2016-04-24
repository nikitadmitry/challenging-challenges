using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.Challenges;
using Challenging_Challenges.Enums;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;
using Challenging_Challenges.Models.ViewModels;
using Data.Identity.Context;
using Data.Identity.Entities;
using PagedList;

namespace Challenging_Challenges.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IChallengesService challengesService;

        public HomeController(IChallengesService challengesService)
        {
            this.challengesService = challengesService;
        }

        public string UpdateIndex()
        {
            LuceneSearch.AddUpdateLuceneIndex(IndexRepository.GetAll());
            IdentityContext usersDb = new IdentityContext();
            ApplicationUser user = usersDb.Users.OrderByDescending(x => x.Rating).Take(1).First();
            new StatisticsWorker(usersDb, user).BecameTopOne();
            return "OK";
        }

        public ActionResult Index()
        {
            HomeChallengeViewModel model = new HomeChallengeViewModel();
            return View(model);
        }

        public ActionResult MobileTest()
        {
            //string user = "4e3c80ca-3fe0-49b6-aad0-be99d3243479";
            //var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            //foreach (var connectionId in NotificationHub.Connections.GetConnections(user))
            //{
            //    context.Clients.Client(connectionId).showAchievement("Achievement text goes there");
            //}
            return View();
        }

        public ActionResult ChallengesPartial(int? page, string selector, SortType sort)
        {
            page = page ?? 1;
            SearchService searchService = new SearchService();
            IPagedList<SearchIndex> result = searchService.GetPagedList(sort, page.Value);
            ViewData["selector"] = selector;
            ViewData["sort"] = sort;
            return PartialView("_ChallengesPartial", result);
        }

        public ActionResult SetCulture(string culture)
        {
            culture = CultureHelper.GetImplementedCulture(culture);
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;
            else
            {
                cookie = new HttpCookie("_culture")
                {
                    Value = culture,
                    Expires = DateTime.Now.AddYears(1)
                };
            }
            Response.Cookies.Add(cookie);
            return RedirectToAction("Index");
        }

        public ActionResult ChangeTheme(string theme)
        {
            if (!theme.Equals("light") && !theme.Equals("dark")) return RedirectToAction("Index", "Home");
            HttpCookie themeCookie = new HttpCookie("theme", theme) {Expires = DateTime.Now.AddYears(1)};
            Response.Cookies.Add(themeCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}