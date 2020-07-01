using System;

namespace ConsoleBackEnd
{
    /// <summary>
    ///     Metadata for input/output part of a command: either a command argument or a command return value.
    /// </summary>
    public interface ICommandIOPart
    {
        string Name { get; }

        string Description { get; }

        Type Type { get; }
    }
}