using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Autofac.Core;
using Business.Achievements;
using Business.Challenges;
using Business.Challenges.ViewModels;
using Business.Identity;
using Business.Identity.ViewModels;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Identity.Context;
using Data.Identity.Entities;
using Lucene.Net.Search;
using Microsoft.AspNet.Identity;
using PagedList;
using Shared.Framework.Resources;
using WebGrease.Css.Extensions;

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

        public ChallengesController(IChallengesService challengesService, 
            IIdentityService identityService,
            IAchievementsService achievementsService,
            IAchievementsSignalRProvider achievementsSignalRProvider)
        {
            this.challengesService = challengesService;
            this.identityService = identityService;
            this.achievementsService = achievementsService;
            this.achievementsSignalRProvider = achievementsSignalRProvider;
        }

        public JsonResult TagSearch(string term = "", int limit = 10)
        {
            var items = TagSearcher.Search(term, limit);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        // GET, POST: Challenges
        public ActionResult Index(string keyword = "", string field = "", int page = 1)
        {
            if (page < 1) page = 1;
            var list = LuceneSearch.Search(new Sort(new SortField("Id", SortField.STRING, true)), keyword, field, page - 1).ToList();
            IPagedList<SearchIndex> result = new StaticPagedList<SearchIndex>
                (list, page, 10, list.Count < 10 ? list.Count : 500);
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ChallengesIndexList", result)
                : View(result);
        }

        public ActionResult ByUser(string userName)
        {
            IdentityContext usersDb = new IdentityContext();
            var userId = identityService.GetIdentityUserByUserName(userName).Id;
            //var userId = accountController.GetApplicationUser(userName, usersDb).Id;
            var list = LuceneSearch.Search(Sort.RELEVANCE, userId.ToString(), "AuthorId", 0, 100).ToList();
            int count = list.Count;
            if (count < 1) count = 1;
            IPagedList<SearchIndex> result = new StaticPagedList<SearchIndex>(list, 1, count, count);
            return View("Index", result);
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
            model.AuthorId = User.Identity.GetUserId().ToGuid();
            model.Tags = LuceneSearch.Search(Sort.RELEVANCE, model.Id.ToString(), "Id", 0, 1).First().Tags;
            if (ModelState.IsValid)
            {
                var challenge = challengesService.GetChallengeViewModel(model.Id);

                if (!UserIsAuthor(challenge)) return RedirectToAction("Index");

                challengesService.UpdateChallenge(challenge);

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

            var challenge = challengesService.GetChallengeViewModel(id.Value);

            //if (challenge == null) return RedirectToAction("Removed");

            if (UserIsAuthor(challenge))
            {
                ViewBag.IsAuthor = true;
            }
            else
            {
                challengesService.AddSolver(id.Value, User.Identity.GetUserId().ToGuid());
            }

            var challenge1 = challengesService.GetChallenge(id.Value);

            return View("Solve", challenge1);
        }

        // POST: Challenges/Solve/5
        [HttpPost]
        public ActionResult Solve(Guid? id, string answer)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var challenge = challengesService.GetChallengeViewModel(id.Value);
            if (challenge == null) return HttpNotFound();
            if (UserIsAuthor(challenge)) return RedirectToAction("Index");
            challengesService.AddSolveAttempt(challenge.Id, User.Identity.GetUserId().ToGuid());
            ViewBag.IsSolved = challengesService.TryToSolve(challenge.Id, User.Identity.GetUserId().ToGuid(), answer);
            ViewBag.Answer = answer;

            var challenge1 = challengesService.GetChallenge(id.Value);
            return View("Solve", challenge1);
        }

        // GET: Challenges/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null) return RedirectToAction("Removed");
            var challenge = challengesService.GetChallengeViewModel(id.Value);
            if (challenge == null) return RedirectToAction("Removed");
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
            var challenge = challengesService.GetChallengeViewModel(challengeId);
            if (challenge == null) return HttpNotFound();
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
