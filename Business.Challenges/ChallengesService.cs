using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using Autofac;
using AutoMapper;
using Business.Challenges.Private;
using Business.Challenges.ViewModels;
using Business.Identity;
using Business.SearchIndex;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Data.Common.Query.QueryParameters;
using Lucene.Net.Search;
using Shared.Framework.DataSource;
using Shared.Framework.Resources;
using Shared.Framework.Utilities;
using Shared.Framework.Validation;

namespace Business.Challenges
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ChallengesService: IChallengesService
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IIdentityService identityService;
        private readonly ISearchIndexService searchIndexService;
        private readonly Lazy<IChallengeSolutionDispatcher> challengeSolutionDispatcher;

        public ChallengesService(IChallengesUnitOfWork unitOfWork, 
            IIdentityService identityService,
            ISearchIndexService searchIndexService,
            Lazy<IChallengeSolutionDispatcher> challengeSolutionDispatcher)
        {
            this.unitOfWork = unitOfWork;
            this.identityService = identityService;
            this.searchIndexService = searchIndexService;
            this.challengeSolutionDispatcher = challengeSolutionDispatcher;
        }

        public ChallengeViewModel AddChallenge(ChallengeViewModel challenge)
        {
            Contract.NotNull<ArgumentNullException>(challenge);

            var challengeEntity = Mapper.Map<Challenge>(challenge);

            challengeEntity.TimeCreated = DateTime.Now;

            var updatedChallenge = unitOfWork.InsertOrUpdate(challengeEntity);
            unitOfWork.Commit();

            return Mapper.Map<ChallengeViewModel>(updatedChallenge);
        }

        public void RemoveChallenge(Guid id)
        {
            var challenge = unitOfWork.Get<Challenge>(id);

            unitOfWork.Delete(challenge);

            unitOfWork.Commit();
        }

        public ChallengeViewModel GetChallengeViewModel(Guid id)
        {
            var challenge = unitOfWork.Get<Challenge>(id);

            return Mapper.Map<ChallengeViewModel>(challenge);
        }

        public ChallengeFullViewModel GetChallengeFullViewModel(Guid id)
        {
            var challenge = unitOfWork.Get<Challenge>(id);

            var challengeViewModel = Mapper.Map<ChallengeFullViewModel>(challenge);

            foreach (var comment in challenge.Comments)
            {
                var userName = identityService.GetUserNameById(comment.UserId);
                challengeViewModel.Comments.Single(x => x.Id == comment.Id).UserName = userName;
            }

            return challengeViewModel;
        }

        public void AddSolver(Guid challengeId, Guid userId)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);

            if (challenge.Solvers.Any(x => x.UserId == userId))
            {
                return;
            }

            challenge.Solvers.Add(Solver.Create(userId));

            unitOfWork.InsertOrUpdate(challenge);

            unitOfWork.Commit();
        }

        public void AddComment(Guid challengeId, Guid userId, string message)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "userId must not be default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);
            
            challenge.Comments.Add(new Comment
            {
                UserId = userId,
                Value = message
            });

            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.Commit();
        }

        public void RemoveComment(Guid challengeId, Guid commentId, Guid userId)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(commentId, "Commend id must be not default");
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var commentToRemove = challenge.Comments.First(x => x.Id == commentId && x.UserId == userId);

            challenge.Comments.Remove(commentToRemove);

            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.Commit();
        }

        public void RateChallenge(Guid challengeId, Guid userId, int rating)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            if (challenge.AuthorId == userId)
            {
                throw new InvalidOperationException(Localization.YouCantRateYourChallenge);
            }

            var solver = challenge.Solvers.SingleOrDefault(x => x.UserId == userId);

            if (solver == null || !solver.HasSolved)
            {
                throw new InvalidOperationException(Localization.YouHaveToSolveFirst);
            }

            if (solver.HasRated)
            {
                throw new InvalidOperationException(Localization.YouHaveRated);
            }

            challenge.Rating += (rating - challenge.Rating) / challenge.NumberOfVotes;
            challenge.NumberOfVotes++;

            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.Commit();

            identityService.AddRatingToUser(challenge.AuthorId, (rating - challenge.Rating) / 10);
        }

        public void AddSolveAttempt(Guid challengeId, Guid userId)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var solver = challenge.Solvers.Single(x => x.UserId == userId);

            solver.NumberOfTries++;

            unitOfWork.InsertOrUpdate(solver);
            unitOfWork.Commit();
        }

        public ChallengeSolveResult TryToSolve(Guid challengeId, Guid userId, string answer)
        {
            return challengeSolutionDispatcher.Value.Solve(challengeId, userId, answer);
        }

        public int GetChallengeTimesSolved(Guid challengeId)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);
            
            return challenge.TimesSolved;
        }

        public ChallengeViewModel UpdateChallenge(ChallengeViewModel challenge)
        {
            Contract.NotNull<ArgumentNullException>(challenge);

            var challengeEntity = unitOfWork.Get<Challenge>(challenge.Id);

            Mapper.Map(challenge, challengeEntity);

            var updatedChallenge = unitOfWork.InsertOrUpdate(challengeEntity);

            unitOfWork.Commit();

            return Mapper.Map<ChallengeViewModel>(updatedChallenge);
        }

        public List<ChallengesDescriptionViewModel> GetLatestChallenges(PageRule pageRule)
        {
            var queryParameters = new QueryParameters
            {
                PageRule = pageRule,
                SortSettings = SortSettingsBuilder<Challenge>
                    .Create()
                    .DescendingBy("TimeCreated")
                    .GetSettings()
            };

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);

            return Mapper.Map<List<ChallengesDescriptionViewModel>>(challenges);
        }

        public List<ChallengesDescriptionViewModel> GetPopularChallenges(PageRule pageRule)
        {
            var queryParameters = new QueryParameters
            {
                PageRule = pageRule,
                SortSettings = SortSettingsBuilder<Challenge>
                    .Create()
                    .DescendingBy("TimesSolved")
                    .GetSettings()
            };

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);

            return Mapper.Map<List<ChallengesDescriptionViewModel>>(challenges);
        }

        public List<ChallengesDescriptionViewModel> GetUnsolvedChallenges(PageRule pageRule)
        {
            var queryParameters = new QueryParameters
            {
                PageRule = pageRule,
                SortSettings = SortSettingsBuilder<Challenge>
                    .Create()
                    .AscendingBy("TimeCreated")
                    .GetSettings(),
                FilterSettings = FilterSettingsBuilder<Challenge>
                    .Create()
                    .AddFilterRule(x => x.TimesSolved, FilterOperator.IsEqualTo, 0)
                    .GetSettings()
            };

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);

            return Mapper.Map<List<ChallengesDescriptionViewModel>>(challenges);
        }

        public int GetChallengesCount()
        {
            return unitOfWork.Count<Challenge>(QueryParameters.Empty);
        }

        public string GetTagsAsStringByChallengeId(Guid challengeId)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);

            StringBuilder tagsAsString = new StringBuilder();

            foreach (var tag in challenge.Tags)
            {
                tagsAsString.Append($"{tag.Value} ");
            }

            return tagsAsString.ToString();
        }

        public List<ChallengeInfoViewModel> GetByProperty(string keyword, string property, PageRule pageRule)
        {
            if (property.In(GetIndexedPropertyNames().ToArray()))
            {
                var list = searchIndexService.Search(new Sort(new SortField("Id", SortField.STRING, true)), keyword,
                    property, pageRule.Count * pageRule.Start, pageRule.Count);

                var viewModels = list.Select(searchIndex => 
                    unitOfWork.Get<Challenge>(searchIndex.Id))
                    .Select(Mapper.Map<ChallengeInfoViewModel>).ToList();

                return viewModels;
            }

            var challenges = SearchChallengesOnDb(keyword, property, pageRule);

            return Mapper.Map<List<ChallengeInfoViewModel>>(challenges);
        }

        public Guid GetChallengeAuthor(Guid challengeId)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);

            return challenge.AuthorId;
        }

        public bool CheckIfSolved(Guid challengeId, Guid userId)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var solver = challenge.Solvers.SingleOrDefault(x => x.UserId == userId);

            return solver?.HasSolved ?? false;
        }

        private IList<Challenge> SearchChallengesOnDb(string keyword, string property, PageRule pageRule)
        {
            var queryParameters = new QueryParameters
            {
                PageRule = pageRule
            };

            if (property.IsNullOrEmpty())
            {
                property = "Title";
            }

            if (!keyword.IsNullOrEmpty())
            {
                PopulateFilterSettings(keyword, property, queryParameters);
            }

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);
            return challenges;
        }

        private void PopulateFilterSettings(string keyword, string property, QueryParameters queryParameters)
        {
            var propertyInfo = GetPropertyInfo(property);

            if (propertyInfo == null)
            {
                return;
            }

            var typedValue = GetCastedPropertyValue(propertyInfo, keyword);

            if (typedValue != null)
            {
                queryParameters.FilterSettings = FilterSettingsBuilder<Challenge>.Create()
                    .AddFilterRule(GetPropertyExpression(property),
                        propertyInfo.PropertyType == typeof(string)
                            ? FilterOperator.Contains
                            : FilterOperator.IsEqualTo, typedValue).GetSettings();
            }
        }

        private object GetCastedPropertyValue(PropertyInfo propertyInfo, string value)
        {
            if (propertyInfo == null || string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (propertyInfo.PropertyType == typeof(Guid))
            {
                Guid userId;
                if (Guid.TryParse(value, out userId))
                {
                    return userId;
                }

                return null;
            }

            if (propertyInfo.PropertyType.IsEnum)
            {
                Type enumType = propertyInfo.PropertyType;
                if (Enum.IsDefined(enumType, value))
                {
                    return Enum.Parse(enumType, value);
                }

                return null;
            }

            return Convert.ChangeType(value, propertyInfo.PropertyType);
        }

        private PropertyInfo GetPropertyInfo(string property)
        {
            return typeof(Challenge).GetProperty(property);
        }

        private Expression<Func<Challenge, object>> GetPropertyExpression(string property)
        {
            switch (property)
            {
                case "AuthorId":
                    return x => x.AuthorId;
                case "Difficulty":
                    return x => x.Difficulty;
                case "Section":
                    return x => x.Section;
                case "Language":
                    return x => x.Language;
                case "Title":
                default:
                    return x => x.Title;
            }
        }

        private IEnumerable<string> GetIndexedPropertyNames()
        {
            yield return "Condition";
            yield return "PreviewText";
            yield return "Tags";
        } 
    }
}