using System;
using System.Text.RegularExpressions;
using LanguageExt;
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

            object? Deserialize() =>
                JsonConvert.DeserializeObject(repr, targetType, serializerSettings ?? DeserializationSettings);

            return () => Deserialize();
        }

        public static Try<string> ToString<T>(T value,
                                              bool unescape = true,
                                              Formatting formatting = Formatting.Indented,
                                              JsonSerializerSettings? serializerSettings = null)
        {
            string Serialize()
            {
                if (value is null) return "null";

                string serialized = JsonConvert.SerializeObject(value, formatting, serializerSettings);
                string maybeUnescaped = unescape ? Regex.Unescape(serialized) : serialized;
                return maybeUnescaped;
            }

            return () => Serialize();
        }
    }
}