using System;
using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    public class CommandExecuteFailure : ICommandExecuteResult
    {
        /// <inheritdoc />
        public Exception Exception { get; }

        /// <inheritdoc />
        public bool IsSuccess => false;

        public CommandExecuteFailure(Exception e)
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