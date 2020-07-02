using LanguageExt;

namespace ConsoleBackEnd
{
    public interface ICommandReturnedObjectConverter
    {
        /// <summary> Converts any object returned by ah executed command into a string. </summary>
        /// <param name="commandReturnedObject">Object returned by an executed command. </param>
        /// <returns>String representation of the given object.</returns>
        Try<string> Convert(object? commandReturnedObject);
    }
}