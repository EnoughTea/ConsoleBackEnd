using System;

namespace ConsoleBackEnd
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandExecutableAttribute : Attribute
    {
        public string Description { get; }

        public CommandExecutableAttribute(string description = "") => Description = description;
    }
}