using System;
using System.Globalization;
using LanguageExt;

namespace Compro
{
    public class ConvertibleCommandPieceConverter : ICommandPieceConverter
    {
        public Try<object> ConvertFromString(string repr, Type targetType) =>
            () => Convert.ChangeType(repr, targetType, CultureInfo.InvariantCulture);

        public Try<string> ConvertToString(object value) =>
            () => Convert.ChangeType(value, TypeCode.String, CultureInfo.InvariantCulture) as string;
    }
}