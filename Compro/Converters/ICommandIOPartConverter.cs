using System;
using LanguageExt;

namespace Compro
{
    public interface ICommandIOPartConverter
    {
        Try<object> ConvertFromString(string repr, Type targetType);

        Try<string> ConvertToString<T>(T value);
    }
}