using System;

namespace Compro
{
    /// <inheritdoc />
    public abstract class CommandIOPart : ICommandIOPart
    {
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public Type Type { get; }

        protected CommandIOPart(Type type,
                                string name = "",
                                string description = "")
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}