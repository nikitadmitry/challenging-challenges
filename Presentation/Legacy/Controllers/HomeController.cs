﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Business.Achievements;
using Business.Achievements.ViewModels;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Business.SearchIndex;
using Microsoft.AspNet.Identity;
using PagedList;
using Presentation.Legacy.Enums;
using Presentation.Legacy.Helpers;
using Presentation.Legacy.Infrastructure;
using Presentation.Legacy.Models.ViewModels;

namespace Presentation.Legacy.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IChallengesService challengesService;
        private readonly IComplexViewModelsProvider complexViewModelsProvider;
        private readonly IAchievementsSignalRProvider achievementsSignalRProvider;

        public HomeController(IChallengesService challengesService,
            IComplexViewModelsProvider complexViewModelsProvider,
            IAchievementsSignalRProvider achievementsSignalRProvider)
        {
            this.challengesService = challengesService;
            this.complexViewModelsProvider = complexViewModelsProvider;
            this.achievementsSignalRProvider = achievementsSignalRProvider;
        }

        [Authorize]
        public string AchievementTest(string id = "First")
        {
            AchievementType achievement;
            if (Enum.TryParse(id, out achievement))
            {
                achievementsSignalRProvider.ShowAchievementMessage(achievement, User.Identity.GetUserId().ToGuid());
                return "OK";
            }
            return "Not a valid achievement id";
        }

        [Authorize]
        public string UpdateIndex()
        {
            using (var scope = DependencyRegistration.Container.BeginLifetimeScope())
            {
                var searchIndexService = scope.Resolve<ISearchIndexService>();

                searchIndexService.RemoveRecords(LuceneRegistry.GetRemovedRecordIds());
                searchIndexService.UpdateIndex();
                searchIndexService.Optimize();

                scope.Resolve<IAchievementsService>().UpdateTopOne();
            }
            return "OK";
        }

        public ActionResult Index()
        {
            HomeChallengeViewModel model = complexViewModelsProvider.GetHomeChallengeViewModel();

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public string AdminTest()
        {
            return "You are Administrator.";
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
            page = page ?? 0;

            var result = GetChallengesDescriptionViewModels(page.Value, sort);

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

        private IPagedList<ChallengesDescriptionViewModel> GetChallengesDescriptionViewModels(int page, SortType sort)
        {
            var count = ConfigurationValuesProvider.Get<int>("DefaultMainPageCount");

            var pageRule = new SortedPageRule
            {
                Count = count,
                Start = (page - 1) * count,
                SortingType = (SortingType) sort
            };

            IList<ChallengesDescriptionViewModel> challengesViewModels = challengesService.GetChallenges(pageRule);
            var totalCount = challengesService.GetChallengesCount();

            var result = PagedListBuilder<ChallengesDescriptionViewModel>.Build(challengesViewModels, page, count,
                totalCount);

            return result;
        }
    }
}