namespace ConsoleBackEnd
{
    /// <summary> Represents a success when executing a command. </summary>
    public interface IConsoleCommandExecuteSuccess : ICommandExecuteResult
    {
        /// <summary>
        ///     Gets a result of a command execution.
        ///     Always null when command does not have a return value.
        /// </summary>
        object? ReturnedValue { get; }

        /// <summary> Gets a value indicating whether a command has a return value or not. </summary>
        bool HasValue { get; }
    }
}