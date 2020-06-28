using System;
using System.Globalization;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.Common;
using Newtonsoft.Json;

namespace Compro
{
    public static class JsonConverter
    {
        public static JsonSerializerSettings DefaultSettings { get; } = new JsonSerializerSettings {
            Culture = CultureInfo.InvariantCulture,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static Try<object?> FromString(string repr, Type targetType)
        {
            if (repr == null) throw new ArgumentNullException(nameof(repr));
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            Result<object?> Try()
            {
                return JsonConvert.DeserializeObject(repr, targetType, DefaultSettings);
            }

            return Try;
        }

        public static Try<string> ToString<T>(T value,
                                              JsonSerializerSettings? serializerSettings = null,
                                              bool unescape = true)
        {
            Result<string> Try()
            {
                string serialized = JsonConvert.SerializeObject(value, serializerSettings ?? DefaultSettings);
                string maybeUnescaped = unescape ? Regex.Unescape(serialized) : serialized;
                return !(value is null)
                    ? value.GetType().IsPrimitive ? maybeUnescaped.Replace("\"", "") : maybeUnescaped
                    : "null";
            }

            return Try;
        }
    }
}