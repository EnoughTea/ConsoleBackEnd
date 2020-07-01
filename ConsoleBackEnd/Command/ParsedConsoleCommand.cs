using System;

namespace ConsoleBackEnd
{
    public class ParsedConsoleCommand : IParsedConsoleCommand
    {
        internal static readonly string[] ZeroArgs = new string[0];

        public static ParsedConsoleCommand Empty { get; } = new ParsedConsoleCommand("", ZeroArgs);

        public string Name { get; }

        public string[] Args { get; }

        public ParsedConsoleCommand(string name, string[] args)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Args = args ?? throw new ArgumentNullException(nameof(args));
        }
    }
}