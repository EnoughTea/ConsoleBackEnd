using LanguageExt;

namespace Compro
{
    public class CommandExecuteSuccess : ICommandExecuteResult
    {
        public static CommandExecuteSuccess Void { get; } = new CommandExecuteSuccess();
        
        public object? ReturnedValue { get; }
        
        public bool HasValue { get; }

        /// <inheritdoc />
        public bool IsSuccess => true;

        public CommandExecuteSuccess(object returnedValue)
        {
            ReturnedValue = returnedValue;
            HasValue = true;
        }

        public Try<string> Convert() => JsonConverter.ToString(ReturnedValue);

        private CommandExecuteSuccess()
        {
            ReturnedValue = typeof(void);
        }
    }
}