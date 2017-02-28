namespace Business.CodeExecution.Private
{
    internal enum CodeExecutionStatus
    {
        Success = 0,
        WaitingForCompilation = -1,
        Compilation = 1,
        Running = 3
    }
}