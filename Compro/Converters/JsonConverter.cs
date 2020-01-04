using System;
using System.Globalization;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using Newtonsoft.Json;

namespace Compro
{
    public class JsonConverter : ICommandIOPartConverter
    {
        private static readonly JsonSerializerSettings _DefaultSettings = new JsonSerializerSettings {
            Culture = CultureInfo.InvariantCulture,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };

        public JsonSerializerSettings Settings { get; }

        public JsonConverter() => Settings = _DefaultSettings;

        public JsonConverter(JsonSerializerSettings settings) =>
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

        public Try<object> ConvertFromString(string repr, Type targetType)
        {
            if (repr == null) throw new ArgumentNullException(nameof(repr));
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));

            return Deserialize(repr, targetType);
        }

        public Try<string> ConvertToString<T>(T value)
        {
            return Serialize(value);
        }

        // Stuff below used to avoid closure allocations.

        private static Try<string> SerializeNoOp(string value)
        {
            Result<string> Try()
            {
                return value;
            }

            return Try;
        }

        private Try<string> Serialize<T>(T value)
        {
            Result<string> Try()
            {
                string serialized = JsonConvert.SerializeObject(value, Settings);
                return value.GetType().IsPrimitive ? serialized.Replace("\"", "") : serialized;
            }

            return Try;
        }

        private static Try<object> DeserializeNoOp<T>(T valueRepr)
        {
            Result<object> Try()
            {
                return valueRepr;
            }

            return Try;
        }

        private Try<object> Deserialize(string valueRepr, Type targetType)
        {
            Result<object> Try()
            {
                return JsonConvert.DeserializeObject(valueRepr, targetType, Settings);
            }

            return Try;
        }
    }
}