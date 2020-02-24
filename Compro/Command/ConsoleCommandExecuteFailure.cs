using System;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    /// <inheritdoc />
    public class ConsoleCommandExecuteFailure : IConsoleCommandExecuteFailure
    {
        public Exception Exception { get; }

        public bool IsSuccess => false;

        public ConsoleCommandExecuteFailure(Exception e)
        {
            Exception = e ?? throw new ArgumentNullException(nameof(e));
        }

        public Try<string> Convert()
        {
            Result<string> Try()
            {
                throw Exception;
            }

            return Try;
        }
    }
}