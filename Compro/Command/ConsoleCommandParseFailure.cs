using System;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    public class ConsoleCommandParseFailure : IConsoleCommandParseResult
    {
        public static ConsoleCommandParseFailure Fail(string message = "") =>
            new ConsoleCommandParseFailure(new ArgumentException(message));
        
        public Exception Exception { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;
        
        public ConsoleCommandParseFailure(Exception e)
        {
            Exception = e ?? throw new ArgumentNullException(nameof(e));
        }

        /// <inheritdoc />
        public Try<ParsedConsoleCommand> Get()
        {
            Result<ParsedConsoleCommand> Try()
            {
                throw Exception;
            }

            return Try;
        }
    }
}