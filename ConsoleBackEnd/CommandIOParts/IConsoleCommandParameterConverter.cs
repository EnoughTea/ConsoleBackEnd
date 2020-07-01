using LanguageExt;

namespace ConsoleBackEnd
{
    public interface IConsoleCommandParameterConverter
    {
        /// <summary>
        ///     Converts given string representation of a command parameter to the actual object passed to the command.
        /// </summary>
        /// <param name="parameterInfo">Command parameter metadata to assist in the conversion.</param>
        /// <param name="parameter">String representation of a command parameter .</param>
        /// <returns>Either success with a command argument or failure.</returns>
        Try<object?> Convert(ICommandIOPart parameterInfo, string parameter);
    }
}