using System;

namespace Compro
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandExecutableAttribute : Attribute
    {
        public string Description { get; }

        public CommandExecutableAttribute(string description = "") => Description = description;
    }
}