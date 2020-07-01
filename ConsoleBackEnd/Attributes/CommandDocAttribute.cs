using System;

namespace ConsoleBackEnd
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class CommandDocAttribute : Attribute
    {
        public string Description { get; }

        public CommandDocAttribute(string description = "") => Description = description;
    }
}