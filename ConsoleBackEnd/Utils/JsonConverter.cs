using System;
using System.Text.RegularExpressions;
using LanguageExt;
using LanguageExt.Common;
using Newtonsoft.Json;
using static ConsoleBackEnd.TryHelper;

namespace ConsoleBackEnd
{
    internal static class JsonConverter
    {
        private static JsonSerializerSettings DeserializationSettings { get; } = new JsonSerializerSettings {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static Try<object?> FromString(string repr,
                                              Type targetType,
                                              JsonSerializerSettings? serializerSettings = null)
        {
            if (repr == null) return FailNull<object?>(nameof(repr));
            if (targetType == null) return FailNull<object?>(nameof(targetType));

            Result<object?> Try() =>
                JsonConvert.DeserializeObject(repr, targetType, serializerSettings ?? DeserializationSettings);

            return Try;
        }

        public static Try<string> ToString<T>(T value,
                                              JsonSerializerSettings? serializerSettings = null,
                                              bool unescape = true)
        {
            Result<string> Try()
            {
                string serialized = JsonConvert.SerializeObject(value, serializerSettings);
                string maybeUnescaped = unescape ? Regex.Unescape(serialized) : serialized;
                return !(value is null)
                    ? value.GetType().IsPrimitive ? maybeUnescaped.Replace("\"", "") : maybeUnescaped
                    : "null";
            }

            return Try;
        }
    }
}