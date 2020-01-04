using System;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    public class ConsoleCommandParseSuccess : IConsoleCommandParseResult
    {
        public ParsedConsoleCommand Command { get; }

        /// <inheritdoc />
        public bool IsSuccess => true;

        public ConsoleCommandParseSuccess(string commandName, string[] commandArgs)
        {
            Command = new ParsedConsoleCommand(
                commandName ?? throw new ArgumentNullException(nameof(commandName)),
                commandArgs ?? throw new ArgumentNullException(nameof(commandArgs))
            );
        }

        public ConsoleCommandParseSuccess(string commandName) : this(commandName, ParsedConsoleCommand.ZeroArgs) { }
        
        /// <inheritdoc />
        public Try<ParsedConsoleCommand> Get()
        {
            Result<ParsedConsoleCommand> Try()
            {
                return Command;
            }

            return Try;
        }
    }
}