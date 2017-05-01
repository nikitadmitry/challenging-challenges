using System;
using System.Collections.Generic;
using System.Linq;
using Business.Challenges.ViewModels;
using Business.CodeExecution;
using Business.Identity;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Challenges.Enums;
using Shared.Framework.Dependency;
using Shared.Framework.Resources;
using Shared.Framework.Validation;
using ChallengeType = Business.Challenges.ViewModels.ChallengeType;

namespace Business.Challenges.Private
{
    [KeyedDependency(ChallengeType.CodeAnswered)]
    internal class TestCaseChallengeSolvingStrategy : ChallengeSolvingStrategyBase
    {
        private readonly ICodeExecutor codeExecutor;
        private readonly CodeExecutionRequestBuilder codeExecutionRequestBuilder;

        public TestCaseChallengeSolvingStrategy(
            IChallengesUnitOfWork unitOfWork,
            ICodeExecutor codeExecutor,
            IIdentityService identityService,
            CodeExecutionRequestBuilder codeExecutionRequestBuilder)
            : base(unitOfWork, identityService)
        {
            this.codeExecutor = codeExecutor;
            this.codeExecutionRequestBuilder = codeExecutionRequestBuilder;
        }

        protected override ChallengeSolveResult ValidateAnswer(Challenge challenge, string answer)
        {
            Contract.Assert<InvalidOperationException>(challenge.Section != Section.Other,
                "Other section cannot contain test-cased challenges");

            var testCaseValidationResult = new ChallengeSolveResult
            {
                IsSolved = true
            };

            foreach (var testCase in challenge.TestCases.OrderByDescending(x => x.IsPublic))
            {
                var codeExecutionRequest = codeExecutionRequestBuilder.Build(challenge.Section, answer, 
                    testCase.InputParameters.Select(x => x.Value));

                var codeExecutionResult = codeExecutor.Execute(codeExecutionRequest);

                testCaseValidationResult.IsSolved = codeExecutionResult.IsValid
                    && CheckOutput(testCase.OutputParameters, codeExecutionResult.Output);

                if (!testCaseValidationResult.IsSolved)
                {
                    testCaseValidationResult.ErrorMessage = codeExecutionResult.IsValid
                        ? (testCase.IsPublic ? GetTestCaseExecutionMessage(testCase.InputParameters, codeExecutionResult.Output) : "Private Test Case Failed")
                        : codeExecutionResult.ErrorMessage;

                    break;
                }
            }

            return testCaseValidationResult;
        }

        private string GetTestCaseExecutionMessage(IEnumerable<CodeParameter> testCaseInputParameters, 
            IEnumerable<string> codeExecutionOutput)
        {
            return string.Format(Localization.WrongTestCaseOutputTemplate, 
                string.Join(", ", testCaseInputParameters.Select(x => x.Value)),
                string.Join(", ", codeExecutionOutput));
        }

        private bool CheckOutput(IEnumerable<CodeParameter> testCaseOutputParameters, 
            IEnumerable<string> codeOutput)
        {
            return testCaseOutputParameters.Select(x => x.Value).SequenceEqual(codeOutput);
        }
    }
}