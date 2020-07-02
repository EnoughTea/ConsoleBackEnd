using LanguageExt;

namespace ConsoleBackEnd
{
    /// <summary> Base for a command execution result, either success or failure. </summary>
    public interface ICommandExecuteResult
    {
        /// <summary> <c>true</c> if command was successfully executed; <c>false otherwise</c>. </summary>
        bool IsSuccess { get; }

        /// <summary>
        ///     Converts object returned by a successfully executed command to string with the help of a given converter.
        /// </summary>
        /// <param name="resultConverter">Converter used to convert command result to a string.</param>
        /// <returns>String representation of the command return value.</returns>
        Try<string> Convert(ICommandReturnedObjectConverter resultConverter);

        /// <summary>
        ///     Converts object returned by a successfully executed command to string with the help of a json serializer.
        /// </summary>
        /// <param name="resultConverter">Converter used to convert command result to a string.</param>
        /// <returns>String representation of the command return value.</returns>
        Try<string> Convert() => Convert(CommandReturnedObjectJsonConverter.Default);

        /// <summary>
        ///     Converts object returned by a successfully executed command to string with the help of a given converter.
        /// </summary>
        /// <param name="resultConverter">Converter used to convert command result to a string.</param>
        /// <returns>String representation of either the command return value or command execution error.</returns>
        string ConvertOrError(ICommandReturnedObjectConverter resultConverter) =>
            Convert(resultConverter).IfFail(e => e.ToString());

        /// <summary>
        ///     Converts object returned by a successfully executed command to string with the help of a json serializer.
        /// </summary>
        /// <returns>String representation of either the command return value or command execution error.</returns>
        string ConvertOrError() => ConvertOrError(CommandReturnedObjectJsonConverter.Default);
    }
}