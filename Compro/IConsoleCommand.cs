using System.Collections.Generic;

namespace Compro
{
    /// <summary> Represents a terminal command which can be executed with given arguments. </summary>
    public interface IConsoleCommand
    {
        string Name { get; }
        
        IReadOnlyList<string> Aliases { get; }

        string Description { get; }

        IReadOnlyList<CommandParameterInfo> Parameters { get; }

        ICommandReturnInfo Result { get; }

        ICommandExecuteResult Execute(params string[] args);

        bool CanBeCalledBy(string name);
    }
}