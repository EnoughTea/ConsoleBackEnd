using System;

namespace Compro
{
    public interface ICommandCallResult
    {
        object ReturnedValue { get; }

        string ConvertedValue { get; }

        Exception Exception { get; }

        bool IsSuccess { get; }
    }
}