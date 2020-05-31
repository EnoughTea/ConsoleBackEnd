using System;
using System.Globalization;
using LanguageExt;
using LanguageExt.Common;
using Newtonsoft.Json;

namespace Compro
{
    internal static class JsonConverter
    {
        private static readonly JsonSerializerSettings _DefaultSettings = new JsonSerializerSettings {
            Culture = CultureInfo.InvariantCulture,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };

        public static Try<object?> FromString(string repr, Type targetType)
        {
            if (repr == null) throw new ArgumentNullException(nameof(repr));
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            Result<object?> Try()
            {
                return JsonConvert.DeserializeObject(repr, targetType, _DefaultSettings);
            }

            return Try;
        }

        public static Try<string> ToString<T>(T value)
        {
            Result<string> Try()
            {
                string serialized = JsonConvert.SerializeObject(value, _DefaultSettings);
                return !(value is null)
                    ? value.GetType().IsPrimitive ? serialized.Replace("\"", "") : serialized
                    : "null";
            }

            return Try;
        }
    }
}