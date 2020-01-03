using System;

namespace Compro
{
    public class CommandCallResult
    {
        public static CommandCallResult Void { get; } = new CommandCallResult();

        public string ReturnedValue { get; }

        public bool HasValue { get; }

        public Exception Exception { get; }

        public bool IsSuccess => ReferenceEquals(Exception, null);

        public CommandCallResult() => ReturnedValue = "";

        public CommandCallResult(Exception exception) => Exception = exception;

        public CommandCallResult(string returnedValue)
        {
            HasValue = true;
            ReturnedValue = returnedValue;
        }

        public override string ToString() => IsSuccess
            ? HasValue ? ReturnedValue ?? "<null>" : "<void>"
            : Exception.ToString();
    }
}