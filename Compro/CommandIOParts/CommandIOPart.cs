using System;

namespace Compro {
    public abstract class CommandIOPart: ICommandIOPart
    {
        public string Name { get; }

        public string Description { get; }

        public Type Type { get; }

        public ICommandIOPartConverter Converter { get; }

        protected CommandIOPart(Type type,
                                ICommandIOPartConverter converter,
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