using System;

namespace Compro
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAliasAttribute : Attribute
    {
        public string Alias { get; }

        public CommandAliasAttribute(string alias) => Alias = alias ?? throw new ArgumentNullException(nameof(alias));
    }
}