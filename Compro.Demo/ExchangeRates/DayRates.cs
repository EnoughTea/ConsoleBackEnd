using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Compro.Demo
{
    public class DayRates
    {
        [JsonProperty("rates")]
        public Dictionary<Currency, decimal> Rates { get; set; } = new Dictionary<Currency, decimal>();

        [JsonProperty("base")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Base { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Date.ToShortDateString()} exchange rates for 1 {Base} " + RatesToString(Rates);
        }

        internal static string RatesToString(Dictionary<Currency, decimal> rates) =>
            rates != null
                ? rates.Aggregate("", (acc, curr) => $"{acc}{Environment.NewLine}{curr.Key}: {curr.Value}")
                : "<null>";
    }
}