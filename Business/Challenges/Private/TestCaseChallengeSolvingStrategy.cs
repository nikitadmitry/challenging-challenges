using System;
using System.Collections.Generic;
using System.Linq;
using Business.Challenges.ViewModels;
using Business.CodeExecution;
using Business.CodeExecution.ViewModels;
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

        public TestCaseChallengeSolvingStrategy(
            IChallengesUnitOfWork unitOfWork,
            ICodeExecutor codeExecutor)
            : base(unitOfWork)
        {
            this.codeExecutor = codeExecutor;
        }

        protected override ChallengeSolveResult ValidateAnswer(Challenge challenge, string answer)
        {
            Contract.Assert<InvalidOperationException>(challenge.Section != Section.Other,
                "Other section cannot contain test-cased challenges");

            var testCaseValidationResult = new ChallengeSolveResult
            {
                IsSolved = true
            };

            foreach (var testCase in challenge.TestCases)
            {
                var codeExecutionRequest = BuildCodeExecutionRequest(challenge, answer, testCase);

                var codeExecutionResult = codeExecutor.Execute(codeExecutionRequest);

                testCaseValidationResult.IsSolved = codeExecutionResult.IsValid
                    && CheckOutput(testCase.OutputParameters, codeExecutionResult.Output);

                if (!testCaseValidationResult.IsSolved)
                {
                    testCaseValidationResult.ErrorMessage = codeExecutionResult.IsValid
                        ? GetTestCaseExecutionMessage(testCase.InputParameters, codeExecutionResult.Output)
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

        private CodeExecutionRequest BuildCodeExecutionRequest(Challenge challenge, 
            string answer, TestCase testCase)
        {
            var codeExecutionRequest = new CodeExecutionRequest
            {
                CodeLanguage = ConvertSectionToCodeLanguage(challenge.Section),
                Input = testCase.InputParameters.Select(x => x.Value),
                SourceCode = answer
            };
            return codeExecutionRequest;
        }

        private bool CheckOutput(IEnumerable<CodeParameter> testCaseOutputParameters, 
            IEnumerable<string> codeOutput)
        {
            return testCaseOutputParameters.Select(x => x.Value).SequenceEqual(codeOutput);
        }

        private CodeLanguage ConvertSectionToCodeLanguage(Section challengeSection)
        {
            switch (challengeSection)
            {
                case Section.CSharp:
                    return CodeLanguage.CSharp;
                case Section.Java:
                    return CodeLanguage.Java;
                case Section.Python:
                    return CodeLanguage.Python;
                case Section.Ruby:
                    return CodeLanguage.Ruby;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}