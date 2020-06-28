using LanguageExt;
using LanguageExt.Common;
using Newtonsoft.Json;

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

        public Try<string> Convert(JsonSerializerSettings? serializerSettings = null, bool unescape = true)
        {
            static Result<string> NoValueRepr() => "";
            return HasValue ? JsonConverter.ToString(ReturnedValue, serializerSettings, unescape) : NoValueRepr;
        }

        private ConsoleCommandExecuteSuccess()
        {
            ReturnedValue = typeof(void);
        }
    }
}