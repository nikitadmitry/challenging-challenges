using System;
using System.Collections.Generic;
using Business.CodeExecution.ViewModels;
using Data.Challenges.Enums;
using Shared.Framework.Dependency;

namespace Business.Challenges.Private
{
    public class CodeExecutionRequestBuilder : IDependency
    {
        public CodeExecutionRequest Build(Section section, string answer, IEnumerable<string> input)
        {
            return new CodeExecutionRequest
            {
                CodeLanguage = ConvertSectionToCodeLanguage(section),
                Input = input,
                SourceCode = answer
            };
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