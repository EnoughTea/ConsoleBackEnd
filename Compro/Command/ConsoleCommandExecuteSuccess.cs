using LanguageExt;
using LanguageExt.Common;

namespace Compro
{
    /// <inheritdoc />
    public class ConsoleCommandExecuteSuccess : IConsoleCommandExecuteSuccess
    {
        public static ConsoleCommandExecuteSuccess Void { get; } = new ConsoleCommandExecuteSuccess();

        public object? ReturnedValue { get; }

        public bool HasValue { get; }

        public bool IsSuccess => true;

        public ConsoleCommandExecuteSuccess(object returnedValue)
        {
            ReturnedValue = returnedValue;
            HasValue = true;
        }

        public Try<string> Convert(bool unescape = true)
        {
            static Result<string> NoValueRepr() => "";
            return HasValue ? JsonConverter.ToString(ReturnedValue, unescape) : NoValueRepr;
        }

        private ConsoleCommandExecuteSuccess()
        {
            ReturnedValue = typeof(void);
        }
    }
}