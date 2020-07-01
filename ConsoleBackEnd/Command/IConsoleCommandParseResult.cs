using LanguageExt;

namespace ConsoleBackEnd
{
    public interface IConsoleCommandParseResult
    {
        bool IsSuccess { get; }

        Try<IParsedConsoleCommand> Get();

        IParsedConsoleCommand GetOrEmpty() => Get().IfFail(ParsedConsoleCommand.Empty);
    }
}