using System;
using LanguageExt;

namespace Compro
{
    public interface IConsoleCommandParseResult
    {
        bool IsSuccess { get; }

        Try<ParsedConsoleCommand> Get();

        ParsedConsoleCommand GetOrEmpty() => Get().IfFail(ParsedConsoleCommand.Empty);
    }
}