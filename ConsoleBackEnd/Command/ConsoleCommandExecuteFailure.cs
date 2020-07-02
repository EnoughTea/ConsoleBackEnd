using System;
using LanguageExt;
using Newtonsoft.Json;
using static ConsoleBackEnd.TryHelper;

namespace ConsoleBackEnd
{
    /// <inheritdoc />
    public class ConsoleCommandExecuteFailure : IConsoleCommandExecuteFailure
    {
        public Exception Exception { get; }

        public bool IsSuccess => false;

        public ConsoleCommandExecuteFailure(Exception e) => Exception = e ?? throw new ArgumentNullException(nameof(e));

        public Try<string> Convert(ICommandReturnedObjectConverter resultConverter) =>
            Fail<string>(Exception);
    }
}