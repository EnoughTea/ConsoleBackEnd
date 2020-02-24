namespace Compro
{
    public interface IConsoleCommandParser
    {
        IConsoleCommandParseResult Parse(string commandRepr);
    }
}