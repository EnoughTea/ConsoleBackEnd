using System;
using LanguageExt;

namespace Compro
{
    public interface ICommandExecuteResult
    {
        bool IsSuccess { get; }

        Try<string> Convert();
        
        string ConvertOrError() => Convert().IfFail(e => e.ToString());
    }
}