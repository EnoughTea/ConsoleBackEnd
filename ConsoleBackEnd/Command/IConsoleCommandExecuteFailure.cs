using System;

namespace ConsoleBackEnd
{
    /// <summary> Represents a failure when executing a command. </summary>
    public interface IConsoleCommandExecuteFailure : ICommandExecuteResult
    {
        /// <summary> Gets the cause of this failure. </summary>
        Exception Exception { get; }
    }
}