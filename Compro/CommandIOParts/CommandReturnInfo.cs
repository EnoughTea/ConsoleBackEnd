using System;

namespace Compro
{
    /// <summary> Metadata for a command return value. </summary>
    public class CommandReturnInfo : CommandIOPart, ICommandReturnInfo
    {
        /// <summary>
        /// Gets the value indicating whether the command returns something or not (returns void).
        /// </summary>
        public bool HasValue => Type != null && Type != typeof(void);

        public CommandReturnInfo(Type type,
                                 string name = "",
                                 string description = "")
            : base(type, name, description) { }
    }
}