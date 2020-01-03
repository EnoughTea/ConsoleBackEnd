using System;

namespace Compro {
    public class CommandCallFailure : ICommandCallResult
    {
        public CommandCallFailure(Exception e)
        {
            ReturnedValue = CommandCallSuccess.Void.ReturnedValue;
            ConvertedValue = CommandCallSuccess.Void.ConvertedValue;
        }

        /// <inheritdoc />
        public object ReturnedValue { get; }

        /// <inheritdoc />
        public string ConvertedValue { get; }

        /// <inheritdoc />
        public Exception Exception { get; }

        /// <inheritdoc />
        public bool IsSuccess { get; }

        /// <inheritdoc />
        public bool HasValue { get; }
    }
}