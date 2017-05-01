using System.Collections.Generic;
using System.Linq;
using Business.Challenges.ViewModels;
using Data.Challenges.Context;
using Data.Challenges.Entities;
using Data.Challenges.Enums;
using Shared.Framework.Dependency;
using ChallengeType = Business.Challenges.ViewModels.ChallengeType;

namespace Business.Challenges.Handlers
{
    [KeyedDependency(ChallengeType.CodeAnswered)]
    public class CodeAnsweredChallengeMapper : IChallengeMapper
    {
        private readonly IChallengesUnitOfWork unitOfWork;

        public CodeAnsweredChallengeMapper(IChallengesUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Map(Challenge challenge, EditChallengeViewModel model)
        {
            challenge.SolutionSourceCode = model.SourceCode;

            foreach (var removedTestCase in challenge.TestCases.ToList())
            {
                unitOfWork.Delete(removedTestCase);
            }

            foreach (var testCase in model.TestCases)
            {
                var testCaseEntity = new TestCase
                {
                    Id = testCase.Id,
                    IsPublic = testCase.IsPublic
                };

                var parameters = new List<CodeParameter>();
                parameters.AddRange(testCase.InputParameters.Select(p => new CodeParameter
                {
                    Type = CodeParameterType.Input,
                    Value = p
                }));
                parameters.AddRange(testCase.OutputParameters.Select(p => new CodeParameter
                {
                    Type = CodeParameterType.Output,
                    Value = p
                }));
                testCaseEntity.CodeParameters = parameters;

                challenge.TestCases.Add(testCaseEntity);
            }
        }
    }
}