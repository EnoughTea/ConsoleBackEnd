using LanguageExt;
using LanguageExt.Common;

namespace ConsoleBackEnd
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

        private ConsoleCommandExecuteSuccess() => ReturnedValue = typeof(void);

        public Try<string> Convert(ICommandReturnedObjectConverter resultConverter)
        {
            static Result<string> NoValueRepr() => "";
            return HasValue ? resultConverter.Convert(ReturnedValue) : NoValueRepr;
        }
    }
}