using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
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
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ChallengesService: IChallengesService
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly Lazy<IIdentityService> identityService;
        private readonly ISearchIndexService searchIndexService;
        private readonly Lazy<IChallengeSolutionDispatcher> challengeSolutionDispatcher;
        private readonly IMapper mapper;

        public ChallengesService(IChallengesUnitOfWork unitOfWork, 
            Lazy<IIdentityService> identityService,
            ISearchIndexService searchIndexService,
            Lazy<IChallengeSolutionDispatcher> challengeSolutionDispatcher,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.identityService = identityService;
            this.searchIndexService = searchIndexService;
            this.challengeSolutionDispatcher = challengeSolutionDispatcher;
            this.mapper = mapper;
        }

        public ChallengeViewModel AddChallenge(ChallengeViewModel challenge)
        {
            Contract.NotNull<ArgumentNullException>(challenge);

            var challengeEntity = mapper.Map<Challenge>(challenge);

            challengeEntity.TimeCreated = DateTime.Now;

            var updatedChallenge = unitOfWork.InsertOrUpdate(challengeEntity);
            unitOfWork.Commit();

            return mapper.Map<ChallengeViewModel>(updatedChallenge);
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

            return mapper.Map<ChallengeViewModel>(challenge);
        }

        public ChallengeFullViewModel GetChallengeFullViewModel(Guid id)
        {
            var challenge = unitOfWork.Get<Challenge>(id);

            var challengeViewModel = mapper.Map<ChallengeFullViewModel>(challenge);

            foreach (var comment in challenge.Comments)
            {
                var userName = identityService.Value.GetUserNameById(comment.UserId);
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

            identityService.Value.AddRatingToUser(challenge.AuthorId, (rating - challenge.Rating) / 10);
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

            mapper.Map(challenge, challengeEntity);

            var updatedChallenge = unitOfWork.InsertOrUpdate(challengeEntity);

            unitOfWork.Commit();

            return mapper.Map<ChallengeViewModel>(updatedChallenge);
        }

        public List<ChallengesDescriptionViewModel> GetChallenges(SortedPageRule sortedPageRule)
        {
            Contract.Requires<ArgumentException>(sortedPageRule.IsValid);
            
            var queryParameters = new QueryParameters
            {
                PageRule = sortedPageRule
            };

            PrepareQueryParameters(queryParameters, sortedPageRule.SortingType);

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);

            return mapper.Map<List<ChallengesDescriptionViewModel>>(challenges);
        }

        private void PrepareQueryParameters(QueryParameters queryParameters, SortingType sortingType)
        {
            switch (sortingType)
            {
                case SortingType.Latest:
                    queryParameters.SortSettings = SortSettingsBuilder<Challenge>
                        .Create()
                        .DescendingBy("TimeCreated")
                        .GetSettings();
                    break;
                case SortingType.Popular:
                    queryParameters.SortSettings = SortSettingsBuilder<Challenge>
                        .Create()
                        .DescendingBy("TimesSolved")
                        .GetSettings();
                    break;
                case SortingType.Unsolved:
                    queryParameters.SortSettings = SortSettingsBuilder<Challenge>
                        .Create()
                        .AscendingBy("TimeCreated")
                        .GetSettings();
                    break;
            }

            if (sortingType == SortingType.Unsolved)
            {
                queryParameters.FilterSettings = FilterSettingsBuilder<Challenge>
                    .Create()
                    .AddFilterRule(x => x.TimesSolved, FilterOperator.IsEqualTo, 0)
                    .GetSettings();
            }
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

        public List<ChallengeInfoViewModel> SearchByRule(ChallengesPageRule pageRule)
        {
            Contract.Assert<InvalidOperationException>(pageRule.IsValid);

            var indexedProperties = pageRule.SearchTypes.Where(x => x.In(GetIndexedSearchTypes().ToArray())).ToList();

            List<ChallengeInfoViewModel> viewModels = new List<ChallengeInfoViewModel>();

            if (indexedProperties.Any())
            {
                var list = searchIndexService.Search(new Sort(new SortField("Id", SortField.STRING, true)),
                    ChallengeSearchTypesToString(indexedProperties), pageRule.Keyword, pageRule.Count * pageRule.Start, 
                    pageRule.Count);

                viewModels.AddRange(mapper.Map<List<ChallengeInfoViewModel>>(
                    list.Select(searchIndex => unitOfWork.Get<Challenge>(searchIndex.Id))));
            }

            var challenges = SearchChallengesOnDb(pageRule.Keyword, viewModels,
                ChallengeSearchTypesToString(pageRule.SearchTypes.Except(indexedProperties)), pageRule);

            viewModels.AddRange(mapper.Map<List<ChallengeInfoViewModel>>(challenges));
            
            return viewModels;
        }

        private string[] ChallengeSearchTypesToString(IEnumerable<ChallengeSearchType> searchTypes)
        {
            var properties = new List<string>();

            foreach (var challengeSearchType in searchTypes)
            {
                properties.Add(GetPropertyNameBySearchType(challengeSearchType));
            }

            return properties.ToArray();
        }

        private string GetPropertyNameBySearchType(ChallengeSearchType searchType)
        {
            switch (searchType)
            {
                case ChallengeSearchType.All:
                    return string.Empty;
                case ChallengeSearchType.Title:
                case ChallengeSearchType.Condition:
                case ChallengeSearchType.Difficulty:
                case ChallengeSearchType.Section:
                case ChallengeSearchType.Language:
                case ChallengeSearchType.PreviewText:
                case ChallengeSearchType.Tags:
                    return searchType.ToString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
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

        private IList<Challenge> SearchChallengesOnDb(string keyword, List<ChallengeInfoViewModel> indexedModels,
            string[] properties, PageRule pageRule)
        {
            var queryParameters = new QueryParameters
            {
                PageRule = pageRule
            };

            var filterSettingsBuilder = FilterSettingsBuilder<Challenge>.Create();

            if (!keyword.IsNullOrEmpty())
            {
                PopulateFilterSettings(keyword, properties, filterSettingsBuilder);
            }

            SkipIndexedModels(filterSettingsBuilder, indexedModels);

            queryParameters.FilterSettings = filterSettingsBuilder.GetSettings();

            var challenges = unitOfWork.GetAll<Challenge>(queryParameters);
            return challenges;
        }

        private void SkipIndexedModels(FilterSettingsBuilder<Challenge> filterSettingsBuilder,
            List<ChallengeInfoViewModel> indexedModels)
        {
            if (indexedModels.Any())
            {
                filterSettingsBuilder.AddFilterRule(x => x.Id, FilterOperator.IsNotContainedIn,
                    indexedModels.Select(x => x.Id));
            }
        }

        private void PopulateFilterSettings(string keyword, string[] properties, 
            FilterSettingsBuilder<Challenge> filterSettingsBuilder)
        {
            foreach (var property in properties)
            {
                var propertyInfo = GetPropertyInfo(property);

                if (propertyInfo == null)
                {
                    return;
                }

                var typedValue = GetCastedPropertyValue(propertyInfo, keyword);

                if (typedValue != null)
                {
                    filterSettingsBuilder.AddFilterRule(x => propertyInfo.GetValue(x),//GetPropertyExpression(property),
                        propertyInfo.PropertyType == typeof(string)
                            ? FilterOperator.Contains
                            : FilterOperator.IsEqualTo, typedValue);
                }
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

        //private Expression<Func<Challenge, object>> GetPropertyExpression(string property)
        //{
        //    switch (property)
        //    {
        //        case "AuthorId":
        //            return x => x.AuthorId;
        //        case "Difficulty":
        //            return x => x.Difficulty;
        //        case "Section":
        //            return x => x.Section;
        //        case "Language":
        //            return x => x.Language;
        //        case "Title":
        //        default:
        //            return x => x.Title;
        //    }
        //}

        private IEnumerable<ChallengeSearchType> GetIndexedSearchTypes()
        {
            yield return ChallengeSearchType.Condition;
            yield return ChallengeSearchType.PreviewText;
            yield return ChallengeSearchType.Tags;
        }
    }
}