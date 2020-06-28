using LanguageExt;

namespace Compro
{
    /// <summary> Base for a command execution result, either success or failure. </summary>
    public interface ICommandExecuteResult
    {
        /// <summary> <c>true</c> if command was successfully executed; <c>false otherwise</c>. </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Converts object returned by a successfully executed command to string with the help of a json serializer.
        /// </summary>
        /// <param name="unescape">If true, returned string will be unescaped.</param>
        /// <returns>String representation of the command return value.</returns>
        Try<string> Convert(bool unescape = true);

        /// <summary>
        /// Converts object returned by a successfully executed command to string with the help of a json serializer.
        /// </summary>
        /// <param name="unescape">If true, returned string will be unescaped.</param>
        /// <returns>String representation of either the command return value or command execution error.</returns>
        string ConvertOrError(bool unescape = true) => Convert(unescape).IfFail(e => e.ToString());
    }
}