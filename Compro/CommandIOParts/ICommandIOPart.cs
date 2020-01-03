using System;

namespace Compro {
    public interface ICommandIOPart
    {
        string Name { get; }

        string Description { get; }

        Type Type { get; }

        ICommandIOPartConverter Converter { get; }
    }
}