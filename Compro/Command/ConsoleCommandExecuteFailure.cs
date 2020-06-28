using System;
using LanguageExt;
using LanguageExt.Common;
using Newtonsoft.Json;

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

        public Try<string> Convert(JsonSerializerSettings? serializerSettings = null, bool unescape = true)
        {
            Result<string> Try()
            {
                throw Exception;
            }

            return Try;
        }
    }
}