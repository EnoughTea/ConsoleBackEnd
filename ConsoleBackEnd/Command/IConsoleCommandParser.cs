namespace ConsoleBackEnd
{
    /// <summary>
    ///     Interface for classes able to parse user input into a registered command with parameters array.
    /// </summary>
    public interface IConsoleCommandParser
    {
        IConsoleCommandParseResult Parse(string commandRepr);
    }
}