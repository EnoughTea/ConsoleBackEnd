using System;
using ConsoleBackEnd.Extensions;

namespace ConsoleBackEnd
{
    /// <summary> Metadata for a command return value. </summary>
    public class CommandReturnInfo : CommandIOPart, ICommandReturnInfo
    {
        /// <summary>
        ///     Gets the value indicating whether the command returns something or not (returns void).
        /// </summary>
        public bool HasValue => Type != null && Type != typeof(void);

        public CommandReturnInfo(Type type,
                                 string name = "",
                                 string description = "")
            : base(type, name, description) { }

        /// <inheritdoc />
        public override string ToString() =>
            string.IsNullOrWhiteSpace(Description)
                ? $"{Type.UnwrappedName()}"
                : $"{Type.UnwrappedName()} — {Description}";
    }
}