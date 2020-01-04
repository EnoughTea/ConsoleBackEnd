using System;

namespace Compro
{
    public class CommandReturnInfo : CommandIOPart
    {
        public bool HasValue => Type != null && Type != typeof(void);

        public CommandReturnInfo(Type type,
                                 string name = "",
                                 string description = "")
            : base(type, name, description) { }
    }
}