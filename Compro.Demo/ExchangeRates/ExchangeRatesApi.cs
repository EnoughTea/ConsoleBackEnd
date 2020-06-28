using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Compro.Demo
{
    /// <summary> Provides commands to get foreign exchange rates. </summary>
    /// <remarks> Courtesy of European Central Bank via https://exchangeratesapi.io/ </remarks>
    public class ExchangeRatesApi
    {
        private const string BaseDescription = "Currency to quote rates against (\"EUR\" by default).";
        private const string SymbolsDescription = "Specific currencies to display (shows all by default).";

        private static readonly Uri _Home = new Uri("https://api.exchangeratesapi.io/", UriKind.Absolute);

        private static readonly HttpClient _HttpClient = new HttpClient();

        private static readonly ConcurrentDictionary<string, Task<string>> _RequestCache =
            new ConcurrentDictionary<string, Task<string>>();


        [CommandAlias("l")]
        [CommandExecutable("Gets the latest foreign exchange reference rates.")]
        public DayRates Latest([CommandDoc(BaseDescription)]
                               Currency @base = Currency.EUR,
                               [CommandDoc(SymbolsDescription)]
                               IEnumerable<Currency>? symbols = null) =>
            Fetch<DayRates>(new Uri(_Home, $"latest?base={@base}&{SymbolsQueryPart(symbols)}")).Result;

        [CommandAlias("d")]
        [CommandExecutable("Gets historical rates for any day since 1999.")]
        public DayRates Day([CommandDoc("Target day.")]
                            DateTime day,
                            [CommandDoc(BaseDescription)]
                            Currency @base = Currency.EUR,
                            [CommandDoc(SymbolsDescription)]
                            IEnumerable<Currency>? symbols = null) =>
            Fetch<DayRates>(new Uri(_Home, $"{ToIsoDate(day)}?base={@base}&{SymbolsQueryPart(symbols)}")).Result;

        [CommandAlias("h")]
        [CommandExecutable("Gets historical rates for a time period.")]
        public HistoryRates History([CommandDoc("First day of the target period.")]
                                    DateTime start,
                                    [CommandDoc("Last day of the target period.")]
                                    DateTime end,
                                    [CommandDoc(BaseDescription)]
                                    Currency @base = Currency.EUR,
                                    [CommandDoc(SymbolsDescription)]
                                    IEnumerable<Currency>? symbols = null) =>
            Fetch<HistoryRates>(new Uri(_Home, $"history?base={@base}&start_at={ToIsoDate(start)}" +
                $"&end_at={ToIsoDate(end)}&{SymbolsQueryPart(symbols)}")).Result;


        private static async Task<TResponse> Fetch<TResponse>(Uri uri)
        {
            async Task<string> GetFromExchangeRatesApi(string uriRepr) => await _HttpClient.GetStringAsync(uri);

            string response = await _RequestCache.GetOrAdd(uri.ToString(), GetFromExchangeRatesApi);
            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        private static string SymbolsQueryPart(IEnumerable<Currency>? symbols) =>
            symbols != null ? $"symbols={string.Join(',', symbols)}" : "";

        private static string ToIsoDate(DateTime value) => value.ToString("yyyy-MM-dd");
    }
}