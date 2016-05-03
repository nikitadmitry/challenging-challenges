using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Business.Achievements;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Business.Identity;
using Business.SearchIndex;
using Challenging_Challenges.Helpers;
using Challenging_Challenges.Infrastructure;
using Microsoft.AspNet.Identity;
using PagedList;
using Shared.Framework.DataSource;
using Shared.Framework.Resources;

namespace Challenging_Challenges.Controllers
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

        public JsonResult TagSearch(string term = "", int limit = 10)
        {
            var items = searchIndexService.GetTagsByTerm(term, limit);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        // GET, POST: Challenges
        public ActionResult Index(string keyword = "", string field = "", int page = 0)
        {
            if (page < 0) page = 0;

            var count = ConfigurationValuesProvider.Get<int>("TotalChallengesFetchedNumber");

            var totalCount = challengesService.GetChallengesCount();

            var pageRule = new PageRule
            {
                Count = count,
                Start = count * page
            };

            var pagedList =
                PagedListBuilder<ChallengeInfoViewModel>.Build(
                    challengesService.GetByProperty(keyword, field, pageRule), ++page, count, totalCount);
   
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ChallengesIndexList", pagedList)
                : View(pagedList);
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

            //if (challenge == null) return RedirectToAction("Removed");

            if (UserIsAuthor(challenge))
            {
                ViewBag.IsAuthor = true;
            }
            else
            {
                challengesService.AddSolver(id.Value, User.Identity.GetUserId().ToGuid());
            }

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

            if (challenge.Solvers.Any(x => x.UserId == userId && !x.HasSolved))
            {
                return RedirectToAction("Index");
            }

            challengesService.AddSolveAttempt(challenge.Id, userId);

            var isCorrect = challengesService.TryToSolve(challenge.Id, userId, answer);

            if (isCorrect)
            {
                var achievement = achievementsService.ChallengeSolved(challenge.Id, userId);
                achievementsSignalRProvider.ShowAchievementMessage(achievement, userId);
            }

            ViewBag.IsSolved = isCorrect;
            ViewBag.Answer = answer;

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
