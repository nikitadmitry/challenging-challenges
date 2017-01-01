using System.Collections.Generic;
using System.Linq;
using Business.CodeExecution;
using Business.CodeExecution.ViewModels;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Tests
{
    [TestClass]
    public class CodeExecutorTests
    {
        private ICodeExecutor subject;
        private const string Parameter1 = "parameter1";
        private const string Parameter2 = "parameter2";

        [TestInitialize]
        public void Initialize()
        {
            subject = new CodeExecutor();
        }

        [TestMethod]
        public void Execute_ValidPythonRequestProvided_ValidResultReturned()
        {
            var request = new CodeExecutionRequest
            {
                CodeLanguage = CodeLanguage.Python,
                SourceCode = "import fileinput" +
                    "\nfor line in fileinput.input():" +
                    "\n    print line",
                Input = new List<string>
                {
                    Parameter1, Parameter2
                }
            };

            var response = subject.Execute(request);

            response.Should().NotBeNull();
            response.IsValid.Should().BeTrue();
            response.Output.Should().NotBeNull();
            var output = response.Output.ToList();
            output[0].Should().Be(Parameter1);
            output[1].Should().Be(Parameter2);
        }
    }
}
