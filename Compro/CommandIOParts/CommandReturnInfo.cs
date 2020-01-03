using System;

namespace Compro
{
    public class CommandReturnInfo : CommandIOPart
    {
        public bool HasValue => Type != null && Type != typeof(void);

        public CommandReturnInfo(Type type,
                                 ICommandIOPartConverter converter,
                                 string name = "",
                                 string description = "")
            : base(type, converter, name, description) { }
    }
}