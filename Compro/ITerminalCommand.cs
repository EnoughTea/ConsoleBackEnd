using System;
using System.Collections.Generic;

namespace Compro
{
    /// <summary> Represents a terminal command which can be executed with given arguments. </summary>
    public interface ITerminalCommand
    {
        string Name { get; }

        string Description { get; }

        IReadOnlyList<CommandParameterInfo> Parameters { get; }

        CommandReturnInfo Result { get; }

        CommandCallResult Call(params string[] args);
    }
}