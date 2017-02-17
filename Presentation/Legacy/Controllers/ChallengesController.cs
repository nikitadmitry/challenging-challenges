using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Business.Achievements;
using Business.Achievements.ViewModels;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Business.Identity;
using Business.SearchIndex;
using Microsoft.AspNet.Identity;
using Presentation.Legacy.Helpers;
using Presentation.Legacy.Infrastructure;
using Shared.Framework.Resources;

namespace Presentation.Legacy.Controllers
{
    [Authorize]
    [RoutePrefix("Challenges")]
    public class ChallengesController : BaseController
    {
        private readonly IChallengesService challengesService;
        private readonly IIdentityService identityService;
        private readonly IAchievementsService achievementsService;
        private readonly IAchievementsSignalRProvider achievementsSignalRProvider;
        private readonly ISearchIndexService searchIndexService;

        public ChallengesController(IChallengesService challengesService, 
            IIdentityService identityService,
            IAchievementsService achievementsService,
            IAchievementsSignalRProvider achievementsSignalRProvider,
            ISearchIndexService searchIndexService)
        {
            this.challengesService = challengesService;
            this.identityService = identityService;
            this.achievementsService = achievementsService;
            this.achievementsSignalRProvider = achievementsSignalRProvider;
            this.searchIndexService = searchIndexService;
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

        public JsonResult TagSearch(string term = "", int limit = 10)
        {
            var items = searchIndexService.GetTagsByTerm(term, limit);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        // GET, POST: Challenges
        public ActionResult Index(string keyword = "", string field = "", int page = 1)
        {
            if (page < 1) page = 1;
            if (keyword.Length > 200)
            {
                keyword = keyword.Substring(0, 200);
            }

            var count = ConfigurationValuesProvider.Get<int>("TotalChallengesFetchedNumber");

            var totalCount = challengesService.GetChallengesCount();

            var pageRule = new ChallengesPageRule
            {
                Count = count,
                Start = count * (page-1),
                Keyword = keyword,
                SearchTypes = new []{GetSearchType(field)}
            };

            var pagedList =
                PagedListBuilder<ChallengeInfoViewModel>.Build(
                    challengesService.SearchByRule(pageRule), page, count, totalCount);
   
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ChallengesIndexList", pagedList)
                : View(pagedList);
        }

        private ChallengeSearchType GetSearchType(string field)
        {
            ChallengeSearchType enumValue;

            if (!Enum.TryParse(field, out enumValue))
            {
                enumValue = ChallengeSearchType.Title;
            }

            return enumValue;
        }

        public ActionResult ByUser(string userName)
        {
            var userId = identityService.GetIdentityUserByUserName(userName).Id;
            return RedirectToAction("Index", "Challenges", new { keyword = userId.ToString(), field = "AuthorId" });
        }

        // GET: Challenges/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Challenges/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ChallengeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId().ToGuid();
                model.AuthorId = userId;

                challengesService.AddChallenge(model);

                var achievement = achievementsService.ChallengePosted(userId);
                achievementsSignalRProvider.ShowAchievementMessage(achievement, userId);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Challenges/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var challenge = challengesService.GetChallengeViewModel(id.Value);
            if (challenge == null) return RedirectToAction("Removed");
            if (!UserIsAuthor(challenge)) return RedirectToAction("Index");
            ViewBag.IsEditing = true;
            return View("Create", challenge);
        }

        // POST: Challenges/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChallengeViewModel model)
        {
            model.AuthorId = challengesService.GetChallengeAuthor(model.Id);

            if (ModelState.IsValid)
            {
                if (!UserIsAuthor(model)) return RedirectToAction("Index");

                model.Tags = challengesService.GetTagsAsStringByChallengeId(model.Id);

                challengesService.UpdateChallenge(model);

                return RedirectToAction("Index");
            }
            model.Answers.RemoveAll(string.IsNullOrWhiteSpace);
            ViewBag.IsEditing = true;
            return View("Create", model);
        }

        // GET: Challenges/Solve/5
        public ActionResult Solve(Guid? id)
        {
            if (id == null) return RedirectToAction("Index");

            var challenge = challengesService.GetChallengeFullViewModel(id.Value);

            var userId = User.Identity.GetUserId().ToGuid();

            if (UserIsAuthor(challenge))
            {
                ViewBag.IsAuthor = true;
            }
            else
            {
                challengesService.AddSolver(id.Value, userId);
            }

            ViewBag.IsSolved = challengesService.CheckIfSolved(id.Value, userId);

            return View("Solve", challenge);
        }

        // POST: Challenges/Solve/5
        [HttpPost]
        public ActionResult Solve(Guid? id, string answer)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var challenge = challengesService.GetChallengeFullViewModel(id.Value);

            if (UserIsAuthor(challenge)) return RedirectToAction("Index");

            var userId = User.Identity.GetUserId().ToGuid();

            challengesService.AddSolveAttempt(challenge.Id, userId);

            var solveResult = challengesService.TryToSolve(challenge.Id, userId, answer);

            if (solveResult.IsSolved)
            {
                var achievement = achievementsService.ChallengeSolved(challenge.Id, userId);
                achievementsSignalRProvider.ShowAchievementMessage(achievement, userId);
            }

            ViewBag.IsSolved = solveResult.IsSolved;
            ViewBag.Answer = answer;
            ViewBag.ShowBanner = solveResult.IsSolved;

            return View("Solve", challenge);
        }

        // GET: Challenges/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null) return RedirectToAction("Removed");

            var challenge = challengesService.GetChallengeViewModel(id.Value);

            //if (challenge == null) return RedirectToAction("Removed");

            if (!UserIsAuthor(challenge)) return HttpNotFound();

            return View(challenge);
        }

        // POST: Challenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var challenge = challengesService.GetChallengeViewModel(id);

            if (!UserIsAuthor(challenge)) return HttpNotFound();

            var userId = User.Identity.GetUserId().ToGuid();

            challengesService.RemoveChallenge(id);

            achievementsService.ChallengeRemoved(userId);

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public ActionResult AddComment(Guid? id, string message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (message.Length <= 100)
            {
                challengesService.AddComment(id.Value, User.Identity.GetUserId().ToGuid(), message);
            }

            return RedirectToAction("Solve", new { id = id.Value });
        }

        public ActionResult DeleteComment(Guid commentId, Guid challengeId)
        {
            challengesService.RemoveComment(challengeId, commentId, User.Identity.GetUserId().ToGuid());

            return RedirectToAction("Solve", new { id = challengeId });
        }

        public ActionResult UploadFile()
        {
            List<string> output = new FilesWorker().UploadFile(Request, Response);
            return Json(new { uploaded = 1, fileName = output[0], url = output[1] }, JsonRequestBehavior.AllowGet);
        }

        public string Rate(Guid? challengeId, int rating = 3)
        {
            if (challengeId == null || !Enumerable.Range(1, 5).Contains(rating)) return Localization.Error;

            var challenge = challengesService.GetChallengeViewModel(challengeId.Value);

            if (challenge == null) return Localization.Error;

            var result = Localization.SuccessfullyRated;

            try
            {
                challengesService.RateChallenge(challengeId.Value, User.Identity.GetUserId().ToGuid(), rating);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            
            return result;
        }

        public ActionResult Removed()
        {
            return View();
        }

        private bool UserIsAuthor(ChallengeViewModel challenge)
        {
            return challenge.AuthorId.Equals(User.Identity.GetUserId().ToGuid());
        }
    }
}
