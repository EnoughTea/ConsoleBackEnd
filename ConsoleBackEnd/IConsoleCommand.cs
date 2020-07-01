using System.Collections.Generic;

namespace ConsoleBackEnd
{
    /// <summary> Represents a terminal command which can be executed with given arguments. </summary>
    public interface IConsoleCommand
    {
        string Name { get; }

        IReadOnlyList<string> Aliases { get; }

        string Description { get; }

        IReadOnlyList<CommandParameterInfo> Parameters { get; }

        ICommandReturnInfo Result { get; }

        ICommandExecuteResult Execute(IConsoleCommandParameterConverter argConverter, params string[] args);

        bool CanBeCalledBy(string name);
    }
}