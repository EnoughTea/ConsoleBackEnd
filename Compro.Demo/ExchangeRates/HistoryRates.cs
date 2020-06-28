using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Compro.Demo
{
    public class HistoryRates
    {
        [JsonProperty("rates")]
        public Dictionary<DateTime, Dictionary<Currency, decimal>> Rates { get; set; } =
            new Dictionary<DateTime, Dictionary<Currency, decimal>>();

        [JsonProperty("base")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Base { get; set; }

        [JsonProperty("start_at")]
        public DateTime StartAt { get; set; }

        [JsonProperty("end_at")]
        public DateTime EndAt { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            string ratesPart = Rates.Aggregate("", (acc, curr) =>
                $"{acc}{Environment.NewLine}{curr.Key.ToShortDateString()}: {DayRates.RatesToString(curr.Value)}");
            return $"{StartAt.ToShortDateString()}–{EndAt.ToShortDateString()} exchange rates for 1 {Base} " +
                ratesPart;
        }
    }
}