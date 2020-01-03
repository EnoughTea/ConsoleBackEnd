using System;

namespace Compro {
    public abstract class CommandIOPart: ICommandIOPart
    {
        public string Name { get; }

        public string Description { get; }

        public Type Type { get; }

        public ICommandPieceConverter Converter { get; }

        protected CommandIOPart(Type type,
                                ICommandPieceConverter converter,
                                string name = "",
                                string description = "")
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }
    }
}