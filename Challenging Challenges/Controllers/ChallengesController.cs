using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Business.Challenges.ViewModels;
using Challenging_Challenges.Infrastructure;
using Challenging_Challenges.Models.Entities;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Challenges.Repositories;
using Data.Identity.Entities;
using Data.Identity.Repositories;
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
        private readonly IChallengesUnitOfWork challengesUnitOfWork;
        private readonly ChallengesContext db;
        private readonly ChallengesEditorFactory challengesEditorFactory;
        private readonly StatisticsWorkerFactory statisticsWorkerFactory;
        private readonly AccountController accountController;

        public ChallengesController()
        {
            db = new ChallengesContext();
            challengesEditorFactory = new ChallengesEditorFactory();
            statisticsWorkerFactory = new StatisticsWorkerFactory();
            accountController = new AccountController();
        }

        public ChallengesController(IChallengesUnitOfWork challengesUnitOfWork) : this()
        {
            this.challengesUnitOfWork = challengesUnitOfWork;
        }

        public ChallengesController(ChallengesContext challengesContext, 
            ChallengesEditorFactory challengesEditorFactory,
            StatisticsWorkerFactory statisticsWorkerFactory,
            AccountController accountController)
        {
            db = challengesContext;
            this.challengesEditorFactory = challengesEditorFactory;
            this.statisticsWorkerFactory = statisticsWorkerFactory;
            this.accountController = accountController;
        }

        public JsonResult TagSearch(string term = "", int limit = 10)
        {
            if (term.Length > 180)
            {
                return Json(new[] { new object() }, JsonRequestBehavior.AllowGet);
            }
            term = term.Trim().Split(' ').Last().ToLower();
            List<string> tags = new List<string>();
            LuceneSearch.Search(Sort.RELEVANCE, term, "Tags", 0, limit).ForEach(index => index.Tags.Split(' ')
                .Where(tag => tag.StartsWith(term) && !tag.Equals(string.Empty)).ForEach(tag => tags.Add(tag)));
            tags = tags.Distinct().ToList();
            return Json(tags, JsonRequestBehavior.AllowGet);
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
            var userId = accountController.GetApplicationUser(userName, usersDb).Id;
            var list = LuceneSearch.Search(Sort.RELEVANCE, userId, "AuthorId", 0, 100).ToList();
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
                string userId = User.Identity.GetUserId();
                IdentityContext usersDb = new IdentityContext();
                ApplicationUser user = accountController.GetApplicationUser(userId, usersDb);
                Challenge challenge = model.ToChallenge(userId);
                challengesEditorFactory.GetEditor(challenge, db, userId).AddChallenge(model.Tags);
                statisticsWorkerFactory.GetWorker(usersDb, user).ChallengePosted(true);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Challenges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Challenge challenge = db.Challenges.Find(id);
            if (challenge == null) return RedirectToAction("Removed");
            if (!UserIsAuthor(challenge)) return RedirectToAction("Index");
            ViewBag.IsEditing = true;
            return View("Create", new ChallengeViewModel(challenge));
        }

        // POST: Challenges/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChallengeViewModel model)
        {
            model.Tags = LuceneSearch.Search(Sort.RELEVANCE, model.Id.ToString(), "Id", 0, 1).First().Tags;
            if (ModelState.IsValid)
            {
                Challenge challenge = db.Challenges.Find(model.Id);
                if (!UserIsAuthor(challenge)) return RedirectToAction("Index");
                string userId = User.Identity.GetUserId();
                new ChallengesEditor(challenge, db, userId).EditChallenge(model.ToChallenge(userId));
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
            Challenge challenge = db.Challenges.Find(id);
            if (challenge == null) return RedirectToAction("Removed");
            if (UserIsAuthor(challenge)) ViewBag.IsAuthor = true;
            else new ChallengesEditor(challenge, db, User.Identity.GetUserId()).AddSolver();
            return View("Solve", challenge);
        }

        // POST: Challenges/Solve/5
        [HttpPost]
        public ActionResult Solve(Guid? id, string answer)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Challenge challenge = db.Challenges.Find(id);
            if (challenge == null) return HttpNotFound();
            if (UserIsAuthor(challenge)) return RedirectToAction("Index");
            IChallengesEditor ce = challengesEditorFactory.GetEditor(challenge, db, User.Identity.GetUserId());
            ce.AddSolveAttempt();
            ViewBag.IsSolved = ce.TryToSolve(answer);
            ViewBag.Answer = answer;
            return View("Solve", challenge);
        }

        // GET: Challenges/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null) return RedirectToAction("Removed");
            Challenge challenge = db.Challenges.Find(id);
            if (challenge == null) return RedirectToAction("Removed");
            if (!UserIsAuthor(challenge)) return HttpNotFound();
            return View(challenge);
        }

        // POST: Challenges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Challenge challenge = db.Challenges.Find(id);
            if (!UserIsAuthor(challenge)) return HttpNotFound();
            string userId = User.Identity.GetUserId();
            new ChallengesEditor(challenge, db, userId).RemoveChallenge();
            IdentityContext usersDb = new IdentityContext();
            new StatisticsWorker(usersDb, accountController.GetApplicationUser(userId, usersDb)).ChallengePosted(false);
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public ActionResult AddComment(Guid? id, string message)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Challenge challenge = db.Challenges.Find(id);
            if (challenge == null) return HttpNotFound();
            if (message.Length <= 100)
            {
                new ChallengesEditor(challenge, db, User.Identity.GetUserId()).AddComment(User.Identity.Name, message);
            }
            return RedirectToAction("Solve", new { id = challenge.Id });
        }

        public ActionResult DeleteComment(Guid commentId, Guid challengeId)
        {
            Challenge challenge = db.Challenges.Find(challengeId);
            if (challenge == null) return HttpNotFound();
            var comment = challenge.Comments.FirstOrDefault(x => x.Id == commentId && x.UserName.Equals(User.Identity.Name));
            if (comment != null)
                new ChallengesEditor(challenge, db, User.Identity.GetUserId()).RemoveComment(comment);
            return RedirectToAction("Solve", new { id = challengeId });
        }

        public ActionResult UploadFile()
        {
            List<string> output = new FilesWorker().UploadFile(Request, Response);
            return Json(new { uploaded = 1, fileName = output[0], url = output[1] }, JsonRequestBehavior.AllowGet);
        }

        public string Rate(int? challengeId, int rating = 3)
        {
            if (challengeId == null || !Enumerable.Range(1, 5).Contains(rating)) return Localization.Error;
            Challenge challenge = db.Challenges.Find(challengeId);
            if (challenge == null) return Localization.Error;
            var result = challengesEditorFactory.GetEditor(challenge, db, User.Identity.GetUserId()).RateChallenge(rating);
            return result;
        }

        public ActionResult Removed()
        {
            return View();
        }

        private bool UserIsAuthor(Challenge challenge)
        {
            return challenge.AuthorId.Equals(User.Identity.GetUserId());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
