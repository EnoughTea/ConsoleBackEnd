using System;
using LanguageExt;

namespace Compro
{
    public interface IConsoleCommandParseResult
    {
        bool IsSuccess { get; }

        Try<IParsedConsoleCommand> Get();

        IParsedConsoleCommand GetOrEmpty() => Get().IfFail(ParsedConsoleCommand.Empty);
    }
}