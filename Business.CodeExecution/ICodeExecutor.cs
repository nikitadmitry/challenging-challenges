using Business.CodeExecution.ViewModels;

namespace Business.CodeExecution
{
    public interface ICodeExecutor
    {
        CodeExecutionResponse Execute(CodeExecutionRequest codeExecutionRequest);
    }
}