using System;
using LanguageExt;
using static ConsoleBackEnd.TryHelper;

namespace ConsoleBackEnd
{
    public class ConsoleCommandParseFailure : IConsoleCommandParseResult
    {
        public Exception Exception { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        public ConsoleCommandParseFailure(Exception e) => Exception = e ?? throw new ArgumentNullException(nameof(e));

        public static ConsoleCommandParseFailure Fail(string message = "") =>
            new ConsoleCommandParseFailure(new ArgumentException(message));

        /// <inheritdoc />
        public Try<IParsedConsoleCommand> Get() => Fail<IParsedConsoleCommand>(Exception);
    }
}