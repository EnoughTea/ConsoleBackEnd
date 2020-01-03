using System;
using System.Globalization;
using LanguageExt;
using Newtonsoft.Json;

namespace Compro
{
    public class JsonCommandPieceConverter : ICommandPieceConverter
    {
        private static readonly JsonSerializerSettings _DefaultSettings = new JsonSerializerSettings {
            Culture = CultureInfo.InvariantCulture,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };

        public JsonSerializerSettings Settings { get; }

        public JsonCommandPieceConverter()
        {
            Settings = _DefaultSettings;
        }

        public JsonCommandPieceConverter(JsonSerializerSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public Try<object> ConvertFromString(string repr, Type targetType) =>
            () => JsonConvert.DeserializeObject(repr, targetType, Settings);

        public Try<string> ConvertToString(object value) => () => JsonConvert.SerializeObject(value, Settings);
    }
}