using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Autofac.Features.Indexed;
using AutoMapper;
using Business.Challenges.Private;
using Business.Challenges.Private.SearchStrategies;
using Business.Challenges.ViewModels;
using Business.Identity;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Common.Query.Builder;
using Data.Common.Query.QueryParameters;
using Shared.Framework.DataSource;
using Shared.Framework.Dependency;
using Shared.Framework.Resources;
using Shared.Framework.Utilities;
using Shared.Framework.Validation;

namespace Business.Challenges
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ChallengesService: IChallengesService, IDependency
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly Lazy<IIdentityService> identityService;
        private readonly Lazy<IChallengeSolutionDispatcher> challengeSolutionDispatcher;
        private readonly IMapper mapper;
        private readonly IIndex<ChallengeSearchType, Lazy<ISearchStrategy>> searchStrategies;
        private readonly Lazy<SourceCodeTemplateCollector> sourceCodeTemplateCollector;
        private const ChallengeSearchType DefaultSearchType = ChallengeSearchType.Title;

        public ChallengesService(IChallengesUnitOfWork unitOfWork, 
            Lazy<IIdentityService> identityService,
            Lazy<IChallengeSolutionDispatcher> challengeSolutionDispatcher,
            IMapper mapper,
            IIndex<ChallengeSearchType, Lazy<ISearchStrategy>> searchStrategies,
            Lazy<SourceCodeTemplateCollector> sourceCodeTemplateCollector)
        {
            this.unitOfWork = unitOfWork;
            this.identityService = identityService;
            this.challengeSolutionDispatcher = challengeSolutionDispatcher;
            this.mapper = mapper;
            this.searchStrategies = searchStrategies;
            this.sourceCodeTemplateCollector = sourceCodeTemplateCollector;
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

        private void AddSolveAttempt(Guid challengeId, Guid userId)
        {
            Contract.NotDefault<InvalidOperationException, Guid>(userId, "user id must be not default");

            var parameters = FilterSettingsBuilder<Solver>.Create()
                .AddFilterRule(x => x.ChallengeId, FilterOperator.IsEqualTo, challengeId)
                .AddFilterRule(x => x.UserId, FilterOperator.IsEqualTo, userId)
                .ToListQueryParameters();

            var solver = unitOfWork.GetFirstOrDefault<Solver>(parameters);
            
            solver.NumberOfTries++;

            unitOfWork.InsertOrUpdate(solver);
            unitOfWork.Commit();
        }

        public ChallengeSolveResult TryToSolve(Guid challengeId, Guid userId, string answer)
        {
            AddSolveAttempt(challengeId, userId);

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

        public List<ChallengeInfoViewModel> SearchByRule(ChallengesSearchOptions searchOptions)
        {
            Contract.NotNull<ArgumentNullException>(searchOptions);
            Contract.NotNull<ArgumentException>(searchOptions.PageRule);
            Contract.Assert<InvalidOperationException>(searchOptions.PageRule.IsValid);

            var searchType = searchOptions.SearchTypes.IsNullOrEmpty() 
                ? DefaultSearchType
                : searchOptions.SearchTypes.First();

            return searchStrategies[searchType].Value.Search(searchOptions);
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

        public ChallengeDetailsModel GetChallenge(Guid challengeId, Guid userId)
        {
            var challenge = unitOfWork.Get<Challenge>(challengeId);

            var viewModel = mapper.Map<ChallengeDetailsModel>(challenge);
            viewModel.AuthorName = identityService.Value.GetIdentityUserById(challenge.AuthorId).UserName;
            viewModel.AnswerTemplate = GetSourceCodeTemplate((BusinessSection)challenge.Section);

            if (challenge.AuthorId == userId)
            {
                viewModel.IsAuthor = true;
            }
            else
            {
                var solver = GetSolver(challengeId, userId);

                if (solver.IsNull())
                {
                    CreateSolver(userId, challenge);
                }
                else
                {
                    viewModel.IsSolved = solver.HasSolved;
                }
            }

            return viewModel;
        }

        private void CreateSolver(Guid userId, Challenge challenge)
        {
            challenge.Solvers.Add(Solver.Create(userId));

            unitOfWork.InsertOrUpdate(challenge);

            unitOfWork.Commit();
        }

        private Solver GetSolver(Guid challengeId, Guid userId)
        {
            var queryParameters = FilterSettingsBuilder<Solver>.Create()
                .AddFilterRule(x => x.ChallengeId, FilterOperator.IsEqualTo, challengeId)
                .AddFilterRule(x => x.UserId, FilterOperator.IsEqualTo, userId)
                .ToListQueryParameters();

            return unitOfWork.GetFirstOrDefault<Solver>(queryParameters);
        }

        public string GetSourceCodeTemplate(BusinessSection section)
        {
            return sourceCodeTemplateCollector.Value.GetTemplate(section);
        }
    }
}