using System;
using LanguageExt;

namespace Compro
{
    public interface ICommandPieceConverter
    {
        Try<object> ConvertFromString(string repr, Type targetType);

        Try<string> ConvertToString(object value);
    }
}