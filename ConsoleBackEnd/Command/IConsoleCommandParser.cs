namespace ConsoleBackEnd
{
    public interface IConsoleCommandParser
    {
        IConsoleCommandParseResult Parse(string commandRepr);
    }
}