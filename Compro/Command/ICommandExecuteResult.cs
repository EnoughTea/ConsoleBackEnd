using LanguageExt;

namespace Compro
{
    /// <summary> Base for a command execution result, either success or failure. </summary>
    public interface ICommandExecuteResult
    {
        bool IsSuccess { get; }

        Try<string> Convert();
        
        string ConvertOrError() => Convert().IfFail(e => e.ToString());
    }
}