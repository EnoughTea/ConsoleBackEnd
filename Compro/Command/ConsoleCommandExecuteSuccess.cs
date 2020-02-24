using LanguageExt;

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

        public Try<string> Convert() => JsonConverter.ToString(ReturnedValue);

        private ConsoleCommandExecuteSuccess()
        {
            ReturnedValue = typeof(void);
        }
    }
}