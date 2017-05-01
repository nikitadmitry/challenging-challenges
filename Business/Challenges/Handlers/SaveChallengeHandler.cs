using System;
using System.Linq;
using Autofac.Features.Indexed;
using AutoMapper;
using Business.Challenges.Private;
using Business.Challenges.ViewModels;
using Business.CodeExecution;
using Business.CodeExecution.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Challenges.Enums;
using Shared.Framework.Dependency;
using Shared.Framework.Validation;
using ChallengeType = Business.Challenges.ViewModels.ChallengeType;

namespace Business.Challenges.Handlers
{
    public class SaveChallengeHandler : IDependency
    {
        private readonly IChallengesUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IIndex<ChallengeType, IChallengeMapper> challengeMappers;
        private readonly ITagService tagService;
        private readonly ICodeExecutor codeExecutor;
        private readonly CodeExecutionRequestBuilder codeExecutionRequestBuilder;

        public SaveChallengeHandler(IChallengesUnitOfWork unitOfWork, IMapper mapper,
            IIndex<ChallengeType, IChallengeMapper> challengeMappers,
            ITagService tagService, ICodeExecutor codeExecutor,
            CodeExecutionRequestBuilder codeExecutionRequestBuilder)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.challengeMappers = challengeMappers;
            this.tagService = tagService;
            this.codeExecutor = codeExecutor;
            this.codeExecutionRequestBuilder = codeExecutionRequestBuilder;
        }

        public EditChallengeViewModel Save(EditChallengeViewModel model)
        {
            Contract.NotNull<ArgumentNullException>(model);

            ValidateChallenge(model);

            Challenge challenge;

            if (model.IsNew)
            {
                challenge = new Challenge
                {
                    TimeCreated = DateTime.Now
                };
            }
            else
            {
                challenge = unitOfWork.Get<Challenge>(model.Id.GetValueOrDefault());
            }

            MapModelToChallenge(challenge, model);

            unitOfWork.InsertOrUpdate(challenge);
            unitOfWork.Commit();

            return mapper.Map<EditChallengeViewModel>(challenge);
        }

        private void ValidateChallenge(EditChallengeViewModel model)
        {
            if (model.ChallengeType == ChallengeType.CodeAnswered)
            {
                foreach (var testCase in model.TestCases)
                {
                    ValidateTestCase(model, testCase);
                }
            }
        }

        private void ValidateTestCase(EditChallengeViewModel model, TestCaseViewModel testCase)
        {
            var executionRequest = codeExecutionRequestBuilder.Build((Section) model.Section, model.SourceCode,
                testCase.InputParameters);

            var executionResult = codeExecutor.Execute(executionRequest);

            Contract.Assert<InvalidOperationException>(executionResult.IsValid, executionResult.ErrorMessage);
            Contract.Assert<InvalidOperationException>(executionResult.Output.SequenceEqual(testCase.OutputParameters), string.Join(", ", executionResult.Output));
        }

        private void MapModelToChallenge(Challenge challenge, EditChallengeViewModel model)
        {
            MapCommonProperties(challenge, model);

            challengeMappers[model.ChallengeType].Map(challenge, model);
        }

        private void MapCommonProperties(Challenge challenge, EditChallengeViewModel model)
        {
            challenge.AuthorId = model.AuthorId;
            challenge.ChallengeType = (Data.Challenges.Enums.ChallengeType) model.ChallengeType;
            challenge.PreviewText = model.PreviewText;
            challenge.Condition = model.Condition;
            challenge.Difficulty = model.Difficulty;
            challenge.Language = model.Language;
            challenge.Section = (Section) model.Section;
            challenge.Title = model.Title;
            MapTags(challenge, model);
        }

        private void MapTags(Challenge challenge, EditChallengeViewModel model)
        {
            var removedTags = challenge.Tags.Where(tag => !model.Tags.Any(x => x.Equals(tag.Value)));

            foreach (var removedTag in removedTags)
            {
                tagService.Untag(challenge, removedTag.Value);
            }

            foreach (var tag in model.Tags)
            {
                tagService.Tag(challenge, tag);
            }
        }
    }
}