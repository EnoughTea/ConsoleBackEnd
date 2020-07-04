using LanguageExt;

namespace ConsoleBackEnd
{
    /// <summary>
    /// Interface for a class used to convert object returned by a command into its string representation.
    /// </summary>
    public interface ICommandReturnedObjectConverter
    {
        /// <summary>
        ///     Gets or sets value toggling whether the <see cref="ICommandExecuteResult.Convert()" />
        ///     should print entire stack trace for exceptions or just the error message.
        /// </summary>
        bool PrintErrorStackTrace { get; set; }

        /// <summary> Converts any object returned by ah executed command into a string. </summary>
        /// <param name="commandReturnedObject">Object returned by an executed command. </param>
        /// <returns>String representation of the given object.</returns>
        Try<string> Convert(object? commandReturnedObject);
    }
}