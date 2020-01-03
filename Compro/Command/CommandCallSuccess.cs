using System;

namespace Compro
{
    public class CommandCallSuccess : ICommandCallResult
    {
        public static CommandCallSuccess Void { get; } = new CommandCallSuccess();

        /// <inheritdoc />
        public object ReturnedValue { get; }

        /// <inheritdoc />
        public string ConvertedValue { get; }

        /// <inheritdoc />
        public Exception Exception { get; private set; }

        /// <inheritdoc />
        public bool IsSuccess => true;

        public CommandCallSuccess(object returnedValue, ICommandIOPartConverter converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            ReturnedValue = returnedValue;
            var attemptedConvert = converter.ConvertToString(ReturnedValue);
            ConvertedValue = attemptedConvert.Match(s => s, e => {
                Exception = e;
                return e.ToString();
            });
        }

        private CommandCallSuccess()
        {
            ReturnedValue = "void";
            ConvertedValue = "";
        }
    }
}